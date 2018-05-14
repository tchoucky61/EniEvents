namespace EniEvents.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FourthMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Duration = c.Int(nullable: false),
                        Address = c.String(),
                        Description = c.String(),
                        Lat = c.Int(nullable: false),
                        Long = c.Int(nullable: false),
                        Thema_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Themas", t => t.Thema_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Thema_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uri = c.String(),
                        Event_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.Event_Id);
            
            CreateTable(
                "dbo.Themas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        Zipcode = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Events", "Thema_Id", "dbo.Themas");
            DropForeignKey("dbo.Pictures", "Event_Id", "dbo.Events");
            DropIndex("dbo.Pictures", new[] { "Event_Id" });
            DropIndex("dbo.Events", new[] { "User_Id" });
            DropIndex("dbo.Events", new[] { "Thema_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.Themas");
            DropTable("dbo.Pictures");
            DropTable("dbo.Events");
        }
    }
}
