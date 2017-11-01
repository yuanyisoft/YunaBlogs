namespace Blogs.ModelDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blogs : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.BlogInfo", "Content", c => c.String());
            //AddColumn("dbo.BlogInfo", "Remarks", c => c.String());
            //AddColumn("dbo.BlogInfo", "Title", c => c.String(nullable: false));
            //AddColumn("dbo.BlogInfo", "Url", c => c.String());
            //AddColumn("dbo.BlogInfo", "ReadNum", c => c.Int());
            //AddColumn("dbo.BlogInfo", "ForUrl", c => c.String());
            //AddColumn("dbo.BlogInfo", "CommentNum", c => c.Int());
            AlterColumn("dbo.BlogUser", "UserName", c => c.String(nullable: false));
            AlterColumn("dbo.BlogUser", "UserPass", c => c.String(nullable: false));
            AlterColumn("dbo.BlogUser", "UserMail", c => c.String(nullable: false));
            RenameColumn("dbo.BlogInfo", "BlogContent", "Content");
            RenameColumn("dbo.BlogInfo", "BlogRemarks", "Remarks");
            RenameColumn("dbo.BlogInfo", "BlogTitle", "Title");
            RenameColumn("dbo.BlogInfo", "BlogUrl", "Url");
            RenameColumn("dbo.BlogInfo", "BlogReadNum", "ReadNum");
            RenameColumn("dbo.BlogInfo", "BlogForUrl", "ForUrl");
            RenameColumn("dbo.BlogInfo", "BlogCommentNum", "CommentNum");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.BlogInfo", "BlogCommentNum", c => c.Int());
            //AddColumn("dbo.BlogInfo", "BlogForUrl", c => c.String());
            //AddColumn("dbo.BlogInfo", "BlogReadNum", c => c.Int());
            //AddColumn("dbo.BlogInfo", "BlogUrl", c => c.String());
            //AddColumn("dbo.BlogInfo", "BlogTitle", c => c.String());
            //AddColumn("dbo.BlogInfo", "BlogRemarks", c => c.String());
            //AddColumn("dbo.BlogInfo", "BlogContent", c => c.String());
            //AlterColumn("dbo.BlogUser", "UserMail", c => c.String());
            //AlterColumn("dbo.BlogUser", "UserPass", c => c.String());
            //AlterColumn("dbo.BlogUser", "UserName", c => c.String());
            //DropColumn("dbo.BlogInfo", "CommentNum");
            //DropColumn("dbo.BlogInfo", "ForUrl");
            //DropColumn("dbo.BlogInfo", "ReadNum");
            //DropColumn("dbo.BlogInfo", "Url");
            //DropColumn("dbo.BlogInfo", "Title");
            //DropColumn("dbo.BlogInfo", "Remarks");
            //DropColumn("dbo.BlogInfo", "Content");
        }
    }
}
