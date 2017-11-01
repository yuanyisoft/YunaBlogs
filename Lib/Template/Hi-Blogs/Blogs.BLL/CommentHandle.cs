using Blogs.BLL.Common;
using Blogs.ModelDB;
using Blogs.ModelDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.BLL
{
    /// <summary>
    /// 评论数据 操作逻辑类
    /// </summary>
    public class CommentHandle
    {
        #region 根据博客文章id 取相关评论  ()
        /// <summary>
        /// 根据博客文章id 取相关评论  ()
        /// </summary>
        /// <param name="blogId"></param>
        public List<List<BlogComment>> GetComment(int blogId, int pageIndex, int order)
        {

            var isOrder = order == 0;
            int total;
            BLL.BaseBLL<BlogComment> com = new BaseBLL<BlogComment>();
            //IsInitial == true 父评论 （第一次数据库查询：查询30条父评论）
            List<int> disCom = com.GetList<int>(pageIndex, 30, out total, t => t.IsInitial == true && t.BlogInfo.Id == blogId, false, t => t.Id, isOrder).Select(t => t.Id).ToList();
            if (pageIndex > total)//已经没有评论信息了
            {
                return null;
            }
            //第二次数据库查询：查询30条父评论 和30条父评论下的子评论
            var listCom = com.GetList(t => disCom.Contains(t.CommentID) || disCom.Contains(t.Id)).ToList();
            List<List<BlogComment>> ComObj = new List<List<BlogComment>>();
            var ini = listCom.Where(t => t.IsInitial == true).ToList();//这里就不查数据库了直接进行集合筛选
            if (isOrder)
                ini = ini.OrderBy(t => t.CreationTime).ToList();
            else
                ini = ini.OrderByDescending(t => t.CreationTime).ToList();
            //对评论进行分组（以父评论 分组）
            foreach (BlogComment item in ini)
            {
                item.BlogUser = GetDataHelper.GetAllUser().Where(t => t.Id == item.BlogUser.Id).FirstOrDefault();
                var userobj = GetDataHelper.GetAllUser().Where(t => t.Id == item.ReplyUserID).FirstOrDefault();
                if (null != userobj)
                    item.ReplyUserName = string.IsNullOrEmpty(userobj.UserNickname) ? item.AnonymousName : userobj.UserNickname;
                //添加 以父评论 为一分组 的评论
                ComObj.Add(GetCom(item, listCom));
            }
            return ComObj;
        }
        #endregion

        #region 取 顶级评论 及下的子评论
        /// <summary>
        /// 取 顶级评论 及下的子评论
        /// </summary>
        /// <param name="com"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<BlogComment> GetCom(BlogComment com, List<BlogComment> list)
        {
            var li = list.Where(t => t.CommentID == com.Id).ToList();
            li.Insert(0, com);
            return li;
        }
        #endregion
    }
}
