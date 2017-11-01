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
    ///博客评论
    /// </summary>  
    [Table("Comment"), DisplayName("评论内容"), Serializable]
    public class Comment : EntityBase
    {
        /// <summary>
        /// 评论内容
        /// </summary>
        [DisplayName("评论内容"), Display(Description = "评论内容")]
        public string Content { get; set; }
        /// <summary>
        /// 评论ID
        /// </summary>
        [DisplayName("评论ID"), Display(Description = "父评论ID")]
        public int CommentID { get; set; }
        /// <summary>
        /// 回复人ID
        /// </summary>
        [DisplayName("回复人ID"), Display(Description = "回复人ID")]
        public int? ReplyUserID { get; set; }
        /// <summary>
        /// 是否父评论
        /// </summary>
        [DisplayName("是否父评论"), Display(Description = "是否父评论")]
        public bool? isInitial { get; set; }
        /// <summary>
        /// 回复人昵称
        /// </summary>
        [DisplayName("回复人昵称"), Display(Description = "回复人昵称")]
        public string ReplyUserNick { get; set; }
        /// <summary>
        /// 匿名评论的姓名
        /// </summary>
        [DisplayName("匿名评论的姓名"), Display(Description = "匿名评论的姓名")]
        public string AnonyName { get; set; }
        /// <summary>
        /// 评论人IP
        /// </summary>
        [DisplayName("评论人IP"),Display(Description ="评论人IP")]
        public string CommentIP { get; set; }

        public virtual ICollection<Article> Article { get; set; }

    }
}
