namespace YunaBlogs.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Article", "ArticleTagID", "dbo.ArticleTag");
            DropForeignKey("dbo.Article", "ArticleTypeID", "dbo.ArticleType");
            DropForeignKey("dbo.Article", "CommentID", "dbo.Comment");
            DropForeignKey("dbo.Article", "ReadInfoID", "dbo.ReadInfo");
            DropForeignKey("dbo.Article", "UserInfoID", "dbo.UserInfo");
            DropIndex("dbo.Article", new[] { "ArticleTypeID" });
            DropIndex("dbo.Article", new[] { "ArticleTagID" });
            DropIndex("dbo.Article", new[] { "CommentID" });
            DropIndex("dbo.Article", new[] { "ReadInfoID" });
            DropIndex("dbo.Article", new[] { "UserInfoID" });
            DropPrimaryKey("dbo.Article");
            DropPrimaryKey("dbo.ArticleTag");
            DropPrimaryKey("dbo.ArticleType");
            DropPrimaryKey("dbo.Comment");
            DropPrimaryKey("dbo.ReadInfo");
            DropPrimaryKey("dbo.UserInfo");
            AddColumn("dbo.Article", "ArticleTag_ID", c => c.Guid());
            AddColumn("dbo.Article", "ArticleType_ID", c => c.Guid());
            AddColumn("dbo.Article", "Comment_ID", c => c.Guid());
            AddColumn("dbo.Article", "ReadInfo_ID", c => c.Guid());
            AddColumn("dbo.Article", "UserInfo_ID", c => c.Guid());
            AlterColumn("dbo.Article", "ID", c => c.Guid(nullable: false));
            AlterColumn("dbo.ArticleTag", "ID", c => c.Guid(nullable: false));
            AlterColumn("dbo.ArticleType", "ID", c => c.Guid(nullable: false));
            AlterColumn("dbo.Comment", "ID", c => c.Guid(nullable: false));
            AlterColumn("dbo.ReadInfo", "ID", c => c.Guid(nullable: false));
            AlterColumn("dbo.UserInfo", "ID", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Article", "ID");
            AddPrimaryKey("dbo.ArticleTag", "ID");
            AddPrimaryKey("dbo.ArticleType", "ID");
            AddPrimaryKey("dbo.Comment", "ID");
            AddPrimaryKey("dbo.ReadInfo", "ID");
            AddPrimaryKey("dbo.UserInfo", "ID");
            CreateIndex("dbo.Article", "ArticleTag_ID");
            CreateIndex("dbo.Article", "ArticleType_ID");
            CreateIndex("dbo.Article", "Comment_ID");
            CreateIndex("dbo.Article", "ReadInfo_ID");
            CreateIndex("dbo.Article", "UserInfo_ID");
            AddForeignKey("dbo.Article", "ArticleTag_ID", "dbo.ArticleTag", "ID");
            AddForeignKey("dbo.Article", "ArticleType_ID", "dbo.ArticleType", "ID");
            AddForeignKey("dbo.Article", "Comment_ID", "dbo.Comment", "ID");
            AddForeignKey("dbo.Article", "ReadInfo_ID", "dbo.ReadInfo", "ID");
            AddForeignKey("dbo.Article", "UserInfo_ID", "dbo.UserInfo", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Article", "UserInfo_ID", "dbo.UserInfo");
            DropForeignKey("dbo.Article", "ReadInfo_ID", "dbo.ReadInfo");
            DropForeignKey("dbo.Article", "Comment_ID", "dbo.Comment");
            DropForeignKey("dbo.Article", "ArticleType_ID", "dbo.ArticleType");
            DropForeignKey("dbo.Article", "ArticleTag_ID", "dbo.ArticleTag");
            DropIndex("dbo.Article", new[] { "UserInfo_ID" });
            DropIndex("dbo.Article", new[] { "ReadInfo_ID" });
            DropIndex("dbo.Article", new[] { "Comment_ID" });
            DropIndex("dbo.Article", new[] { "ArticleType_ID" });
            DropIndex("dbo.Article", new[] { "ArticleTag_ID" });
            DropPrimaryKey("dbo.UserInfo");
            DropPrimaryKey("dbo.ReadInfo");
            DropPrimaryKey("dbo.Comment");
            DropPrimaryKey("dbo.ArticleType");
            DropPrimaryKey("dbo.ArticleTag");
            DropPrimaryKey("dbo.Article");
            AlterColumn("dbo.UserInfo", "ID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.ReadInfo", "ID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Comment", "ID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.ArticleType", "ID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.ArticleTag", "ID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Article", "ID", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Article", "UserInfo_ID");
            DropColumn("dbo.Article", "ReadInfo_ID");
            DropColumn("dbo.Article", "Comment_ID");
            DropColumn("dbo.Article", "ArticleType_ID");
            DropColumn("dbo.Article", "ArticleTag_ID");
            AddPrimaryKey("dbo.UserInfo", "ID");
            AddPrimaryKey("dbo.ReadInfo", "ID");
            AddPrimaryKey("dbo.Comment", "ID");
            AddPrimaryKey("dbo.ArticleType", "ID");
            AddPrimaryKey("dbo.ArticleTag", "ID");
            AddPrimaryKey("dbo.Article", "ID");
            CreateIndex("dbo.Article", "UserInfoID");
            CreateIndex("dbo.Article", "ReadInfoID");
            CreateIndex("dbo.Article", "CommentID");
            CreateIndex("dbo.Article", "ArticleTagID");
            CreateIndex("dbo.Article", "ArticleTypeID");
            AddForeignKey("dbo.Article", "UserInfoID", "dbo.UserInfo", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Article", "ReadInfoID", "dbo.ReadInfo", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Article", "CommentID", "dbo.Comment", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Article", "ArticleTypeID", "dbo.ArticleType", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Article", "ArticleTagID", "dbo.ArticleTag", "ID", cascadeDelete: true);
        }
    }
}
