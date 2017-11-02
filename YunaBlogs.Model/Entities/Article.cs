using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunaBlogs.Model.Entities  
{
    /// <summary>
    /// 文章信息
    /// </summary>
    [Table("Article"), DisplayName("文章信息"), Serializable]
    public class Article : EntityBase
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        [DisplayName("文章标题"), Display(Description = "文章标题")]
        public string Title { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [DisplayName("作者"), Display(Description = "作者")]
        public string Author { get; set; }

        /// <summary>
        /// 来源标题
        /// </summary>
        [DisplayName("来源标题"), Display(Description = "来源标题")]
        public string LoadTitle { get; set; }

        /// <summary>
        /// 来源地址
        /// </summary>
        [DisplayName("来源地址"), Display(Description = "来源地址")]
        public string loadurl { get; set; }

        /// <summary>
        /// 文章描述
        /// </summary>        
        [DisplayName("文章描述"), Display(Description = "文章描述")]
        public string Decoration { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        [DisplayName("文章内容"), Display(Description = "文章内容")]
        public string Contents { get; set; }

        /// <summary>
        /// 文章图片
        /// </summary>
        [DisplayName("文章图片"), Display(Description = "文章图片")]
        public string ArticlePic { get; set; }

        /// <summary>
        /// 文章类型 Foreign key
        /// </summary>
        public int ArticleTypeID { get; set; }
        // Navigationproperties
        public virtual ArticleType ArticleType { get; set; }

        /// <summary>
        /// 文章标签Foreign key
        /// </summary>
        public int ArticleTagID { get; set; }
        public virtual ArticleTag ArticleTag { get; set; }

        /// <summary>
        /// 评论表Foreign key
        /// </summary>
        public int CommentID { get; set; }
        public virtual Comment Comment { get; set; }

        /// <summary>
        /// 博客阅读统计 Foreign key
        /// </summary>
        public int ReadInfoID { get; set; }
        public virtual ReadInfo ReadInfo { get; set; }

        /// <summary>
        /// 用户信息Foreign key
        /// </summary>
        public int UserInfoID { get; set; }
        public virtual UserInfo UserInfo { get; set; }


    }
}
