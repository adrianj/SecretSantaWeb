using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace SecretSantaWeb.Models
{
    public class DatabaseSeeder
    {
        public static void Seed(SecretSantaWeb.Models.SecretSantaWebContext context)
        {
            context.Families.AddOrUpdate(
                f => f.FamilyName,
                new Family() { FamilyID = 1, FamilyName = "Jongenelen" },
                new Family() { FamilyID = 2, FamilyName = "Vanstone" }
                );

            context.People.AddOrUpdate(
                p => p.Name,
                new Person() { PersonID = 1, Name = "Adrian", FamilyID = 1}, 
                new Person() { PersonID = 2, Name = "Claire", FamilyID = 1 }, 
                new Person() { PersonID = 3, Name = "Arjen", FamilyID = 1 }, 
                new Person() { PersonID = 4, Name = "Jeff", FamilyID = 2 }
                );

            context.Exclusions.AddOrUpdate(
                e => e.OwnerID,
                new Exclusion() {  ExclusionID = 1, OwnerID = 1, NotBuyingForID = 1}
                );
        }
    }
}