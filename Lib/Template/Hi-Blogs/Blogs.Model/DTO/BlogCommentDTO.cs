using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.ModelDB.DTO
{
    public class BlogCommentDTO
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public Nullable<System.DateTime> CreateTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public Nullable<System.DateTime> UpTime { get; set; }
        /// <summary>
        /// 是否不可用
        /// </summary>
        public bool IsDel { get; set; }
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
        /// <summary>
        /// 博客用户ID
        /// </summary>
        public int BlogUsersId { get; set; }
        /// <summary>
        /// 博客文章ID
        /// </summary>
        public int BlogsId { get; set; }
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
    }
}
