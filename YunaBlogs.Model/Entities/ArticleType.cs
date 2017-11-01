using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YunaBlogs.Model.Entities
{
    /// <summary>
    /// 文章类型
    /// </summary>
    [Table("ArticleType"),DisplayName("文章类型"), Serializable]
   public class ArticleType:EntityBase
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        [DisplayName("类型名称"), Display(Description = "类型名称")]
        public string TypeName { get; set; }
        /// <summary>
        /// 类型备注
        /// </summary>
        [DisplayName("类型备注"),Display(Description ="类型备注")]
        public string TypeRemarks { get; set; }

        public virtual ICollection<Article> Article { get; set; }
    }
}
