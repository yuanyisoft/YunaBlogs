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
    /// 博客用户
    /// </summary>
    [Table("BlogUser")]
    public partial class BlogUser : BlogEntityBase
    {       
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string UserPass { get; set; }
        /// <summary>
        /// 别名昵称
        /// </summary>
        public string UserNickname { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "邮箱不能为空")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "邮箱地址错误")]
        public string UserMail { get; set; }
        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLock { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserImage { get; set; }

        /// <summary>
        ///// 博客评论
        ///// </summary> 
        public virtual ICollection<BlogComment> BlogComments { get; set; }

        /// <summary>
        /// 博客
        /// </summary>
        public virtual ICollection<BlogInfo> BlogInfos { get; set; }

        /// <summary>
        /// 博客标签
        /// </summary>
        public virtual ICollection<BlogTag> BlogTags { get; set; }

        /// <summary>
        /// 博客类型
        /// </summary>
        public virtual ICollection<BlogType> BlogTypes { get; set; }

        /// <summary>
        /// 博客用户详细信息
        /// </summary>
        public virtual BlogUserInfo BlogUserInfo { get; set; }      
    }
}
