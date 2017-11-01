using Blogs.Common.CustomModel;
using Blogs.ModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Blogs.BLL.Common;
using Blogs.Common.Helper;
using CommonLib.HiLog;
using System.IO;
using Blogs.ModelDB.Entities;

namespace Blogs.Controllers
{
    public class DemoController : Controller
    {
        const string TuHuaKey = "TuHua";
        const string LikeKey = "LikeKey";

        public ActionResult TuHua(int? id)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            var blog = new BlogUser() { };
            blog.BlogUserInfo = new BlogUserInfo() { };
            dic.Add(Constant.blogUser, blog);
            dic.Add(Constant.SessionUser, null);
            return View(dic);
        }

        public ActionResult saveSer(string key, string value)
        {
            var sessUser = BLL.Common.BLLSession.UserInfoSessioin;
            if (null == sessUser)
                return Json("err", JsonRequestBehavior.AllowGet);
            var userName = string.IsNullOrEmpty(sessUser.UserNickname) ? sessUser.UserName : sessUser.UserNickname;
            var dic = saveCache(key, userName + ":" + value);
            return Json(dic[key], JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存到内存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Dictionary<string, List<string>> saveCache(string key, string value)
        {
            if (value.Length > 1000 || key.Length > 1000)
                return null;
            var temp = CacheHelper.GetCache(TuHuaKey);
            var dic = new Dictionary<string, List<string>>();
            if (null != temp)
            {
                dic = temp as Dictionary<string, List<string>>;
                if (dic.Keys.Contains(key))
                    dic[key].Insert(0, value);
                else
                    dic.Add(key, new List<string>() { value });
            }
            else
                dic.Add(key, new List<string>() { value });
            LogSave.FileLogSave(value + "\r\n", "画话" + key);
            if (dic[key].Count() > 50)
                dic[key].RemoveRange(49, dic[key].Count() - 50);
            CacheHelper.SetCache(TuHuaKey, dic);
            return dic;
        }

        public ActionResult GetSer()
        {
            var temp = CacheHelper.GetCache(TuHuaKey);
            var dic = new Dictionary<string, List<string>>();
            if (null != temp)
            {
                dic = temp as Dictionary<string, List<string>>;
                return Json(dic, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public static void GetData()
        {
            DirectoryInfo dir = new DirectoryInfo(LogConfig.logFilePath);
            var dirs = dir.GetDirectories("画话*");
            var dirtemp = dirs.GroupBy(t => t.Name.Substring(0, t.Name.Length - 8));
            if (dirtemp != null && dirtemp.Count() > 0)
            {
                var dic = new Dictionary<string, List<string>>();
                foreach (var dirinfo in dirtemp)
                {
                    try
                    {
                        var fileInfo = dirinfo.OrderByDescending(t => t.CreationTime).FirstOrDefault().GetFiles().OrderByDescending(t => t.CreationTime).FirstOrDefault();
                        var arr = System.IO.File.ReadAllLines(fileInfo.FullName, Encoding.Default).ToList();
                        dic.Add(dirinfo.First().Name.Substring(2, dirinfo.First().Name.Length - (9 + 2)), arr);
                        var fanTempArr = arr.Where(t => t.Contains("范冰冰点赞"));
                        foreach (var item in fanTempArr)
                            LoadLikeData("范冰冰", item.Substring(3, item.IndexOf("为") - 3), string.Empty);
                        var gaoTempArr = arr.Where(t => t.Contains("高圆圆点赞"));
                        foreach (var item in gaoTempArr)
                            LoadLikeData("高圆圆", item.Substring(3, item.IndexOf("为") - 3), string.Empty);
                    }
                    catch (Exception ex)
                    {
                        LogSave.ErrLogSave("", ex);
                    }

                }
                CacheHelper.SetCache(TuHuaKey, dic);
            }
        }

        /// <summary>
        /// 初始加载点赞数据
        /// </summary>
        /// <param name="ImgName"></param>
        /// <param name="userKey"></param>
        /// <param name="value"></param>
        public static void LoadLikeData(string ImgName, string userKey, string value)
        {
            var cache = CacheHelper.GetCache(LikeKey);
            var list = new Dictionary<string, Dictionary<string, string>>();
            if (null != cache)
                list = cache as Dictionary<string, Dictionary<string, string>>;
            if (!list.Keys.Contains(ImgName))
                list.Add(ImgName, new Dictionary<string, string>() { });
            if (!list[ImgName].Keys.Contains(userKey))
                list[ImgName].Add(userKey, value);
            CacheHelper.SetCache(LikeKey, list);
        }

        /// <summary>
        /// 记录点赞数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="imgKey"></param>
        /// <returns></returns>
        public ActionResult SetLike(string name, string imgKey)
        {
            var cache = CacheHelper.GetCache(LikeKey);
            var list = new Dictionary<string, Dictionary<string, string>>();
            if (null != cache)
                list = cache as Dictionary<string, Dictionary<string, string>>;
            if (!list.Keys.Contains(name))
                list.Add(name, new Dictionary<string, string>() { });

            var ip = string.Empty;
            var userName = string.Empty;
            var isLike = false;
            var sessUser = BLL.Common.BLLSession.UserInfoSessioin;
            if (null != sessUser) //登录情况
            {
                userName = string.IsNullOrEmpty(sessUser.UserNickname) ? sessUser.UserName : sessUser.UserNickname;
                if (!list[name].Keys.Contains(userName))
                {
                    list[name].Add(userName, userName);
                    saveCache(imgKey, "消息：" + userName + "为" + name + "点赞");
                    LogSave.FileLogSave("消息：" + userName + "为" + name + "点赞");
                }
                else
                    isLike = true;
            }
            else//匿名情况
            {
                var md5 = GetUserDistinguish(Request, ref ip, true);
                if (!list[name].Keys.Contains(md5))
                {
                    list[name].Add(md5, ip);
                    for (int i = 0; i < ip.Length; i++)                    
                        if (i == 7 || i == 8)
                            userName += "*";
                        else
                            userName += ip[i]; 
                    //userName = ip.Length > 7 ? ip[5] .Substring(0, 6) + "..." : ip;
                    saveCache(imgKey, "消息：" + userName + "为" + name + "点赞");
                    LogSave.FileLogSave("消息：" + userName + "为" + name + "点赞");
                }
                else
                    isLike = true;
            }
            CacheHelper.SetCache(LikeKey, list);
            var obj = new { num = list[name].Count(), ip = ip, userName = userName, isLike = isLike };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLike(string name)
        {
            var cache = CacheHelper.GetCache(LikeKey);
            var list = new Dictionary<string, Dictionary<string, string>>();
            if (null != cache)
                list = cache as Dictionary<string, Dictionary<string, string>>;
            if (!list.Keys.Contains(name))
                list.Add(name, new Dictionary<string, string>() { });
            return Json(list[name].Count(), JsonRequestBehavior.AllowGet);
        }

        private string GetUserDistinguish(HttpRequestBase requestt, ref string strIp, bool IsMD5 = true)
        {
            //request
            StringBuilder str = new StringBuilder();
            string ip = "";
            if (requestt.ServerVariables.AllKeys.Contains("HTTP_X_FORWARDED_FOR") && requestt.ServerVariables.Get("HTTP_X_FORWARDED_FOR") != null)
                ip = requestt.ServerVariables.Get("HTTP_X_FORWARDED_FOR").ToString().Trim();
            else
                ip = requestt.ServerVariables.Get("Remote_Addr").ToString().Trim();
            str.Append("Ip:" + ip);
            str.Append("\r\n浏览器:" + requestt.Browser.Browser.ToString());
            str.Append("\r\n浏览器版本:" + requestt.Browser.MajorVersion.ToString());
            str.Append("\r\n操作系统:" + requestt.Browser.Platform.ToString());
            str.Append("\r\n页面：" + requestt.Url.ToString());
            //str.Append("客户端IP：" + requestt.UserHostAddress);
            str.Append("\r\n用户信息：" + User);
            str.Append("\r\n浏览器标识：" + requestt.Browser.Id);
            str.Append("\r\n浏览器版本号：" + requestt.Browser.Version);
            str.Append("\r\n浏览器是不是测试版本：" + requestt.Browser.Beta);
            //str.Append("<br/>浏览器的分辨率(像素)：" + Request["width"].ToString() + "*" + Request["height"].ToString());//1280/1024                        
            str.Append("\r\n是不是win16系统：" + requestt.Browser.Win16);
            str.Append("\r\n是不是win32系统：" + requestt.Browser.Win32);
            strIp = ip;
            if (IsMD5)
                return str.ToString().GetMd5_16();
            else
                return str.ToString();
        }
    }
}
