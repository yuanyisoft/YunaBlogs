namespace YunaBlogs.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Article", "ArticleTag_ID", "dbo.ArticleTag");
            DropIndex("dbo.Article", new[] { "ArticleTag_ID" });
            RenameColumn(table: "dbo.Article", name: "ArticleTag_ID", newName: "ArticleTagID");
            AlterColumn("dbo.Article", "ArticleTagID", c => c.Int(nullable: false));
            CreateIndex("dbo.Article", "ArticleTagID");
            AddForeignKey("dbo.Article", "ArticleTagID", "dbo.ArticleTag", "ID", cascadeDelete: true);
            DropColumn("dbo.Article", "TagID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Article", "TagID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Article", "ArticleTagID", "dbo.ArticleTag");
            DropIndex("dbo.Article", new[] { "ArticleTagID" });
            AlterColumn("dbo.Article", "ArticleTagID", c => c.Int());
            RenameColumn(table: "dbo.Article", name: "ArticleTagID", newName: "ArticleTag_ID");
            CreateIndex("dbo.Article", "ArticleTag_ID");
            AddForeignKey("dbo.Article", "ArticleTag_ID", "dbo.ArticleTag", "ID");
        }
    }
}
