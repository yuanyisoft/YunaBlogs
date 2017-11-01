using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.ModelDB.Entities
{
    /// <summary>
    /// 博客评论
    /// </summary>
    [Table("BlogComment")]
    public class BlogComment : BlogEntityBase
    { 
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// (弃用)
        /// </summary>
        public string CommentSort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CommentID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ContentLevy { get; set; }
        ///// <summary>
        ///// 博客用户ID
        ///// </summary>
        //public int BlogUserId { get; set; }
        ///// <summary>
        ///// 博客文章ID
        ///// </summary>
        //public int BlogsId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<bool> IsInitial { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<int> ReplyUserID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReplyUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AnonymousName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CommentIP { get; set; }

        /// <summary>
        /// 博客
        /// </summary>
        public virtual BlogInfo BlogInfo { get; set; }

        /// <summary>
        /// 博客用户
        /// </summary>
        public virtual BlogUser BlogUser { get; set; }

        public int BlogUserId { get; set; }

        public int BlogInfoId { get; set; }
    }
}
