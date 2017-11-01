using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.ModelDB.Entities
{
    /// <summary>
    /// 博客信息
    /// </summary>
    [Table("BlogInfo")]
    public class BlogInfo : BlogEntityBase
    {
        /// <summary>
        /// 文章内容
        /// </summary>
        [MaxLength(400000)]
        public string Content { get; set; }
        /// <summary>
        /// 评论
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// 文章url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 是否是转发文章
        /// </summary>
        public Nullable<bool> IsForwarding { get; set; }
        /// <summary>
        /// 博客创建时间
        /// </summary>
        public Nullable<System.DateTime> BlogCreateTime { get; set; }
        /// <summary>
        /// 博客修改时间
        /// </summary>
        public Nullable<System.DateTime> BlogUpTime { get; set; }
        ///// <summary>
        ///// 用户ID（外键）
        ///// </summary>
        //public int UsersId { get; set; }
        /// <summary>
        /// 文章阅读量
        /// </summary>
        public Nullable<int> ReadNum { get; set; }
        /// <summary>
        /// 转发原链接
        /// </summary>
        public string ForUrl { get; set; }
        /// <summary>
        /// 是否显示在首页
        /// </summary>
        public Nullable<bool> IsShowHome { get; set; }
        /// <summary>
        /// 是否显示在个人主页
        /// </summary>
        public Nullable<bool> IsShowMyHome { get; set; }
        /// <summary>
        /// 此博客文章的评论数
        /// </summary>
        public Nullable<int> CommentNum { get; set; }

        /// <summary>
        /// 博客评论
        /// </summary>      
        public virtual ICollection<BlogComment> Comments { get; set; }

        /// <summary>
        /// 博客用户
        /// </summary>
        public virtual BlogUser User { get; set; }

        /// <summary>
        /// 博客标签
        /// </summary>       
        public virtual ICollection<BlogTag> Tags { get; set; }

        /// <summary>
        /// 博客标签
        /// </summary> 
        public virtual ICollection<BlogType> Types { get; set; }

        /// <summary>
        /// 博客阅读统计
        /// </summary>      
        public virtual ICollection<BlogReadInfo> ReadInfos { get; set; }

        public int BlogUserId { get; set; }
    }
}
