using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SecretSantaWeb.Models
{
    public class Person
    {
        public int PersonID { get; set; }

        public string Name { get; set; }

        public int FamilyID { get; set; }
        public virtual Family Family { get; set; }

        public int? NotBuyingForID { get; set; }
        public virtual Person NotBuyingFor { get; set; }

        public int? BuyingForID { get; set; }
        public virtual Person BuyingFor { get; set; }

    }
    
        /*
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasMany(p => p.Exclusions)
                .WithRequired(e => e.Owner)
                .HasForeignKey(e => e.OwnerID)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Person>()
                .HasMany(p => p.Exclusions)
                .WithRequired(e => e.NotBuyingFor)
                .HasForeignKey(e => e.NotBuyingForID)
                .WillCascadeOnDelete(true);
        }*/

    
}