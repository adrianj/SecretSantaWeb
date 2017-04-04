using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecretSantaWeb.Models
{
    public class SecretSantaCalculator
    {
        SecretSantaWebContext db;
        Family family;
        Dictionary<int, int> BuyForMappings = new Dictionary<int, int>();
        Dictionary<int, int> DontBuyMappings = new Dictionary<int, int>();
        Random rand = new Random();

        public SecretSantaCalculator(SecretSantaWebContext db, Family family)
        {
            this.db = db;
            this.family = family;
        }
        public void Calculate()
        {
            ReadDontBuyMappings();
            ClearAssignments();
            for(int i = 0; i < 100; i++)
            {
                int p1 = GetNextUnassignedPlayer();
                if (p1 < 0) // job done
                    break;
                int p2 = GetRandomUnboughtForPlayer(p1, DontBuyMappings[p1]);
                BuyForMappings[p1] = p2;
            }
            UpdateDatabase();
        }

        void ReadDontBuyMappings()
        {
            foreach (Person person in family.FamilyMembers)
            {
                if (person.NotBuyingForID == null)
                    DontBuyMappings[person.PersonID] = 0;
                else
                    DontBuyMappings[person.PersonID] = (int)person.NotBuyingForID;
            }
        }

        void ClearAssignments()
        {
            foreach (Person person in family.FamilyMembers)
            {
                BuyForMappings[person.PersonID] = 0;
            }
        }

        void UpdateDatabase()
        {
            foreach(Person person in family.FamilyMembers)
            {
                var toUpdate = db.People.Find(person.PersonID);
                toUpdate.BuyingForID = BuyForMappings[person.PersonID];
                Controllers.PeopleController.CheckPersonModel(db, toUpdate);
                db.People.Attach(toUpdate);
                db.Entry(toUpdate).State = System.Data.Entity.EntityState.Modified;
              
            }
            db.SaveChanges();
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

        int GetRandomUnboughtForPlayer(int buyer, int exclusion)
        {
            List<int> unbought = new List<int>();
            foreach(KeyValuePair<int,int> kvp in BuyForMappings)
            {
                if (kvp.Value == buyer) continue;
                if (kvp.Value == exclusion && exclusion > 0) continue;
                if (kvp.Value == 0)
                {
                    unbought.Add(kvp.Value);
                }
            }
            if (unbought.Count == 0)
                return -1;
            int r = rand.Next(unbought.Count);
            return unbought[r];
        }

    }
}