using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.ModelDB.Entities
{
    /// <summary>
    /// 博客阅读统计（用来标记阅读统计）
    /// </summary>
    [Table("BlogReadInfo")]
    public class BlogReadInfo : BlogEntityBase
    { 
        /// <summary>
        /// 阅读客户端标识
        /// </summary>
        public string MD5 { get; set; }
        /// <summary>
        /// 阅读统计最后有效时间
        /// </summary>
        public System.DateTime LastTime { get; set; }
        /// <summary>
        /// 博客
        /// </summary>
        public virtual BlogInfo BlogInfo { get; set; }

        public int BlogInfoId { get; set; }
    }
}
