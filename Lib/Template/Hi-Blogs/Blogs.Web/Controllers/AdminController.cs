using Blogs.BLL;
using Blogs.BLL.Application.Admin;
using Blogs.BLL.Application.Admin.Dto;
using Blogs.BLL.Common;
using Blogs.Common.CustomModel;
using Blogs.Helper;
using Blogs.ModelDB.DTO;
using Blogs.ModelDB.Entities;
using CommonLib.HiLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Blogs.Controllers
{
    public class AdminController : Controller
    {

        /// <summary>
        /// 管理员特权
        /// </summary>
        private readonly static string admin = "admin";
        AdminApplication adminApplication;
        public AdminController()
        {
            adminApplication = new AdminApplication();
        }

        #region 01发布 && 根据blogID编辑具体文章
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Release(int? id)
        {
            var userinfo = BLLSession.UserInfoSessioin;
            if (null == userinfo)
            {
                Response.Redirect("/UserManage/Login?href=/Admin/Release");
                return null;
            }
            ViewBag.userTagTypes = adminApplication.GetUserTagTypes();
            ViewBag.blogInfo = adminApplication.ReleaseGet(id); ;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public string Release(ReleaseInput input)
        {
            var jsdata = adminApplication.ReleasePost(input);
            return jsdata.ToJson();
        }

        #endregion

        #region 02文章编辑列表
        public ActionResult Edit(int? id)
        {
            var userinfo = BLLSession.UserInfoSessioin;
            List<BlogsDTO> blogs = new List<BlogsDTO>();
            if (null != id)
            {
                int ttt;
                BLL.BaseBLL<BlogInfo> blogbll = new BaseBLL<BlogInfo>();
                if (id == 0)//代表 未分类
                {
                    blogs = blogbll.GetList(1, 50, out ttt, t => t.Types.Count() == 0 && (t.User.Id == userinfo.Id), false, t => t.BlogCreateTime, false)
                      .ToList().Select(t => new BlogsDTO()
                      {
                          Id = t.Id,
                          BlogTitle = t.Title
                      }).ToList();
                }
                else
                {

                    blogs = blogbll.GetList(1, 50, out ttt, t => t.Types.Where(v => v.Id == id).Count() > 0 && (t.User.Id == userinfo.Id), false, t => t.BlogCreateTime, false)
                       .ToList().Select(t => new BlogsDTO()
                       {
                           Id = t.Id,
                           BlogTitle = t.Title
                       }).ToList();
                }
            }
            ViewBag.userTagTypes = adminApplication.GetUserTagTypes();
            ViewBag.blogInfos = blogs;
            return View();
        }
        #endregion

        #region 03删除  删除 文章
        /// <summary>
        /// 删除 文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Del(int? id)
        {
            var userinfo = BLLSession.UserInfoSessioin;
            List<BlogsDTO> blogs = new List<BlogsDTO>();
            int isdelok = -1;
            if (null != id)
            {
                BLL.BaseBLL<BlogInfo> blogbll = new BaseBLL<BlogInfo>();
                blogbll.Delete(new BlogInfo() { Id = (int)id }, true);
                isdelok = blogbll.save(false);
                List<SearchResult> list = new List<SearchResult>();
                list.Add(new SearchResult() { id = (int)id });
                SafetyWriteHelper<SearchResult>.logWrite(list, PanGuLuceneHelper.instance.Delete);
            }
            return Content((isdelok > 0).ToString());
        }
        #endregion

        #region 04新增文章类型
        /// <summary>
        /// 新增文章类型
        /// </summary>
        /// <param name="newtypename"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult NewAddType(string newtypename)
        {
            JSData jsdata = new JSData();

            #region 数据验证
            if (null == BLLSession.UserInfoSessioin)
                jsdata.Messg = "您还未登录~";
            else if (string.IsNullOrEmpty(newtypename))
                jsdata.Messg = "类型不能为空~";

            if (!string.IsNullOrEmpty(jsdata.Messg))
            {
                jsdata.State = EnumState.失败;
                return Json(jsdata);
            }
            #endregion

            int userid = BLLSession.UserInfoSessioin.Id;

            var user = new BLL.BaseBLL<BlogUser>().GetList(t => t.Id == userid, isAsNoTracking: false).FirstOrDefault();
            BLL.BaseBLL<BlogType> bll = new BaseBLL<BlogType>();
            bll.Insert(
                new BlogType()
                {
                    TypeName = newtypename,
                    BlogUser = user,
                    IsDelte = false
                }
                );

            if (bll.save() > 0)//保存
            {
                //BLL.Common.CacheData.GetAllType(true);//更新缓存
                jsdata.State = EnumState.成功;
                jsdata.Messg = "新增成功~";

            }
            else
            {
                jsdata.State = EnumState.失败;
                jsdata.Messg = "新增失败~";
            }
            return Json(jsdata);
        }
        #endregion

        #region 05编辑文章类型
        /// <summary>
        /// 编辑文章类型
        /// </summary>
        /// <param name="typename"></param>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public ActionResult EditType(string typename, int typeid)
        {
            JSData jsdata = new JSData();

            #region 数据验证
            if (null == BLLSession.UserInfoSessioin)
                jsdata.Messg = "您还未登录~";
            else if (string.IsNullOrEmpty(typename))
                jsdata.Messg = "类型不能为空~";
            else if (null == typeid)
                jsdata.Messg = "未取到文章ID~";
            if (!string.IsNullOrEmpty(jsdata.Messg))
            {
                jsdata.State = EnumState.失败;
                return Json(jsdata);
            }
            #endregion

            BLL.BaseBLL<BlogType> bll = new BaseBLL<BlogType>();
            var blogtype = new BlogType()
            {
                Id = typeid,
                TypeName = typename
            };
            bll.Updata(blogtype, "TypeName");

            if (bll.save() > 0)//保存
            {
              //  BLL.Common.CacheData.GetAllType(true);//更新缓存
                jsdata.State = EnumState.成功;
                // jsdata.Messg = "修改成功~";
            }
            else
            {
                jsdata.State = EnumState.失败;
                jsdata.Messg = "操作失败~";
            }
            return Json(jsdata);
        }
        #endregion

        #region 06自定义设置

        #region 设置 PC 端 主题样式
        /// <summary>
        /// 设置 PC 端 主题样式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ConfigurePC()
        {
            ViewBag.configure = adminApplication.ConfigurePC();
            ViewBag.userTagTypes = adminApplication.GetUserTagTypes();
            return View("Configure");
        }
        #endregion

        #region 设置 移动 端 主题样式
        /// <summary>
        /// 设置 移动 端 主题样式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ConfigureMobile()
        {
            ViewBag.configure = adminApplication.ConfigureMobile();
            ViewBag.userTagTypes = adminApplication.GetUserTagTypes();
            return View("Configure");
        }
        #endregion

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Configure(ConfigureInput input)
        {
            return Json(adminApplication.Configure(input));
            #region MyRegion
            //var IsShowCSS = Request.Form["IsShowCSS"] == "on";
            //var IsDisCSS = Request.Form["IsDisCSS"] == "on";
            //if (BLLSession.UserInfoSessioin == null)
            //    return Json("您还没有登录 不能修改~"); ;
            //try
            //{
            //    //==============================================================================================================
            //    //遗留问题：
            //    //如下：如果 userinfobll.Up(BLLSession.UserInfoSessioin.BlogUserInfo)两次的话 报异常：[一个实体对象不能由多个 IEntityChangeTracker 实例引用]
            //    //那么 我只能 new一个新的对象 修改  然后 同时 BLLSession.UserInfoSessioin.BlogUserInfo里面的属性，不然 其他地方访问的话 是没有修改过来的值
            //    //==============================================================================================================
            //    var userinftemp = new BlogUserInfo(); //BLLSession.UserInfoSessioin.BlogUserInfo;
            //    BLL.BaseBLL<BlogUserInfo> userinfobll = new BaseBLL<BlogUserInfo>();
            //    if (Request.Form["TerminalType"] == "PC")//如果是PC端
            //    {
            //        userinftemp.IsShowCSS =
            //            BLLSession.UserInfoSessioin.BlogUserInfo.IsShowCSS = IsShowCSS;
            //        userinftemp.IsDisCSS =
            //            BLLSession.UserInfoSessioin.BlogUserInfo.IsDisCSS = IsDisCSS;
            //        userinftemp.Id =
            //            BLLSession.UserInfoSessioin.BlogUserInfo.Id;
            //        userinfobll.Updata(userinftemp, "IsShowCSS", "IsDisCSS");//"IsShowHTML",, "IsShowJS"
            //    }
            //    else
            //    {
            //        userinftemp.IsShowMCSS =
            //          BLLSession.UserInfoSessioin.BlogUserInfo.IsShowMCSS = IsShowCSS;
            //        userinftemp.IsDisMCSS =
            //            BLLSession.UserInfoSessioin.BlogUserInfo.IsDisMCSS = IsDisCSS;
            //        userinftemp.Id =
            //            BLLSession.UserInfoSessioin.BlogUserInfo.Id;
            //        userinfobll.Updata(userinftemp, "IsShowMCSS", "IsDisMCSS");
            //    }

            //    CacheData.GetAllUserInfo().FirstOrDefault(t => t.Id == BLLSession.UserInfoSessioin.Id).BlogUserInfo
            //        = BLLSession.UserInfoSessioin.BlogUserInfo;

            //    userinfobll.save();

            //    string path = FileHelper.defaultpath + "/MyConfigure/" + BLLSession.UserInfoSessioin.UserName + "/";
            //    FileHelper.CreatePath(path);
            //    if (conf_css.Length >= 40000 ||
            //        conf_tail_html.Length >= 40000 ||
            //        conf_first_html.Length >= 40000 ||
            //        conf_side_html.Length >= 40000 ||
            //        conf_js.Length >= 40000)
            //    {
            //        return Json("您修改的内容字符过多~");
            //    }

            //    if (Request.Form["TerminalType"] == "PC")//如果是PC端
            //    {
            //        FileHelper.SaveFile(path, "conf.css", conf_css);
            //        FileHelper.SaveFile(path, "conf_side.txt", conf_side_html);
            //        FileHelper.SaveFile(path, "conf_first.txt", conf_first_html);
            //        FileHelper.SaveFile(path, "conf_tail.txt", conf_tail_html);
            //        FileHelper.SaveFile(path, "conf.js", conf_js);
            //    }
            //    else
            //    {
            //        FileHelper.SaveFile(path, "Mconf.css", conf_css);
            //        FileHelper.SaveFile(path, "Mconf_side.txt", conf_side_html);
            //        FileHelper.SaveFile(path, "Mconf_first.txt", conf_first_html);
            //        FileHelper.SaveFile(path, "Mconf_tail.txt", conf_tail_html);
            //        FileHelper.SaveFile(path, "Mconf.js", conf_js);
            //    }


            //    return Json("修改成功~");
            //}
            //catch (Exception ex)
            //{
            //    LogSave.ErrLogSave("自定义样式出错", ex);
            //    return Json("修改失败~"); ;
            //} 
            #endregion
        }
        #endregion
    }
}
