
using Blogs.Helper;
using Blogs.ModelDB;
using Blogs.ModelDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;

namespace Blogs.BLL.Common
{
    public class BLLSession
    {
        #region 01 户信息Session
        /// <summary>
        /// 用户信息Session
        /// </summary>
        public static BlogUser UserInfoSessioin
        {
            get
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["userinfo"] != null)
                    return HttpContext.Current.Session["userinfo"] as BlogUser;
                else
                {
                    HttpCookie Cookie = CookiesHelper.GetCookie("userInfo");
                    if (Cookie != null)
                    {
                        var lodname = Cookie.Values["userName"];
                        var lodpass = Cookie.Values["userPass"];
                        if (!string.IsNullOrEmpty(lodname) && !string.IsNullOrEmpty(lodpass))
                        {
                            //var objuser = Common.CacheData.GetAllUserInfo().Where(t => t.UserName == lodname.Trim() && t.UserPass == lodpass.Trim().MD5().MD5() && t.IsLock == false).FirstOrDefault();
                            var pass = lodpass.Trim().MD5().MD5();
                            var objuser = GetDataHelper.GetAllUser(t => t.BlogUserInfo, true).Where(t => (t.UserName == lodname || t.UserMail == lodname) && t.UserPass == pass).FirstOrDefault();

                            if (null != objuser)
                            {
                                HttpContext.Current.Session["userinfo"] = objuser;
                                return objuser;
                            }
                        }
                    }
                    return null;
                }
            }
            set
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session["userinfo"] = value;
            }
        }
        #endregion

        //#region 02 QQ户信息Session
        ///// <summary>
        ///// QQ用户信息Session
        ///// </summary>
        //public static ModelDB.BlogUser QQUserInfoSessioin
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session != null && HttpContext.Current.Session["QQUserInfoSessioin"] != null)
        //            return HttpContext.Current.Session["QQUserInfoSessioin"] as BlogUser;
        //        else
        //        {
        //            HttpCookie Cookie = CookiesHelper.GetCookie("QQUserInfoSessioin");
        //            if (Cookie != null)
        //            {
        //                var lodname = Cookie.Values["userName"];                        
        //                if (!string.IsNullOrEmpty(lodname))
        //                {
        //                    var objuser = new BlogUser()
        //                    {
        //                        IsLock = false,
        //                        UserName = lodname,
        //                        UserNickname = lodname
        //                    };
        //                    if (null != objuser)
        //                    {
        //                        HttpContext.Current.Session["QQUserInfoSessioin"] = objuser;
        //                        return objuser;
        //                    }
        //                }
        //            }
        //            return null;
        //        }
        //    }
        //    set
        //    {
        //        if (HttpContext.Current.Session != null)
        //        { 

        //            HttpContext.Current.Session["QQUserInfoSessioin"] = value;

        //            var user = HttpContext.Current.Session["QQUserInfoSessioin"] as BlogUser;
        //            HttpCookie Cookie = CookiesHelper.GetCookie("QQUserInfoSessioin");
        //            if (Cookie == null)
        //            {
        //                Cookie = new HttpCookie("QQUserInfoSessioin");
        //                Cookie.Values.Add("userName", user.UserName);

        //                //设置Cookie过期时间
        //                Cookie.Expires = DateTime.Now.AddDays(365);
        //                CookiesHelper.AddCookie(Cookie);
        //            }
        //            else
        //            {
        //                if (!Cookie.Values["userName"].Equals(user.UserName))
        //                    CookiesHelper.SetCookie("userInfo", "userName", user.UserName);                       
        //            }
        //        }
        //    }
        //}
        //#endregion
    }
}
