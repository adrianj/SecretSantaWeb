using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
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
            Database.SetInitializer<SecretSantaWebContext>(new SecretSantaWebDBInitialiser());
            
        }

        

        public System.Data.Entity.DbSet<SecretSantaWeb.Models.Family> Families { get; set; }

        public System.Data.Entity.DbSet<SecretSantaWeb.Models.Person> People { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Family>()
                .HasMany(f => f.FamilyMembers)
                .WithRequired(p => p.Family)
                .HasForeignKey(p => p.FamilyID)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Person>()
                .HasOptional(p => p.BuyingFor)
                .WithOptionalDependent();

            modelBuilder.Entity<Person>()
                .HasOptional(p => p.NotBuyingFor)
                .WithOptionalDependent();
            /*
            
            modelBuilder.Entity<Person>()
                .HasMany(p => p.NotBuyingFor)
                .WithMany(p => p.NotBoughtBy)
                .Map(p => p.ToTable("NotBuyingFor"))
                ;
            
            modelBuilder.Entity<Person>()
                .HasMany(p => p.Exclusions)
                .WithRequired(e => e.NotBuyingFor)
                .HasForeignKey(e => e.NotBuyingForID)
                .WillCascadeOnDelete(true);
                
            modelBuilder.Entity<Exclusion>()
                .HasRequired(e => e.Owner)
                .WithMany(p => p.Exclusions)
                .HasForeignKey(e => e.OwnerID)
                .WillCascadeOnDelete(true);
            
            modelBuilder.Entity<Exclusion>()
                .HasOptional(e => e.NotBuyingFor)
                .WithMany(p => p.NotBuyingFor)
                .HasForeignKey(e => e.NotBuyingForID)
                .WillCascadeOnDelete(false);*/
        }
        
    }
    public class SecretSantaWebDBInitialiser : DropCreateDatabaseAlways<SecretSantaWebContext>
    {
        protected override void Seed(SecretSantaWebContext context)
        {
            DatabaseSeeder.Seed(context);
            base.Seed(context);
        }
    }
}
