using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YunaBlogs.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        [HttpPost]
        public JsonResult Login(string name,string pwd)
        {
             name = Request.QueryString["name"].ToString();
             pwd = Request.QueryString["pwd"].ToString();
            if (name == "admin" && pwd == "123")
            {
                return Json(new { errCode = 0, msg = "登陆成功" });
            }
            else {
                return Json(new { errCode = 1, msg = "登录失败" });
            }
        }

        public ActionResult Main()
        {
            return View();
        }
    }
}