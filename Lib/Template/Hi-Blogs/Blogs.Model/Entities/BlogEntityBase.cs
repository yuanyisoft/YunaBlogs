using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.ModelDB
{
    /// <summary>
    /// 模型父类
    /// </summary>
    public abstract class BlogEntityBase
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public Nullable<System.DateTime> CreationTime { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public Nullable<System.DateTime> LastModifiedTime { get; set; }

        private bool _isDelte = false;
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelte
        {
            get { return _isDelte; }
            set { _isDelte = value; }
        }

        private bool _isActive = true;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

    }
}
