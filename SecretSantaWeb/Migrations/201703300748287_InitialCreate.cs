namespace SecretSantaWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Families",
                c => new
                    {
                        FamilyID = c.Int(nullable: false, identity: true),
                        FamilyName = c.String(),
                    })
                .PrimaryKey(t => t.FamilyID);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PersonID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FamilyID = c.Int(nullable: false),
                        Person_PersonID = c.Int(),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("dbo.People", t => t.Person_PersonID)
                .ForeignKey("dbo.Families", t => t.FamilyID, cascadeDelete: true)
                .Index(t => t.FamilyID)
                .Index(t => t.Person_PersonID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.People", "FamilyID", "dbo.Families");
            DropForeignKey("dbo.People", "Person_PersonID", "dbo.People");
            DropIndex("dbo.People", new[] { "Person_PersonID" });
            DropIndex("dbo.People", new[] { "FamilyID" });
            DropTable("dbo.People");
            DropTable("dbo.Families");
        }
    }
}
