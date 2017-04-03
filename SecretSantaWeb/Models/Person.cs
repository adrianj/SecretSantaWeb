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
        
        [Key]
        [ForeignKey("OwnerID")]
        public virtual List<Exclusion> Exclusions { get; set; }

        [Key]
        [ForeignKey("NotBuyingForID")]
        public virtual List<Exclusion> NotBuyingFor { get; set; }

        public int? BuyingFor { get; set; }


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
}