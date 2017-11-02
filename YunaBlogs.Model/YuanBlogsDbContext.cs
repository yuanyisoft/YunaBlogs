using System.Data.Entity;
using YunaBlogs.Model.Entities;
using YunaBlogs.Model.Migrations;

namespace YunaBlogs.Model
{
    public class YuanBlogsDbContext : DbContext
    {
        public YuanBlogsDbContext()
            :base("YuanBlogsDB")
        {
            //每次重新启动总是初始化数据库到最新版本（连接字符串为“YuanBlogsDB”）        
           Database.SetInitializer(new MigrateDatabaseToLatestVersion<YuanBlogsDbContext, Configuration>("YuanBlogsDB"));
        }

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ReadInfo> ReadInfos { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleType> ArticleTypes { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
    }
}
