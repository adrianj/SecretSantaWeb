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
            
            var families = new List<Family>
            {
                new Family() { FamilyName = "Jongenelen" },
                new Family() { FamilyName = "Vanstone" },
                new Family() { FamilyName = "Smith" }
            };
            families.ForEach(f => context.Families.AddOrUpdate(p => p.FamilyName, f));
            context.SaveChanges();

            var jongenelen = context.Families.SingleOrDefault(f => f.FamilyName == "Jongenelen");
            var vanstone = context.Families.SingleOrDefault(f => f.FamilyName == "Vanstone");

            var people = new List<Person>
            {
                new Person() {  Name = "Adrian", FamilyID = jongenelen.FamilyID },
                new Person() {  Name = "Claire",FamilyID = jongenelen.FamilyID },
                new Person() {  Name = "Arjen",FamilyID = jongenelen.FamilyID},
                new Person() {  Name = "Jeff",FamilyID = vanstone.FamilyID},
                new Person() {  Name = "Arjen",FamilyID = vanstone.FamilyID},
            };

            people.ForEach(per => context.People.AddOrUpdate(p => p.Name, per));
            context.SaveChanges();

            AddBuyingFor(context, "Adrian", "Jongenelen", "Claire", "Jongenelen");
            AddNotBuyingFor(context, "Adrian", "Jongenelen", "Arjen", "Jongenelen");

            AddBuyingFor(context, "Claire", "Jongenelen", "Arjen", "Jongenelen");
            AddNotBuyingFor(context, "Claire", "Jongenelen", "Adrian", "Jongenelen");

            AddBuyingFor(context, "Arjen", "Vanstone", "Jeff", "Vanstone");

            context.SaveChanges();

        }

        static void AddBuyingFor(SecretSantaWebContext context, string buyerFirstName, string buyerFamilyName, string receiverFirstName, string receiverFamilyName)
        {
            var buyer = context.People.SingleOrDefault(p => p.Name == buyerFirstName && p.Family.FamilyName == buyerFamilyName);
            var receiver = context.People.SingleOrDefault(p => p.Name == receiverFirstName && p.Family.FamilyName == receiverFamilyName);
            buyer.BuyingFor = receiver;
            buyer.BuyingForID = receiver.PersonID;
        }

        static void AddNotBuyingFor(SecretSantaWebContext context, string buyerFirstName, string buyerFamilyName, string receiverFirstName, string receiverFamilyName)
        {
            var buyer = context.People.SingleOrDefault(p => p.Name == buyerFirstName && p.Family.FamilyName == buyerFamilyName);
            var receiver = context.People.SingleOrDefault(p => p.Name == receiverFirstName && p.Family.FamilyName == receiverFamilyName);
            buyer.NotBuyingFor = receiver;
            buyer.NotBuyingForID = receiver.PersonID;
        }

    }
}