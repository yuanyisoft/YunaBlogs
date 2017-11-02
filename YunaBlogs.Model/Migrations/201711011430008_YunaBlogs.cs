namespace YunaBlogs.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class YunaBlogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Article",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Author = c.String(),
                        LoadTitle = c.String(),
                        loadurl = c.String(),
                        Decoration = c.String(),
                        Contents = c.String(),
                        ArticlePic = c.String(),
                        ArticleTypeID = c.Int(nullable: false),
                        ArticleTagID = c.Int(nullable: false),
                        CommentID = c.Int(nullable: false),
                        ReadInfoID = c.Int(nullable: false),
                        UserInfoID = c.Int(nullable: false),
                        CrteateTime = c.DateTime(),
                        LastModifiedTime = c.DateTime(),
                        Sort = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        IsValid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ArticleTag", t => t.ArticleTagID, cascadeDelete: true)
                .ForeignKey("dbo.ArticleType", t => t.ArticleTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Comment", t => t.CommentID, cascadeDelete: true)
                .ForeignKey("dbo.ReadInfo", t => t.ReadInfoID, cascadeDelete: true)
                .ForeignKey("dbo.UserInfo", t => t.UserInfoID, cascadeDelete: true)
                .Index(t => t.ArticleTypeID)
                .Index(t => t.ArticleTagID)
                .Index(t => t.CommentID)
                .Index(t => t.ReadInfoID)
                .Index(t => t.UserInfoID);
            
            CreateTable(
                "dbo.ArticleTag",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TagName = c.String(),
                        TagrRemarks = c.String(),
                        CrteateTime = c.DateTime(),
                        LastModifiedTime = c.DateTime(),
                        Sort = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        IsValid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ArticleType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        TypeRemarks = c.String(),
                        CrteateTime = c.DateTime(),
                        LastModifiedTime = c.DateTime(),
                        Sort = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        IsValid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        CommentID = c.Int(nullable: false),
                        ReplyUserID = c.Int(),
                        isInitial = c.Boolean(),
                        ReplyUserNick = c.String(),
                        AnonyName = c.String(),
                        CommentIP = c.String(),
                        CrteateTime = c.DateTime(),
                        LastModifiedTime = c.DateTime(),
                        Sort = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        IsValid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ReadInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MD5 = c.String(),
                        LaseTime = c.DateTime(nullable: false),
                        CrteateTime = c.DateTime(),
                        LastModifiedTime = c.DateTime(),
                        Sort = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        IsValid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NickName = c.String(),
                        Email = c.String(nullable: false),
                        PersonalUrl = c.String(),
                        UserPic = c.String(),
                        CrteateTime = c.DateTime(),
                        LastModifiedTime = c.DateTime(),
                        Sort = c.Int(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        IsValid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Article", "UserInfoID", "dbo.UserInfo");
            DropForeignKey("dbo.Article", "ReadInfoID", "dbo.ReadInfo");
            DropForeignKey("dbo.Article", "CommentID", "dbo.Comment");
            DropForeignKey("dbo.Article", "ArticleTypeID", "dbo.ArticleType");
            DropForeignKey("dbo.Article", "ArticleTagID", "dbo.ArticleTag");
            DropIndex("dbo.Article", new[] { "UserInfoID" });
            DropIndex("dbo.Article", new[] { "ReadInfoID" });
            DropIndex("dbo.Article", new[] { "CommentID" });
            DropIndex("dbo.Article", new[] { "ArticleTagID" });
            DropIndex("dbo.Article", new[] { "ArticleTypeID" });
            DropTable("dbo.UserInfo");
            DropTable("dbo.ReadInfo");
            DropTable("dbo.Comment");
            DropTable("dbo.ArticleType");
            DropTable("dbo.ArticleTag");
            DropTable("dbo.Article");
        }
    }
}
