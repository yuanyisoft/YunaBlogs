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
    /// 用户信息（用做匿名评论）
    /// </summary>
    [Table("UserInfo"),DisplayName("用户信息"),Serializable]
    public class UserInfo : EntityBase
    {
        /// <summary>
        /// 别名昵称
        /// </summary>
        [DisplayName("别名昵称"),Display(Description= "别名昵称")]
        public string NickName{ get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "邮箱不能为空")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "邮箱地址错误")]
        [DisplayName("邮箱"),Display(Description ="验证用户输入邮箱地址是否合法")]
        public string Email { get; set; }

        /// <summary>
        /// 个人主页URL
        /// </summary>
        [DisplayName("个人主页url"),Display(Description ="验证用户输入的个人网址是否合法")]
        [RegularExpression(@"(http://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?",ErrorMessage ="个人URL地址错误")]
        public string PersonalUrl { get; set; }

        /// <summary>
        /// 用户图像
        /// </summary>
        public string UserPic { get; set; }

        public virtual ICollection<Article> Article { get; set; }

        

    }
}
