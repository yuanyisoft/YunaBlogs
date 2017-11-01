using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.ModelDB.DTO
{
    public class BlogUserDTO
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public Nullable<System.DateTime> CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public Nullable<System.DateTime> UpTime { get; set ;} 
        /// <summary>
        /// 是否不可用
        /// </summary>
        public bool IsDel { get ; set ; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string UserPass { get; set; }
        /// <summary>
        /// 别名昵称
        /// </summary>
        public string UserNickname { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string UserMail { get; set; }
        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLock { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserImage { get; set; }    
    }
}
