namespace ToDoApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Text = c.String(),
                        Position = c.Int(nullable: false),
                        ToDoListId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ToDoLists", t => t.ToDoListId, cascadeDelete: true)
                .Index(t => t.ToDoListId);
            
            CreateTable(
                "dbo.ToDoLists",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Position = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "ToDoListId", "dbo.ToDoLists");
            DropIndex("dbo.Notes", new[] { "ToDoListId" });
            DropTable("dbo.ToDoLists");
            DropTable("dbo.Notes");
        }
    }
}
