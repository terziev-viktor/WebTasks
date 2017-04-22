namespace WebTasks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DailyTasks", "Creator_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Projects", "Creator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.DailyTasks", new[] { "Creator_Id" });
            DropIndex("dbo.Projects", new[] { "Creator_Id" });
            AlterColumn("dbo.DailyTasks", "Title", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.DailyTasks", "Description", c => c.String(maxLength: 250));
            AlterColumn("dbo.DailyTasks", "Creator_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Projects", "Title", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Projects", "Description", c => c.String(maxLength: 250));
            AlterColumn("dbo.Projects", "Creator_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.DailyTasks", "Creator_Id");
            CreateIndex("dbo.Projects", "Creator_Id");
            AddForeignKey("dbo.DailyTasks", "Creator_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Projects", "Creator_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "Creator_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DailyTasks", "Creator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "Creator_Id" });
            DropIndex("dbo.DailyTasks", new[] { "Creator_Id" });
            AlterColumn("dbo.Projects", "Creator_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Projects", "Description", c => c.String());
            AlterColumn("dbo.Projects", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.DailyTasks", "Creator_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.DailyTasks", "Description", c => c.String());
            AlterColumn("dbo.DailyTasks", "Title", c => c.String(nullable: false));
            CreateIndex("dbo.Projects", "Creator_Id");
            CreateIndex("dbo.DailyTasks", "Creator_Id");
            AddForeignKey("dbo.Projects", "Creator_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.DailyTasks", "Creator_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
