namespace ToDoApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notes", "Text", c => c.String(nullable: false));
            AlterColumn("dbo.ToDoLists", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ToDoLists", "Title", c => c.String());
            AlterColumn("dbo.Notes", "Text", c => c.String());
        }
    }
}
