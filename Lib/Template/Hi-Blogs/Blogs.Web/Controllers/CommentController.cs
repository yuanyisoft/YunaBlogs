using Blogs.BLL.Common;
using Blogs.Common;
using Blogs.Common.CustomModel;
using Blogs.Common.Helper;
using Blogs.Helper;
using Blogs.ModelDB;
using Blogs.ModelDB.Entities;
using CommonLib.HiLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogs.Controllers
{
    /// <summary>
    /// 评论操作和显示
    /// </summary>
    public class CommentController : Controller
    {

        #region 邮件配置
        /// <summary>
        /// 发件人密码
        /// </summary>
        private static string s_mailPwd = ConfigHelper.Custom.Mail.Pwd;  //"";
        /// <summary>
        /// SMTP邮件服务器
        /// </summary>
        private static string s_host = ConfigHelper.Custom.Mail.Host;
        /// <summary>
        /// 发件人邮箱
        /// </summary>
        private static string s_mailFrom = ConfigHelper.Custom.Mail.From;
        #endregion

        #region 01写入评论内容
        /// <summary>
        /// 写入评论内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public string WriteContent()
        {
            #region 评论前先检查是否已经登录
            var AnonymousName = string.Empty;//匿名登录
            if (Request.Form.AllKeys.Contains("AnonymousName") && !string.IsNullOrEmpty(Request.Form["AnonymousName"]))
            {
                AnonymousName = Request.Form["AnonymousName"];
            }
            else if (null == BLLSession.UserInfoSessioin)
            {
                return new JSData()
                {
                    Messg = "您还未登录~",
                    State = EnumState.异常或Session超时
                }.ToJson();
            }
            var sessionUser = BLLSession.UserInfoSessioin;

            //if (BLLSession.UserInfoSessioin.IsLock)
            //{
            //    return new JSData()
            //    {
            //        Messg = "您的账户已经被锁定,请联系管理员~",
            //        State = EnumState.失败
            //    }.ToJson();
            //}
            #endregion

            var BlogId = int.Parse(Request.Form["BlogId"]);
            var UserId = sessionUser.Id == 0 ? 1 : sessionUser.Id; //int.Parse(Request.Form["UserId"]);
            var CommentID = int.Parse(Request.Form["CommentID"]);
            var Content = Request.Form["Content"];
            var ReplyUserID = int.Parse(Request.Form["ReplyUser"]);

            if (Content.Length >= 1000)
            {
                return new JSData()
                {
                    State = EnumState.失败
                }.ToJson();
            }

            var ReplyUserName = string.Empty;
            var User = GetDataHelper.GetAllUser().Where(t => t.Id == ReplyUserID).FirstOrDefault();

            if (null != User)
            {
                ReplyUserName = string.IsNullOrEmpty(User.UserNickname) ? User.UserName : User.UserNickname;
            }

            BLL.BaseBLL<BlogComment> comment = new BLL.BaseBLL<BlogComment>();

            var user = new BLL.BaseBLL<BlogUser>().GetList(t => t.Id == UserId, isAsNoTracking: false).FirstOrDefault();
            var bloginfo = new BLL.BaseBLL<BlogInfo>().GetList(t => t.Id == BlogId, isAsNoTracking: false).FirstOrDefault();
            comment.Insert(new BlogComment()
            {
                BlogUser = user,
                BlogInfo = bloginfo,
                Content = Content,
                CommentID = CommentID,
                ReplyUserID = ReplyUserID,
                ReplyUserName = ReplyUserName,
                IsInitial = CommentID == -1,
                AnonymousName = AnonymousName
            });

            BLL.BaseBLL<BlogInfo> blogbll = new BLL.BaseBLL<BlogInfo>();
            var blogmode = blogbll.GetList(t => t.Id == BlogId, isAsNoTracking: false).FirstOrDefault();
            if (null == blogmode.CommentNum)
            {
                blogmode.CommentNum = comment.GetList(t => t.BlogInfo.Id == BlogId).Count() + 1;
            }
            else
            {
                blogmode.CommentNum++;
            }

            blogbll.Up(blogmode);
            blogbll.save();

            comment.save();

            #region 评论邮件通知

            var sessionName = string.IsNullOrEmpty(sessionUser.UserNickname) ? sessionUser.UserName : sessionUser.UserNickname;
            var tempUser = (User ?? blogmode.User);
            var nickName = string.IsNullOrEmpty(tempUser.UserNickname) ? tempUser.UserName : tempUser.UserNickname;
            var blogUrl = "http://" + Request.Url.Authority + "/" + blogmode.User.UserName + "/" + blogmode.Id + ".html";
            EmailHelper email = new EmailHelper()
            {
                mailPwd = s_mailPwd,
                host = s_host,
                mailFrom = s_mailFrom,
                mailSubject = "嗨-博客 消息提醒~",
                mailBody = EmailHelper.tempBody(nickName, sessionName + "回复您:<br/>" + Content, "<a href='" + blogUrl + "' target='_blank'>" + blogUrl + "</a>", isShow: false),
                mailToArray = new string[] { tempUser.UserMail }
            };
            try
            {
                email.Send(t =>
                {
                    LogSave.TrackLogSave("IP:" + RequestHelper.GetIp() + "\r\nToMail:" + User.UserMail + "\r\nBody:" + t.Body, "发送成功的邮件");
                },
                                t =>
                                {
                                    LogSave.TrackLogSave("IP:" + RequestHelper.GetIp() + "\r\nToMail:" + User.UserMail + "\r\nBody:" + t.Body, "发送失败的邮件");
                                }
                           );
            }
            catch (Exception)
            { }
            #endregion

            return new JSData()
            {
                //这里发表成功    就不提示了。
                State = EnumState.成功
            }.ToJson();
        }
        #endregion

        #region 02加载 分布试图 （评论部分）
        /// <summary>
        /// 加载 分布试图 （评论部分）
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadComment()
        {
            int blogId;
            //if (TempData["blogid"] != null)
            //    blogId = Convert.ToInt32(TempData["blogid"]);
            //else
            blogId = int.Parse(Request.Form["blogID"]);
            int pageIndex = int.Parse(Request.Form["pageIndex"]);
            int order = int.Parse(Request.Form["order"]);

            BLL.CommentHandle com = new BLL.CommentHandle();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            var comObj = com.GetComment(blogId, pageIndex, order);
            if (null == comObj)
                return PartialView("Null");
            dic.Add("commentList", comObj);//对应的评论
            dic.Add("SessionUser", BLL.Common.BLLSession.UserInfoSessioin);
            return PartialView(dic);
        }
        #endregion

        public ActionResult Message()
        {
            return View();
        }

    }
}
