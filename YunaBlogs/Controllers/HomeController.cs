using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YunaBlogs.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 博客首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Message = "1111111111111111111111111";
            return View();
        }
    

        /// <summary>
        /// 技术分享
        /// </summary>
        /// <returns></returns>
        public ActionResult Programtech() {
            return View();
        }

        /// <summary>
        /// 关于我们
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page";

            return View();
        }

        /// <summary>
        /// 内容（详情）
        /// </summary>
        /// <returns></returns>
        public ActionResult Content()
        {

            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// 友情链接
        /// </summary>
        /// <returns></returns>
        public ActionResult Friendly() {
            ViewBag.Message = "Your contact Friendly.";

            return View();
        }
    }
}