using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.ModelDB.Entities
{
    /// <summary>
    /// 用户详细信息
    /// </summary>
    [Table("BlogUserInfo")]
    public class BlogUserInfo : BlogEntityBase
    {
        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 爱好
        /// </summary>
        public string Hobby { get; set; }
        /// <summary>
        /// 个人简介
        /// </summary>
        public string Profile { get; set; }
        /// <summary>
        /// 博客最后更新时间
        /// </summary>
        public Nullable<System.DateTime> BlogLastUpTime { get; set; }
        /// <summary>
        /// 今日更新博客数量
        /// </summary>
        public int BlogUpNum { get; set; }
        /// <summary>
        /// 是否禁用默认主题样式（PC）
        /// </summary>
        public Nullable<bool> IsDisCSS { get; set; }
        /// <summary>
        /// 是否在其他用户页面启用自定义主题样式（PC）
        /// </summary>
        public Nullable<bool> IsShowCSS { get; set; }
        /// <summary>
        /// (弃用)
        /// </summary>
        public Nullable<bool> IsShowHTML { get; set; }
        /// <summary>
        /// （弃用）
        /// </summary>
        public Nullable<bool> IsShowJS { get; set; }
        /// <summary>
        /// 是否在其他用户页面启用自定义主题样式（移动端）
        /// </summary>
        public Nullable<bool> IsShowMCSS { get; set; }
        /// <summary>
        /// 是否禁用默认主题样式（移动端）
        /// </summary>
        public Nullable<bool> IsDisMCSS { get; set; }
        /// <summary>
        /// 博客主题
        /// </summary>
        public string BlogTheme { get; set; }
        /// <summary>
        /// 
        /// </summary>  
        public string OAuthName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OAuthId { get; set; }

        /// <summary>
        /// 博客用户
        /// </summary>
        public virtual BlogUser BlogUser { get; set; }
    }
}
