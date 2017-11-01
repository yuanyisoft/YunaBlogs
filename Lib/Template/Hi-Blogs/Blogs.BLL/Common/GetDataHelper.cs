using Blogs.ModelDB;
using Blogs.ModelDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.BLL.Common
{
    /// <summary>
    /// 获取非缓存数据
    /// </summary>
    public class GetDataHelper
    {
        /// <summary>
        /// 获取用户所有文章类型
        /// </summary>
        /// <returns></returns>
        public static IQueryable<BlogType> GetAllType(string name)
        {
            BLL.BaseBLL<BlogType> type = new BaseBLL<BlogType>();
            return type.GetList(t => t.BlogUser.UserName == name); 
        }

        public static IQueryable<BlogType> GetAllType(int id)
        {
            BLL.BaseBLL<BlogType> type = new BaseBLL<BlogType>();
            return type.GetList(t => t.BlogUser.Id == id);
        }

        public static IQueryable<BlogType> GetAllType()
        {
            BLL.BaseBLL<BlogType> type = new BaseBLL<BlogType>();
            return type.GetList(t => true);
        }

        /// <summary>
        /// 获取用户所有文章标签
        /// </summary>
        /// <returns></returns>
        public static IQueryable<BlogTag> GetAllTag(string name)
        {
            BLL.BaseBLL<BlogTag> tag = new BLL.BaseBLL<BlogTag>();
            return tag.GetList(t => t.BlogUser.UserName == name); 
        }

        public static IQueryable<BlogTag> GetAllTag(int id)
        {
            BLL.BaseBLL<BlogTag> tag = new BLL.BaseBLL<BlogTag>();
            return tag.GetList(t => t.BlogUser.Id == id);
        }

        public static IQueryable<BlogTag> GetAllTag()
        {
            BLL.BaseBLL<BlogTag> tag = new BLL.BaseBLL<BlogTag>();
            return tag.GetList(t => true);
        }

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        public static IQueryable<BlogUser> GetAllUser()
        {
            BLL.BaseBLL<BlogUser> user = new BLL.BaseBLL<BlogUser>();
            return user.GetList(t => true); 
        }

        public static IQueryable<BlogUser> GetAllUser<T>(Expression<Func<BlogUser, T>> TTbName, bool isAsNoTracking = true)
        {
            BLL.BaseBLL<BlogUser> user = new BLL.BaseBLL<BlogUser>();
            return user.GetList(t => true, tableName: TTbName, isAsNoTracking: isAsNoTracking);
        }

        /// <summary>
        /// 根据用户名  获取用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static BlogUser GetUser(string name)
        {
            BLL.BaseBLL<BlogUser> user = new BLL.BaseBLL<BlogUser>();
            return user.GetList(t => t.UserName == name).FirstOrDefault();
        }

        public static IQueryable<BlogUserInfo> GetUserInfo(int id)
        {
            BLL.BaseBLL<BlogUserInfo> userinfo = new BLL.BaseBLL<BlogUserInfo>();
            return userinfo.GetList(t => t.BlogUser.Id == id);
        }
    }
}
