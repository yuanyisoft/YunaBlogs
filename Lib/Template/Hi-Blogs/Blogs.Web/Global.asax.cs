using Blogs.Common;
using Blogs.Controllers;
using Blogs.Helper;
using Blogs.ModelDB.Entities;
using CommonLib.HiLog;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;

namespace Blogs.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

#if DEBUG
            StackExchange.Profiling.EntityFramework6.MiniProfilerEF6.Initialize();
#endif

            MyBegin();
            PureViewEngines();
            LogSave.TrackLogSave("iis开始启动", "重启iis");

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //移除 webapi 的xml请求格式
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            ////打开注释 默认 是 移动端 显示效果
            //DisplayModeProvider.Instance.Modes.RemoveAt(0);
            //DisplayModeProvider.Instance.Modes.Insert(0,
            //    new DefaultDisplayMode("Mobile")
            //    {
            //        ContextCondition = Context => true
            //    }
            //    );

        }

        protected void Application_BeginRequest()
        {
#if DEBUG
            MiniProfiler.Start();
#endif
        }

        protected void Application_EndRequest()
        {
#if DEBUG
            MiniProfiler.Stop();
#endif
        }

        #region  保留razor视图引擎，其它的都去掉
        /// <summary>
        /// 保留razor视图引擎，其它的都去掉
        /// </summary>
        void PureViewEngines()
        {
            //移除 集合中 默认添加的 WebFormViewEngine
            ViewEngines.Engines.RemoveAt(0);

            RazorViewEngine razor = ViewEngines.Engines[0] as RazorViewEngine;

            //移除RazorViewEngine中的 vbhtml 路径模版。

            razor.FileExtensions = new string[] { "cshtml" };
            razor.AreaViewLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.cshtml" };
            razor.AreaMasterLocationFormats = new string[]{
                 "~/Areas/{2}/Views/{1}/{0}.cshtml",
                 "~/Areas/{2}/Views/Shared/{0}.cshtml"
            };

            razor.AreaPartialViewLocationFormats = new string[]{
                 "~/Areas/{2}/Views/{1}/{0}.cshtml",
                 "~/Areas/{2}/Views/Shared/{0}.cshtml"
            };

            razor.MasterLocationFormats = new string[]{
                 "~/Views/{1}/{0}.cshtml",
                 "~/Views/Shared/{0}.cshtml"
            };

            razor.PartialViewLocationFormats = new string[]{
                 "~/Views/{1}/{0}.cshtml",
                 "~/Views/Shared/{0}.cshtml"
            };

            razor.ViewLocationFormats = new string[]{
                 "~/Views/{1}/{0}.cshtml",
                 "~/Views/Shared/{0}.cshtml"
            };
        }
        #endregion

        #region 全局未处理异常捕获
        /// 全局未处理异常捕获
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            #region 异常注释
            Exception ex = Server.GetLastError();
            if (ex is HttpException && ((HttpException)ex).GetHttpCode() == 404)
                LogSave.SysErrLogSave(ex, "404");
            else
                LogSave.SysErrLogSave(ex);
            string strErr = LogSave.GetExceptionInfo(ex);
            //处理完及时清理异常 
            Server.ClearError();
            #endregion

            //Application_Error 里不能访问和操作 session为null 20150205            
            //HttpContext.Current.Session
            //Context.Session

            #region 跳转至出错页面
            //跳转至出错页面 
            //Server.Transfer("~/html/500.aspx");
            //注意：如果是ajax的请求 是不能 Response.Redirect 重定向的
            string sheader = Context.Request.Headers["X-Requested-With"];
            bool isAjax = (sheader != null && sheader == "XMLHttpRequest") ? true : false;
            if (isAjax)
            {
                if ((ex.HResult == -2146233087 && ex.Message.Contains("连接字符串不正确")))
                    Session["CreateDataBase"] = true;
                HttpContext.Current.Response.Write(new Blogs.Common.CustomModel.JSData()
                      {
                          State = Blogs.Common.CustomModel.EnumState.异常或Session超时,
                          JSurl = "/html/500.html?err=" + Microsoft.JScript.GlobalObject.escape(strErr)//System.Web.HttpUtility.UrlEncode(strErr)
                      }.ToJson());
            }
            else if (ex is HttpException && ((HttpException)ex).GetHttpCode() == 404)
                Response.Redirect("/Home/Path404");
            else if ((ex.HResult == -2146232060)//登录失败
                  || (ex.HResult == -2146233087 && ex.Message.Contains("连接字符串不正确"))) //连接字符串不正确
            {
                Session["CreateDataBase"] = true;
                Response.Redirect("/AdminHelper/CreateDataBase");
            }
            else
                Response.Redirect("/html/500.html?err=" + Microsoft.JScript.GlobalObject.escape(strErr));
            #endregion
        }
        #endregion

        #region 第一次访问 开始执行 的自定义方法
        private void MyBegin()
        {
            try
            {
                //设置日志存放路径                
                LogConfig.logFilePath = HttpContext.Current.Server.MapPath("~/") + @"\Log\";
                Blogs.Helper.FileHelper.defaultpath = HttpContext.Current.Server.MapPath("~/");

                BLL.BaseBLL<BlogUser> userbll = new BLL.BaseBLL<BlogUser>();
                var isbegin = userbll.GetList(t => true).Count() > 0;
                if (!isbegin)
                {
                    var user = new BlogUser()
                    {
                        UserName = " ",
                        UserPass = " ",
                        IsDelte = false,
                        IsLock = false,
                        UserMail = "无效",
                        CreationTime = DateTime.Now,
                        BlogUserInfo = new BlogUserInfo()
                    };
                    userbll.Insert(user);
                    userbll.save(false);
                }

                DemoController.GetData();
            }
            catch (Exception) { }

        }
        #endregion
    }
}