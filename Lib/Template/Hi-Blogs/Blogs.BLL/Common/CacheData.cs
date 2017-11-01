using Blogs.ModelDB;
using Blogs.ModelDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Blogs.BLL.Common
{
    /// <summary>
    /// 缓存数据
    /// </summary>
    public static class CacheData
    {
        //#region 01 取得所有博客标签
        ///// <summary>
        ///// 取得所有博客标签
        ///// </summary>
        ///// <param name="newCache">是否重新获取</param>
        ///// <returns></returns>
        //public static List<BlogTag> GetAllTag(bool newCache = false)
        //{
        //    if (null == HttpRuntime.Cache["BlogTag"] || newCache)
        //    {
        //        BLL.BlogTagBLL tag = new BlogTagBLL();
        //        HttpRuntime.Cache["BlogTag"] = tag.GetList(t => true).ToList();
        //    }
        //    return (List<BlogTag>)HttpRuntime.Cache["BlogTag"];
        //}
        //#endregion

        //#region 02 获得所有博客的分类
        ///// <summary>
        ///// 获得所有博客的分类
        ///// </summary>
        ///// <param name="newCache">是否重新获取</param>
        ///// <returns></returns>
        //public static List<BlogType> GetAllType(bool newCache = false)
        //{
        //    if (null == HttpRuntime.Cache["BlogType"] || newCache)
        //    {
        //        BLL.BaseBLL<BlogType> tag = new BaseBLL<BlogType>();
        //        HttpRuntime.Cache["BlogType"] = tag.GetList(t => true).ToList().OrderBy(t => t.TypeName).ToList();
        //    }
        //    return (List<BlogType>)HttpRuntime.Cache["BlogType"];
        //}
        //#endregion

        //#region 03 获取所有博客的用户信息
        ///// <summary>
        ///// 获取所有博客的用户信息
        ///// </summary>
        ///// <param name="newCache">是否重新获取</param>
        ///// <returns></returns>
        //public static List<BlogUser> GetAllUserInfo(bool newCache = false)
        //{
        //    if (null == HttpRuntime.Cache["UserInfo"] || newCache)
        //    {
        //        BaseBLL<BlogUser> user = new BaseBLL<BlogUser>();
        //        HttpRuntime.Cache["UserInfo"] = user.GetList(t => true).ToList();
        //    }
        //    return (List<BlogUser>)HttpRuntime.Cache["UserInfo"];
        //}
        //#endregion
    }
}
