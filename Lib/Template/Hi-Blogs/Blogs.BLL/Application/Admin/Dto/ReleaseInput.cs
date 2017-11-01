using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.BLL.Application.Admin.Dto
{
    public class ReleaseInput
    {
        /// <summary>
        /// 正文内容
        /// </summary>
        [Required(ErrorMessage = "内容不能为空")]       
        public string Content { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [StringLength(100, ErrorMessage = "标题太长"), Required(ErrorMessage = "标题不能为空")]
        public string Title { get; set; }
        /// <summary>
        /// 旧的标签
        /// </summary>

        public string Oldtag { get; set; }
        
        /// <summary>
        /// 新的标签
        /// </summary>
        public string Newtag { get; set; }
        /// <summary>
        /// 文章类型
        /// </summary>    

        public string Chk_type { get; set; }
        /// <summary>
        /// 是否显示在主页
        /// </summary>

        public bool Isshowhome { get; set; }
        /// <summary>
        /// 是否显示在个人主页
        /// </summary>

        public bool Isshowmyhome { get; set; }
        /// <summary>
        /// 博客Id
        /// </summary>

        public int Blogid { get; set; }
    }
}
