using Blogs.BLL.Common;
using Blogs.ModelDB;
using Ivony.Html.Parser;
using Ivony.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Blogs.BLL;
using Blogs.ModelDB.Entities;



namespace Blogs.Web.Controllers
{
    /// <summary>
    /// 转发插件
    /// </summary>
    public class PlugInUnitController : Controller
    {

        //static string siteUrl = string.Empty;// "blog.haojima.net";

        /// <summary>
        /// 站内搜索地址
        /// </summary>
        //public string GetSiteUrl()
        //{
        //    if (string.IsNullOrEmpty(siteUrl))
        //        siteUrl = Request.Url.Host;
        //    return siteUrl;
        //}       

        /// <summary>
        /// 登录
        /// </summary>
        public void Login()
        {
            var data = Request.QueryString["mydata"];
            string callback = Request.QueryString["callback"];
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Dictionary<string, string> dic = jss.Deserialize<Dictionary<string, string>>(data);
            var name = dic["username"].Trim();
            var pass = dic["password"].Trim();
            var userinfo = GetDataHelper.GetAllUser().Where(t => t.UserName == name && t.UserPass == pass.MD5().MD5()).FirstOrDefault();
            object tyeList = null;
            if (userinfo != null)
                tyeList = GetDataHelper.GetAllType().Where(t => t.BlogUser.Id == userinfo.Id).Select(t => new
                {
                    TypeName = t.TypeName,
                    Id = t.Id
                }).ToList();
            var cc = callback + "('" + tyeList.ToJson() + "')";
            Response.ContentType = "application/json";
            Response.Write(cc);
        }

        /// <summary>
        /// 转发
        /// </summary>
        public void Forward()
        {
            Response.ContentType = "application/json";
            //GetSiteUrl();
            var ResultValue = string.Empty;
            var data = Request.QueryString["mydata"];
            string callback = Request.QueryString["callback"];
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Dictionary<string, string> dic = jss.Deserialize<Dictionary<string, string>>(data);
            var name = dic["username"].Trim();
            var pass = dic["password"].Trim();
            var userinfo = GetDataHelper.GetAllUser().Where(t => t.UserName == name && t.UserPass == pass.MD5().MD5()).FirstOrDefault();
            //object tyeList = null;
            var tag = dic["tag"].Trim();
            var type = dic["type"].Trim();
            var url = dic["url"].Trim();
            var json = ForwardRealization(userinfo, tag, type, url, Request.Url.Host, false, true);
            var call = callback + "('" + json + "')";
            Response.Write(call);
        }

        public static string ForwardRealization(BlogUser userinfo, string tag, string type, string url, string siteUrl, bool isshowhome, bool isshowmyhome, bool isBJ = false)
        {
            if (userinfo != null)
            {
                int typeint = -1;
                int.TryParse(type, out typeint);
                var tags = tag.Split(',');

                var jp = new JumonyParser();
                var html = jp.LoadDocument(url);
                var titlehtml = html.Find(".postTitle a").FirstOrDefault().InnerHtml();
                if (!isBJ)
                    titlehtml = "【转】" + titlehtml;
                else
                    titlehtml = "《" + titlehtml + "》";
                var bodyhtml = html.Find("#cnblogs_post_body").FirstOrDefault().InnerHtml();
                bodyhtml += "</br><div class='div_zf'>==================================<a  href='" + url + "' target='_blank'>原文链接</a>==================================</div>";

                var mtag = BLL.Common.GetDataHelper.GetAllTag().Where(t => tags.Contains(t.TagName)).ToList();

                var blogtagid = new List<int>();
                for (int i = 0; i < tags.Length; i++)
                {
                    blogtagid.Add(GetTagId(tags[i], userinfo.Id));
                }
                //&& t.UsersId == userinfo.Id         理论是不用 加用户id 筛选
                var myBlogTags = new BLL.BaseBLL<BlogTag>().GetList(t => blogtagid.Contains(t.Id), isAsNoTracking: false).ToList();
                var myBlogTypes = new BLL.BaseBLL<BlogType>().GetList(t => t.Id == typeint, isAsNoTracking: false).ToList();

                object obj = null;
                string call = string.Empty;
                BLL.BaseBLL<BlogInfo> blogbll = new BaseBLL<BlogInfo>();

                var blogtemp = blogbll.GetList(t => t.User.Id == userinfo.Id).OrderByDescending(t => t.Id).FirstOrDefault();
                if (blogtemp != null && blogtemp.Title == titlehtml)
                {
                    obj = new { s = "no", m = "已存在相同标题博客文章~", u = siteUrl };
                    call = obj.ToJson();
                    //Response.Write(call);
                    return call;
                }

                var blogmode = new BlogInfo()
                {
                    User = userinfo,
                    Title = titlehtml,
                    Types = myBlogTypes,
                    Tags = myBlogTags,
                    Content = bodyhtml,
                    CreationTime = DateTime.Now,
                    BlogCreateTime = DateTime.Now,
                    BlogUpTime = DateTime.Now,
                    IsShowMyHome = isshowmyhome,
                    IsShowHome = isshowhome
                };

                blogbll.Insert(blogmode);

                if (blogbll.save() > 0)
                {
                    obj = new { s = "ok", m = "发布成功", u = siteUrl + "/" + userinfo.UserName + "/" + blogmode.Id + ".html" };
                    call = obj.ToJson();
                    //Response.Write(call);
                    return call;
                }
                obj = new { s = "no", m = "发布失败", u = siteUrl + "/" + userinfo.UserName + "/" + blogmode.Id + ".html" };
                call = obj.ToJson();
                //Response.Write(call);
                return call;
            }
            else
            {
                var obj = new { s = "no", m = "发布失败", u = siteUrl + "/" };
                var call = obj.ToJson();
                //Response.Write(call);
                return call;
            }
        }

        #region GetTagId(string tagname, string userName)
        private static int GetTagId(string tagname, int userid)
        {
            BLL.BaseBLL<BlogTag> blogtag = new BaseBLL<BlogTag>();
            try
            {
                var blogtagmode = blogtag.GetList(t => t.TagName == tagname);
                if (blogtagmode.Count() >= 1)
                    return blogtagmode.FirstOrDefault().Id;
                else
                {
                    var user = new BLL.BaseBLL<BlogUser>().GetList(t => t.Id == userid, isAsNoTracking: false).FirstOrDefault();
                    blogtag.Insert(new BlogTag()
                    {
                        TagName = tagname,
                        IsDelte = false,
                        BlogUser = user
                    });
                    blogtag.save();
                    return GetTagId(tagname, userid);
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        #endregion

    }
}
