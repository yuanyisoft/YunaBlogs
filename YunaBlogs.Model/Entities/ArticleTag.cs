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
    /// 文章标签
    /// </summary>
    [Table("ArticleTag"),DisplayName("文章标签"),Serializable]
    public class ArticleTag:EntityBase
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        [DisplayName("标签名称"),Display(Description ="标签名称")]
        public string TagName { get; set; }

        /// <summary>
        /// 标签备注
        /// </summary>
        [DisplayName("标签备注"),Display(Description ="标签备注")]
        public string TagrRemarks { get; set; }

        public virtual ICollection<Article> Article { get; set; }
    }
}
