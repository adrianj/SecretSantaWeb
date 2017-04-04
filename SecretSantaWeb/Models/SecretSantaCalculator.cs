using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretSantaWeb.Models
{
    public class SecretSantaCalculator
    {
        public Dictionary<int, int> BuyForMappings { private set; get; }
        
        Family family;
        Dictionary<int, int> dontBuyMappings = new Dictionary<int, int>();
        Random rand = new Random();

        public SecretSantaCalculator(SecretSantaWebContext db, Family family)
        {
            this.family = family;
            ReadDontBuyMappings(db);
        }

        public SecretSantaCalculator(Dictionary<int,int> dontBuyMappings)
        {
            this.dontBuyMappings = dontBuyMappings;
        }

        /// <summary>
        /// Calculates the set of BuyForMappings.
        /// Throws an exception if the operation is unsuccessful.
        /// </summary>
        /// <returns>True if operation successful, else false.</returns>
        public void Calculate()
        {
            CheckDontBuyMappings();
            for (int x = 0; x < 100; x++)
            {
                int p2 = 0;
                ClearAssignments();
                Dictionary<int, int> reverseMapping = new Dictionary<int, int>();
                for (int i = 0; i < BuyForMappings.Count*2; i++)
                {
                    int p1 = GetNextUnassignedPlayer();
                    if (p1 < 0) // job done
                    {
                        CheckBuyMappings();
                        return;
                    }
                    p2 = GetRandomUnboughtForPlayer(p1, dontBuyMappings[p1], reverseMapping);
                    if (p2 < 0) break; // No unbought for players... error, retry
                    BuyForMappings[p1] = p2;
                    reverseMapping[p2] = p1;
                }
                // If we've reached here, clear assignments and try again.
            }
            CheckBuyMappings();
        }

        void ReadDontBuyMappings(SecretSantaWebContext db)
        {
            foreach (Person person in family.FamilyMembers)
            {
                dontBuyMappings[person.PersonID] = person.NotBuyingForID == null ? 0 : person.NotBuyingForID.Value;
            }
        }

        void ClearAssignments()
        {
            BuyForMappings = new Dictionary<int, int>();
            foreach(int i in dontBuyMappings.Keys)
                BuyForMappings[i] = 0;
        }

        void CheckDontBuyMappings()
        {
            foreach(KeyValuePair<int,int> kvp in dontBuyMappings)
            {
                int i = kvp.Value;
                if (i == 0) continue;
                if (!dontBuyMappings.ContainsKey(i))
                    throw new ArgumentException("Invalid DontBuyMapping: [" + kvp.Key + "] = " + i);
            }
        }

        void CheckBuyMappings()
        {
            Dictionary<int, int> reverseMap = new Dictionary<int, int>();
            foreach (KeyValuePair<int, int> kvp in BuyForMappings)
            {
                int i = kvp.Value;
                if (i == 0)
                    throw new InvalidOperationException("Player "+kvp.Key+" unassigned");
                if (!BuyForMappings.ContainsKey(i))
                    throw new InvalidOperationException("Invalid BuyForMapping: [" + kvp.Key + "] = " + i);
                reverseMap[i] = kvp.Key;
            }
            if (BuyForMappings.Count != reverseMap.Count)
                throw new InvalidOperationException("Some player has missed out");
           
        }


        int GetNextUnassignedPlayer()
        {
            foreach(int i in BuyForMappings.Keys)
            {
                if (BuyForMappings[i] == 0)
                    return i;
            }
            return -1;
        }

        int GetRandomUnboughtForPlayer(int buyer, int exclusion, Dictionary<int,int> reverseMapping)
        {
            List<int> unbought = new List<int>();
            foreach(int receiver in BuyForMappings.Keys)
            {
                if (receiver == buyer || receiver == exclusion)
                    continue;
                if (reverseMapping.ContainsKey(receiver))
                    continue;
                unbought.Add(receiver);
            }

            if (unbought.Count == 0)
                return -1;
            int r = rand.Next(unbought.Count);
            return unbought[r];
        }


        /// <summary>
        /// Writes the current BuyForMappings state to the database provided
        /// </summary>
        /// <param name="db"></param>
        public void UpdateDatabase(SecretSantaWebContext db)
        {
            foreach (Person person in family.FamilyMembers)
            {
                var toUpdate = db.People.Find(person.PersonID);
                toUpdate.BuyingForID = BuyForMappings[person.PersonID];
                Controllers.PeopleController.CheckPersonModel(db, toUpdate);
                db.People.Attach(toUpdate);
                db.Entry(toUpdate).State = System.Data.Entity.EntityState.Modified;

            }
            db.SaveChanges();
        }
    }
}