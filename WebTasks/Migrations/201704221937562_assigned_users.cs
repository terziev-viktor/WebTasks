namespace WebTasks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class assigned_users : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DailyTask_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Project_Id", c => c.Int());
            AddColumn("dbo.DailyTasks", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Projects", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "DailyTask_Id");
            CreateIndex("dbo.AspNetUsers", "Project_Id");
            CreateIndex("dbo.DailyTasks", "ApplicationUser_Id");
            CreateIndex("dbo.Projects", "ApplicationUser_Id");
            AddForeignKey("dbo.AspNetUsers", "DailyTask_Id", "dbo.DailyTasks", "Id");
            AddForeignKey("dbo.DailyTasks", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Project_Id", "dbo.Projects", "Id");
            AddForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.DailyTasks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "DailyTask_Id", "dbo.DailyTasks");
            DropIndex("dbo.Projects", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.DailyTasks", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Project_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "DailyTask_Id" });
            DropColumn("dbo.Projects", "ApplicationUser_Id");
            DropColumn("dbo.DailyTasks", "ApplicationUser_Id");
            DropColumn("dbo.AspNetUsers", "Project_Id");
            DropColumn("dbo.AspNetUsers", "DailyTask_Id");
        }
    }
}
