using Blogs.ModelDB.Entities;
using Blogs.ModelDB.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.ModelDB
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext()
            : base("HiBlogsTemp")
        {
            //每次重新启动总是初始化数据库到最新版本（连接字符串为“HiBlogsTemp”）
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BlogDbContext, Configuration>("HiBlogsTemp"));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityBlogUser = modelBuilder.Entity<BlogUser>();

            entityBlogUser.HasMany(p => p.BlogInfos).WithRequired(t => t.User).HasForeignKey(t => t.BlogUserId).WillCascadeOnDelete(false);
            //与上面等效
            //modelBuilder.Entity<BlogInfo>().HasRequired(p => p.BlogUser).WithMany(t => t.BlogInfos)  

            //以BlogUser为主表（BlogUserInfo为从表，建立外键）
            entityBlogUser.HasRequired(p => p.BlogUserInfo).WithRequiredPrincipal(t => t.BlogUser)
            .Map(m => m.MapKey("BlogUserId")).WillCascadeOnDelete(false);
            //等效于HasRequired(p => ).WithOptional(i => );

            ////以BlogUserInfo为主表（BlogUser为从表，建立外键）
            //modelBuilder.Entity<BlogUser>().HasRequired(p => p.BlogUserInfo).WithRequiredDependent(t => t.BlogUser) 
            //.Map(m => m.MapKey("BlogUserId")).WillCascadeOnDelete(false);
            //等效于 HasOptional(p => ).WithRequired(i => ); 

            entityBlogUser.HasMany(p => p.BlogTags).WithRequired(t => t.BlogUser).HasForeignKey(t => t.BlogUserId).WillCascadeOnDelete(false);

            entityBlogUser.HasMany(p => p.BlogTypes).WithRequired(t => t.BlogUser).HasForeignKey(t => t.BlogUserId).WillCascadeOnDelete(false);

            entityBlogUser.HasMany(p => p.BlogComments).WithRequired(t => t.BlogUser).HasForeignKey(t => t.BlogUserId).WillCascadeOnDelete(false);

            var entityBlogInfo = modelBuilder.Entity<BlogInfo>();

            entityBlogInfo.HasMany(p => p.Tags).WithMany(t => t.BlogInfos)
           .Map(m => m.ToTable("BlogInfo_BlogTag"));

            entityBlogInfo.HasMany(p => p.Types).WithMany(t => t.BlogInfos)
            .Map(m => m.ToTable("BlogInfo_BlogType"));

            entityBlogInfo.HasMany(p => p.Comments).WithRequired(t => t.BlogInfo).HasForeignKey(t => t.BlogInfoId).WillCascadeOnDelete(false);

            entityBlogInfo.HasMany(p => p.ReadInfos).WithRequired(t => t.BlogInfo).HasForeignKey(t => t.BlogInfoId).WillCascadeOnDelete(false);

        }


        public DbSet<BlogInfo> BlogInfos { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<BlogReadInfo> BlogReadInfos { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<BlogType> BlogTypes { get; set; }
        public DbSet<BlogUser> BlogUsers { get; set; }
        public DbSet<BlogUserInfo> BlogUserInfos { get; set; }

    }
}
