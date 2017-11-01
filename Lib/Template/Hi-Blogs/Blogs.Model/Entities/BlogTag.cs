using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.ModelDB.Entities
{
    /// <summary>
    /// 博客标签(个人博客内容标签)
    /// </summary>
    [Table("BlogTag")]
    public class BlogTag : BlogEntityBase
    { 
        /// <summary>
        /// tag标签名字
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TagRemarks { get; set; }            

        public virtual BlogUser BlogUser { get; set; }
       
        public virtual ICollection<BlogInfo> BlogInfos { get; set; }

        public int BlogUserId { get; set; }
    }
}
