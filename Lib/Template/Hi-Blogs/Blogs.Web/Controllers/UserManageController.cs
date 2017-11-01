using Blogs.BLL.Application.UserManage;
using Blogs.BLL.Common;
using Blogs.Common.CustomModel;
using Blogs.Common.Helper;
using Blogs.Helper;
using Blogs.ModelDB;
using Blogs.ModelDB.Entities;
using CommonLib.HiLog;
using GeRenXing.OpenPlatform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogs.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserManageController : Controller
    {
        UserManageApplication _userManageApplication = null;
        public UserManageController()
        {
            _userManageApplication = new UserManageApplication();
        }

        //登录
        [HttpGet]
        public ActionResult Login(int? id) { return View(); }

        //登录
        [HttpPost]
        public JsonResult Login(BlogUser user, string ischeck)
        {
            var objJson = _userManageApplication.Login(user, ischeck);
            return Json(objJson);
        }

        #region 第三方登录
        [HttpPost]
        public JsonResult ThirdLogin(string loginType)
        {
            String authorizeUrl = _userManageApplication.ThirdLogin(loginType);
            return Json(authorizeUrl);
        }

        [HttpPost]
        public JsonResult ThirdLoginCallback(string code, string loginType = "qq")
        {
            return Json(_userManageApplication.ThirdLoginCallback(code, loginType));
        }

        public IOAuthClient GetOAuthClient(string loginType)
        {

            IOAuthClient oauthClient = _userManageApplication.GetOAuthClient(loginType);
            return oauthClient;
        }
        #endregion

        // 注册 
        [HttpGet]
        public ActionResult Regis(int? id) { return View(); }

        // 注册
        [HttpPost]
        public JsonResult Regis(BlogUser blog)
        {
            var json = _userManageApplication.Regis(blog);
            return Json(json);
        }

        //激活 (实际上是验证激活码后  修改用户信息：包括是否激活IsLock、邮箱地址、密码 修改值是根据 Session[tempUserinfo] 里的值 )
        [HttpGet]
        public ActionResult Activate(int? id)
        {
            if (null == Session[UserManageApplication.tempUserinfo] || (BLLSession.UserInfoSessioin != null && !BLLSession.UserInfoSessioin.IsLock))
                Redirect(Url.RouteUrl("Default", new { controller = "Home", action = "Index" }));
            return View();
        }

        //激活 (实际上是验证激活码后  修改用户信息：包括是否激活IsLock、邮箱地址、密码 )
        [HttpPost]
        public string Activate()
        {
            return _userManageApplication.Activate();
        }

        //获取激活码
        public bool GetActivate(ref JSData jsdata)
        {
            return _userManageApplication.GetActivate(ref jsdata);
        }
        //获取激活码   
        [HttpPost]
        public string GetActivate()
        {
            return _userManageApplication.GetActivate();
        }

        //重置密码
        public ActionResult ResetPass(int? id) { return View(); }

        //重置密码
        [HttpPost]
        public JsonResult ResetPass(BlogUser blog)
        {
            var jsData = _userManageApplication.ResetPass(blog);
            return Json(jsData);
        }

        //（无效邮箱）重新绑定邮箱
        [HttpGet]
        public ActionResult EmailValidation(int? id) { return View(); }

        // （无效邮箱）重新绑定邮箱  邮箱发送成功 默认跳转到激活页面
        [HttpPost]
        public JsonResult EmailValidation(string UserMail)
        {
            JSData jsdata = _userManageApplication.EmailValidation(UserMail);
            return Json(jsdata);
        }

        // 修改用户信息
        public ActionResult Modify() { return View(); }

        //用户注销
        public void Cancellation()
        {
            _userManageApplication.Cancellation();
        }
    }
}

