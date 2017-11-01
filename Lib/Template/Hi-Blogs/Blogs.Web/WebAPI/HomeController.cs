using Blogs.BLL.Common;
using Blogs.BLL.WebApi;
using Blogs.Common.CustomModel;
using Blogs.Common.Helper;
using Blogs.ModelDB;
using Blogs.ModelDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace Blogs.Web.WebAPI
{

    /// <summary>
    /// 主页内容显示
    /// </summary>
    public class HomeController : ApiController
    {

        public static string LogonUser = "LogonUser";

        #region 仅做SwaggerUI注释
        [HttpGet]
        public void HomeAPI() { }
        #endregion

        #region (已弃用)
        /// <summary>
        /// (已弃用)
        /// </summary>
        /// <param name="idex">页码</param>
        /// <param name="sizePage">页容量</param>
        /// <param name="ContentLength">内容截取长度</param>
        /// <returns></returns>
        public object Get(int idex, int sizePage, int ContentLength)
        {
            int total;
            BLL.BaseBLL<BlogInfo> blog = new BLL.BaseBLL<BlogInfo>();
            var bloglist = blog.GetList(idex, sizePage, out total, t => t.IsShowHome == true, false, t => t.BlogCreateTime, false, tableName: t => t.User)//           
                .ToList()
                .Select(t => new BlogInfo()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Content = MyHtmlHelper.GetHtmlText(t.Content, ContentLength),
                    BlogCreateTime = t.BlogCreateTime,
                    User = new BlogUser()
                    {
                        UserName = t.User.UserName,
                        UserNickname = t.User.UserNickname
                    },
                    ReadNum = t.ReadNum,
                    CommentNum = t.CommentNum
                }).ToList();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("blog", bloglist);
            dic.Add("total", total);
            //dic.Add("users", CacheData.GetAllUserInfo().Where(t => t.IsLock == false).ToList());
            //dic.Add("SessionUser", BLL.Common.BLLSession.UserInfoSessioin);
            return dic;
        }
        #endregion

        #region 分页获取 博客 内容
        /// <summary>
        /// 分页获取 博客 内容
        /// </summary>
        /// <param name="index">页码</param>
        /// <param name="sizePage">页容量</param>
        /// <param name="contentLength">内容截取长度</param>
        /// <param name="userId">用户ID（如果取全部 就不需要传或传-1）</param>
        /// <returns>返回用户列表</returns>
        [CacheOutput(ClientTimeSpan = 120, ServerTimeSpan = 120)]//120 两分钟
        public object GetBlogContent(int index, int sizePage, int contentLength, int userId = -1)
        {
            int total;
            BLL.BaseBLL<BlogInfo> blog = new BLL.BaseBLL<BlogInfo>();
            IQueryable<BlogInfo> bloglistTemp = null;
            if (userId > 0)//个人用户首页
                bloglistTemp = blog.GetList(index, sizePage, out total, t => t.IsShowMyHome == true && t.User.Id == userId, false, t => t.BlogCreateTime, false, tableName: t => t.User);
            else//首页
                bloglistTemp = blog.GetList(index, sizePage, out total, t => t.IsShowHome == true, false, t => t.BlogCreateTime, false, tableName: t => t.User);//           
            var bloglist = bloglistTemp.ToList()
                 .Select(t => new BlogInfo()
                 {
                     Id = t.Id,//博客id
                     Title = t.Title,//博客标题
                     Content = MyHtmlHelper.GetHtmlText(t.Content, contentLength),//博客简介
                     BlogCreateTime = t.BlogCreateTime,//博客创建时间
                     User = new BlogUser()
                     {
                         UserName = t.User.UserName,//用户名
                         UserNickname = t.User.UserNickname//昵称
                     },
                     ReadNum = t.ReadNum,//博客阅读量
                     CommentNum = t.CommentNum//博客评论量
                 }).ToList();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("BlogBrief", bloglist);//博客简介
            dic.Add("Total", total);//总页数          
            return dic;
        }
        #endregion

        #region 获取博客  详细 信息
        /// <summary>
        /// 获取博客  详细 信息
        /// </summary>
        /// <param name="blogId">博客ID</param>
        /// <returns></returns>
        public object GetBlogInfo(int blogId)
        {
            BLL.BaseBLL<BlogInfo> blog = new BLL.BaseBLL<BlogInfo>();
            var blogobj = blog.GetList(t => t.Id == blogId).FirstOrDefault();
            if (blogobj == null)
                return new { mes = "您输入的博客ID不存在" };
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Blog", new BlogInfo()
            {
                IsDelte = blogobj.IsDelte,
                ReadNum = blogobj.ReadNum,
                Tags = blogobj.Tags,
                Title = blogobj.Title,
                Content = blogobj.Content
            });
            return dic;
        }
        #endregion

        #region 根据博客ID 取评论内容
        /// <summary>
        /// 根据博客ID 取评论内容
        /// </summary>
        /// <param name="blogId">博客id</param>
        /// <param name="index">页码</param>
        /// <param name="sizePage">页容量</param>
        /// <param name="order">升序：true 降序：false</param>
        /// <returns></returns>
        public object GetComment(int blogId, int index, int sizePage, bool order)
        {
            int total;
            BLL.BaseBLL<BlogComment> comment = new BLL.BaseBLL<BlogComment>();
            var commentList = comment.GetList(index, sizePage, out total, t => t.BlogInfo.Id == blogId, false, t => t.Id, order)
                .ToList().Select(t => new BlogComment() { });
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("CommentList", commentList);
            dic.Add("Total", total);//总页数
            return dic;
        }
        #endregion

        #region  登录 返回 token
        /// <summary>
        /// 登录 返回token 
        /// [token 用做需要登录用户才能操作的口令]
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPas">密码 (需要MD5 32位 大写 加密)</param>
        /// <returns></returns>
        public object GetLoginToken(string userName, string userPas)
        {
            UserOperation user = new UserOperation();
            return user.GetLoginToken(userName, userPas);
        }
        #endregion

        #region 写入评论
        /// <summary>
        /// 写入评论
        /// </summary>
        /// <param name="toKen">toKen</param>
        /// <param name="blogId">博客id</param>
        /// <param name="content">评论内容</param>
        /// <param name="tail">终端标识 如："来自安卓客户端"</param>
        /// <param name="commentID">评论楼层的ID（如果是新评论 可不传或者传-1）</param>
        /// <returns></returns>     
        [TokenValidate]//自定义特性 取登录用户
        public object PostComment(string toKen, int blogId, string content, string tail, int commentID = -1)
        {
            var loginUser = ControllerContext.RouteData.Values[LogonUser] as BlogUser;
            UserOperation user = new UserOperation();
            if (content.Length > 2000)
                return new { mes = "评论内容过多" };
            return user.PostComment(loginUser, blogId, content, tail, commentID);
        }

        #endregion
    }
}