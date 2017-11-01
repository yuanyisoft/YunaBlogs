using Blogs.BLL.Common;
using Blogs.Common.CustomModel;
using Blogs.ModelDB;
using Blogs.ModelDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.BLL.WebApi
{
    public class UserOperation
    {
        #region 变量
        /// <summary>
        /// 失效时间 (小时)
        /// </summary>
        static int time = 3;
        /// <summary>
        /// token对应的登录用户
        /// </summary>
        public static Dictionary<string, ToKenModel> ToKens = new Dictionary<string, ToKenModel>();
        #endregion

        #region 根据token获取登录用户
        /// <summary>     
        /// 根据token获取登录用户 
        /// 如果不存在 或过期 返回null
        /// [主要用在特性验证]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static BlogUser GetLoginUser(string key)
        {
            if (!ToKens.Keys.Contains(key))
                return null;
            if (ToKens[key].Time.AddHours(time) < DateTime.Now)
            {
                ToKens.Remove(key);
                return null;
            }
            ToKens[key].Time = DateTime.Now;
            return  GetDataHelper.GetAllUser()
                .Where(t => t.UserName == ToKens[key].Name || t.UserMail == ToKens[key].Name).FirstOrDefault();
        }
        #endregion

        #region 移除过期 token
        /// <summary>
        /// 移除过期 token (time个小时 未操作 需要登录的 方法 则为过期)
        /// </summary>
        private void RemoveToKen()
        {
            foreach (var toKen in ToKens)
                if (toKen.Value.Time.AddHours(time) < DateTime.Now)
                    ToKens.Remove(toKen.Key);
        }
        #endregion

        #region 写入评论
        /// <summary>
        /// 写入评论
        /// </summary>
        /// <param name="toKen"></param>
        /// <param name="blogId"></param>
        /// <param name="content"></param>
        /// <param name="commentID"></param>
        /// <returns></returns>        
        public object PostComment(BlogUser loginUser, int blogId, string content, string tail, int commentID = -1)
        {
            var user = loginUser;
            if (user == null)
                return new { state = 0, msg = "toKen无效,评论失败" };
            var commenttemp = GetCommentInfo(commentID, blogId);
            if (null == commenttemp)
                return new { state = 0, msg = "传入的评论楼层ID错误" };
            BLL.BaseBLL<BlogComment> comment = new BaseBLL<BlogComment>();
            var ReplyUserID = commentID == -1 ? -1 : commenttemp.BlogUser.Id;
            var ReplyUserName = commentID == -1 ? string.Empty : GetUserName(commenttemp.BlogUser.Id);
            BlogComment commentobj = new BlogComment()
            {
                BlogInfo = new BlogInfo()
                {
                    Id = blogId
                },
                BlogUser = user,
                Content = content + "\r\n" + tail,
                CreationTime = DateTime.Now,
                LastModifiedTime = DateTime.Now,
                IsDelte = false,
                ReplyUserID = ReplyUserID,
                ReplyUserName = ReplyUserName,
                CommentID = commentID,
                IsInitial = commentID == -1
            };
            comment.Insert(commentobj);
            var state = comment.save() > 0 ? 1 : 0;
            return new { state = state, msg = state == 1 ? "评论成功" : "评论失败" };
        }

        #region 写入评论 辅助方法
        /// <summary>
        /// 根据用户id 取用户昵称
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private string GetUserName(int userID)
        {
            var user = BLL.Common.GetDataHelper.GetAllUser().Where(t => t.Id == userID).FirstOrDefault();
            if (user != null)
                return string.IsNullOrEmpty(user.UserNickname) ? user.UserName : user.UserNickname;
            return string.Empty;
        }

        private BlogComment GetCommentInfo(int id, int blogid)
        {
            if (id <= 0)
                return new BlogComment();
            BLL.BaseBLL<BlogComment> commentbll = new BaseBLL<BlogComment>();
            return commentbll.GetList(t => t.Id == id && t.BlogInfo.Id == blogid).FirstOrDefault();
        }
        #endregion

        #endregion

        #region 登录 返回token
        /// <summary>
        /// 登录 返回token
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPas">密码</param>
        /// <returns></returns>
        public object GetLoginToken(string userName, string userPas)
        {
            RemoveToKen();
            userPas = userPas.MD5();
            //if ( GetDataHelper.GetAllUser().Count() <= 0)
            //    BLL.Common.CacheData.GetAllUserInfo(true);

            var userTemp =  GetDataHelper.GetAllUser()
                .Where(t => (t.UserName == userName || t.UserMail == userName) && t.UserPass == userPas && t.IsLock == false).FirstOrDefault();
            if (userTemp != null)
            {
                string toKenKey = Guid.NewGuid().ToString("N");
                if (ToKens.Keys.Contains(toKenKey))
                    ToKens[toKenKey].Time = DateTime.Now;
                else
                    ToKens.Add(toKenKey, new ToKenModel() { Name = userName, Time = DateTime.Now });
                return new
                {
                    state = 1,
                    msg = toKenKey,
                    userInfo = new BlogUser()
                    {
                        Id = userTemp.Id,
                        UserName = userTemp.UserName,
                        UserMail = userTemp.UserMail,
                        UserNickname = userTemp.UserNickname
                    }
                };
            }
            else
                return new { state = 0, msg = "用户名或密码错误", userInfo = string.Empty };
        }
        #endregion
    }
}
