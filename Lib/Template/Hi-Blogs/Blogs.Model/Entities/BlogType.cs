using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.ModelDB.Entities
{
    /// <summary>
    /// 博客类型（个人博客内容分类）
    /// </summary>
    [Table("BlogType")]
    public class BlogType : BlogEntityBase
    { 
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 类型备注
        /// </summary>
        public string TypeRemarks { get; set; }      
        /// <summary>
        /// 博客用户
        /// </summary>
        public virtual BlogUser BlogUser { get; set; }
        /// <summary>
        ///博客
        /// </summary>
        public virtual ICollection<BlogInfo> BlogInfos { get; set; }

        public int BlogUserId { get; set; }
    }
}
