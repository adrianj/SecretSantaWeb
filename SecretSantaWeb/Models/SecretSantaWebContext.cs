using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SecretSantaWeb.Models
{
    public class SecretSantaWebContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public SecretSantaWebContext() : base("name=SecretSantaWebContext")
        {
        }

        public System.Data.Entity.DbSet<SecretSantaWeb.Models.Family> Families { get; set; }

        public System.Data.Entity.DbSet<SecretSantaWeb.Models.Person> People { get; set; }

        public System.Data.Entity.DbSet<SecretSantaWeb.Models.Exclusion> Exclusions { get; set; }

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
        }
    }
}
