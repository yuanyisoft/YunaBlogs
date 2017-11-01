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
    /// 博客阅读统计（用来标记阅读统计）
    /// </summary>
    [Table("ReadInfo"),DisplayName("博客阅读统计"),Serializable]
    public class ReadInfo : EntityBase
    {
        /// <summary>
        /// 阅读客户端标识
        /// </summary>
        [DisplayName("阅读客户端标识"), Display(Description = "阅读客户端标识")]
        public string MD5 { get; set; }

        /// <summary>
        /// 阅读统计最后有效时间
        /// </summary>
        [DisplayName("阅读统计最后有效时间"),Display(Description = "阅读统计最后有效时间")]
        public DateTime LaseTime { get; set; }

        public virtual ICollection<Article> Article { get; set; }

    }
}
