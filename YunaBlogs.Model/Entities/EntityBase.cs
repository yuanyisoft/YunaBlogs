using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunaBlogs.Model.Entities
{
    /// <summary>
    /// 模型基类
    /// </summary>
   public class EntityBase
    {
        /// <summary>
        /// 表主键ID
        /// </summary>
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CrteateTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModifiedTime { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        private bool _isDelte = false;
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete {

            get { return _isDelte; }
            set { _isDelte = value; }
        }

        private bool _isValid = true;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid {
            get { return _isValid; }
            set { _isValid = value; }
        }
    }
}
