using Blogs.BLL.Application.Admin.Dto;
using Blogs.BLL.Common;
using Blogs.Common.CustomModel;
using Blogs.Helper;
using Blogs.ModelDB.Entities;
using CommonLib.HiLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.BLL.Application.Admin
{
    public class AdminApplication
    {
        /// <summary>
        /// 管理员特权
        /// </summary>
        private readonly static string admin = "admin";

        public BlogInfo ReleaseGet(int? id)
        {
            var userinfo = BLLSession.UserInfoSessioin;
            BlogInfo blog = new BlogInfo();
            if (null != id)
            {
                BLL.BaseBLL<BlogInfo> blogbll = new BaseBLL<BlogInfo>();
                blog = blogbll.GetList(t => t.Id == id && (t.User.Id == userinfo.Id || userinfo.UserName == admin)).FirstOrDefault();
            }
            return blog;
        }

        /// <summary>
        /// 提交内容的编辑或修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public JSData ReleasePost(ReleaseInput input)
        {
            JSData jsdata = new JSData();

            #region 数据验证
            if (null == BLL.Common.BLLSession.UserInfoSessioin)
                jsdata.Messg = "您还未登录~";
            else if (BLL.Common.BLLSession.UserInfoSessioin.IsLock)
                jsdata.Messg = "您的账户未激活，暂只能评论。~";
            else if (string.IsNullOrEmpty(input.Content))
                jsdata.Messg = "内容不能为空~";

            if (!string.IsNullOrEmpty(jsdata.Messg))
            {
                jsdata.State = EnumState.失败;
                return jsdata;
            }
            #endregion

            BLL.BaseBLL<BlogInfo> blogbll = new BaseBLL<BlogInfo>();
            var blogtemp = blogbll.GetList(t => t.Id == input.Blogid, isAsNoTracking: false).FirstOrDefault();
            var userid = input.Blogid > 0 ? blogtemp.User.Id : BLLSession.UserInfoSessioin.Id;//如果numblogid大于〇证明 是编辑修改
            var sessionuserid = BLLSession.UserInfoSessioin.Id;

            //获取得 文章 类型集合 对象
            var typelist = new List<int>();
            if (!string.IsNullOrEmpty(input.Chk_type))
                foreach (string type in input.Chk_type.Split(',').ToList())
                {
                    if (!string.IsNullOrEmpty(type))
                        typelist.Add(int.Parse(type));
                }
            // types.Split(',').ToList().ForEach(t => typelist.Add(int.Parse(t)));
            var myBlogTypes = new BLL.BaseBLL<BlogType>().GetList(t => typelist.Contains(t.Id), isAsNoTracking: false).ToList();

            //获取得 文章 tag标签集合 对象
            //old
            var oldtaglist = string.IsNullOrEmpty(input.Oldtag) ? new List<string>() : input.Oldtag.Split(',').ToList();
            var myOldTagTypes = new BLL.BaseBLL<BlogTag>().GetList(t => t.BlogUser.Id == userid && oldtaglist.Contains(t.TagName), isAsNoTracking: false).ToList();
            //new           
            var newtaglist = input.Newtag.GetValueOrEmpty().Split(',').ToList();
            AddTag(newtaglist, userid);//保存到数据库
            var myNweTagTypes = new BLL.BaseBLL<BlogTag>().GetList(t => t.BlogUser.Id == userid && newtaglist.Contains(t.TagName), isAsNoTracking: false).ToList();
            myNweTagTypes.ForEach(t => myOldTagTypes.Add(t));


            if (input.Blogid > 0)  //如果有 blogid 则修改
            {
                if (sessionuserid == blogtemp.User.Id || BLLSession.UserInfoSessioin.UserName == admin) //一定要验证更新的博客是否是登陆的用户
                {
                    blogtemp.Content = input.Content;
                    blogtemp.Title = input.Title;
                    blogtemp.IsShowMyHome = input.Isshowmyhome;
                    blogtemp.IsShowHome = input.Isshowhome;
                    blogtemp.Types.Clear();//更新之前要清空      不如会存在主外键约束异常
                    blogtemp.Types = myBlogTypes;
                    blogtemp.Tags.Clear();
                    blogtemp.Tags = myOldTagTypes;
                    blogtemp.IsDelte = false;
                    blogtemp.IsForwarding = false;
                    jsdata.Messg = "修改成功~";
                }
                else
                {
                    jsdata.Messg = "您没有编辑此博文的权限~";
                    jsdata.JSurl = "/";
                    jsdata.State = EnumState.失败;
                    return jsdata;
                }
            }
            else  //否则 新增
            {
                var blogfirst = blogbll.GetList(t => t.User.Id == sessionuserid).OrderByDescending(t => t.Id).FirstOrDefault();

                if (null != blogfirst && blogfirst.Title == input.Title)
                {
                    jsdata.Messg = "不能同时发表两篇一样标题的文章~";
                }
                else
                {
                    var bloguser = new BLL.BaseBLL<BlogUser>().GetList(t => t.Id == BLLSession.UserInfoSessioin.Id, isAsNoTracking: false).FirstOrDefault();

                    blogtemp = new BlogInfo()
                    {
                        User = bloguser,
                        Content = input.Content,
                        Title = input.Title,
                        BlogUpTime = DateTime.Now,
                        BlogCreateTime = DateTime.Now,
                        IsShowMyHome = input.Isshowmyhome,
                        IsShowHome = input.Isshowhome,
                        Types = myBlogTypes,
                        Tags = myOldTagTypes,
                        IsDelte = false,
                        IsForwarding = false
                    };
                    blogbll.Insert(blogtemp);
                    jsdata.Messg = "发布成功~";
                }
            }

            //
            if (blogbll.save(false) > 0)
            {
                #region 添加 或 修改搜索索引
                try
                {
                    var newtagList = string.Empty;
                    blogtemp.Tags.Where(t => true).ToList().ForEach(t => newtagList += t.TagName + " ");
                    var newblogurl = "/" + BLLSession.UserInfoSessioin.UserName + "/" + blogtemp.Id + ".html";
                    SearchResult search = new SearchResult()
                    {
                        flag = blogtemp.User.Id,
                        id = blogtemp.Id,
                        title = blogtemp.Title,
                        clickQuantity = 0,
                        blogTag = newtagList,
                        content = Blogs.Common.Helper.MyHtmlHelper.GetHtmlText(blogtemp.Content),
                        url = newblogurl
                    };
                    SafetyWriteHelper<SearchResult>.logWrite(search, PanGuLuceneHelper.instance.CreateIndex);
                }
                catch (Exception)
                { }
                #endregion

                jsdata.State = EnumState.成功;
                jsdata.JSurl = "/" + GetDataHelper.GetAllUser().Where(t => t.Id == blogtemp.User.Id).First().UserName + "/" + blogtemp.Id + ".html";
                return jsdata;
            }

            jsdata.Messg = string.IsNullOrEmpty(jsdata.Messg) ? "操作失败~" : jsdata.Messg;
            jsdata.State = EnumState.失败;
            return jsdata;
        }

        #region 添加新的tag标签
        /// <summary>
        /// 添加新的tag标签
        /// </summary>
        /// <param name="taglist"></param>
        /// <returns></returns>
        private bool AddTag(List<string> taglist, int userid)
        {
            BLL.BaseBLL<BlogTag> blogtype = new BaseBLL<BlogTag>();
            foreach (string tag in taglist)
            {
                if (string.IsNullOrEmpty(tag) || blogtype.GetList(t => t.TagName == tag).Count() > 0)
                    continue;

                var user = new BLL.BaseBLL<BlogUser>().GetList(t => t.Id == userid, isAsNoTracking: false).FirstOrDefault();
                blogtype.Insert(new BlogTag()
                {
                    TagName = tag,
                    CreationTime = DateTime.Now,
                    IsDelte = false,
                    BlogUser = user
                });
            }
            return blogtype.save() > 0;
        }
        #endregion

        /// <summary>
        /// 设置 移动 端 主题样式
        /// </summary>
        /// <returns></returns>
        public ConfigureOutput ConfigureMobile()
        {
            ConfigureOutput configure = new ConfigureOutput();
            configure.confcss = "";
            configure.confhtml = "";
            configure.confjs = "";
            if (BLLSession.UserInfoSessioin != null)
            {
                string path = FileHelper.defaultpath + "/MyConfigure/" + BLLSession.UserInfoSessioin.UserName + "/";
                FileHelper.CreatePath(path);
                configure.confcss = FileHelper.GetFile(path, "Mconf.css");
                configure.confsidehtml = FileHelper.GetFile(path, "Mconf_side.txt");
                configure.conffirsthtml = FileHelper.GetFile(path, "Mconf_first.txt");
                configure.conftailhtml = FileHelper.GetFile(path, "Mconf_tail.txt");
                configure.confjs = FileHelper.GetFile(path, "Mconf.js");
                configure.IsShowCSS = BLLSession.UserInfoSessioin.BlogUserInfo.IsShowMCSS;
                configure.IsDisCSS = BLLSession.UserInfoSessioin.BlogUserInfo.IsDisMCSS;
                configure.TerminalType = "Mobile";
            }
            return configure;
        }

        /// <summary>
        /// 设置 PC 端 主题样式
        /// </summary>
        /// <returns></returns>
        public ConfigureOutput ConfigurePC()
        {
            ConfigureOutput configure = new ConfigureOutput();
            configure.confcss = "";
            configure.confhtml = "";
            configure.confjs = "";
            if (BLLSession.UserInfoSessioin != null)
            {
                string path = FileHelper.defaultpath + "/MyConfigure/" + BLLSession.UserInfoSessioin.UserName + "/";
                FileHelper.CreatePath(path);
                configure.confcss = FileHelper.GetFile(path, "conf.css");
                configure.confsidehtml = FileHelper.GetFile(path, "conf_side.txt");
                configure.conffirsthtml = FileHelper.GetFile(path, "conf_first.txt");
                configure.conftailhtml = FileHelper.GetFile(path, "conf_tail.txt");
                configure.confjs = FileHelper.GetFile(path, "conf.js");
                configure.IsShowCSS = BLLSession.UserInfoSessioin.BlogUserInfo.IsShowCSS;
                configure.IsDisCSS = BLLSession.UserInfoSessioin.BlogUserInfo.IsDisCSS;
                configure.TerminalType = "PC";
            }
            return configure;
        }

        public string Configure(ConfigureInput input)
        {
            var IsShowCSS = input.IsShowCSS == "on";
            var IsDisCSS = input.IsDisCSS == "on";
            if (BLLSession.UserInfoSessioin == null)
                return "您还没有登录 不能修改~";
            try
            {
                //==============================================================================================================
                //遗留问题：
                //如下：如果 userinfobll.Up(BLLSession.UserInfoSessioin.BlogUserInfo)两次的话 报异常：[一个实体对象不能由多个 IEntityChangeTracker 实例引用]
                //那么 我只能 new一个新的对象 修改  然后 同时 BLLSession.UserInfoSessioin.BlogUserInfo里面的属性，不然 其他地方访问的话 是没有修改过来的值
                //==============================================================================================================
                var userinftemp = new BlogUserInfo(); //BLLSession.UserInfoSessioin.BlogUserInfo;
                BLL.BaseBLL<BlogUserInfo> userinfobll = new BaseBLL<BlogUserInfo>();
                if (input.TerminalType == "PC")//如果是PC端
                {
                    userinftemp.IsShowCSS =
                        BLLSession.UserInfoSessioin.BlogUserInfo.IsShowCSS = IsShowCSS;
                    userinftemp.IsDisCSS =
                        BLLSession.UserInfoSessioin.BlogUserInfo.IsDisCSS = IsDisCSS;
                    userinftemp.Id =
                        BLLSession.UserInfoSessioin.BlogUserInfo.Id;
                    userinfobll.Updata(userinftemp, "IsShowCSS", "IsDisCSS");//"IsShowHTML",, "IsShowJS"
                }
                else
                {
                    userinftemp.IsShowMCSS =
                      BLLSession.UserInfoSessioin.BlogUserInfo.IsShowMCSS = IsShowCSS;
                    userinftemp.IsDisMCSS =
                        BLLSession.UserInfoSessioin.BlogUserInfo.IsDisMCSS = IsDisCSS;
                    userinftemp.Id =
                        BLLSession.UserInfoSessioin.BlogUserInfo.Id;
                    userinfobll.Updata(userinftemp, "IsShowMCSS", "IsDisMCSS");
                }

                GetDataHelper.GetAllUser().FirstOrDefault(t => t.Id == BLLSession.UserInfoSessioin.Id).BlogUserInfo
                    = BLLSession.UserInfoSessioin.BlogUserInfo;

                userinfobll.save();

                string path = FileHelper.defaultpath + "/MyConfigure/" + BLLSession.UserInfoSessioin.UserName + "/";
                FileHelper.CreatePath(path);
                if (input.conf_css.Length >= 40000 ||
                    input.conf_tail_html.Length >= 40000 ||
                    input.conf_first_html.Length >= 40000 ||
                    input.conf_side_html.Length >= 40000 ||
                    input.conf_js.Length >= 40000)
                {
                    return "您修改的内容字符过多~";
                }

                if (input.TerminalType == "PC")//如果是PC端
                {
                    FileHelper.SaveFile(path, "conf.css", input.conf_css);
                    FileHelper.SaveFile(path, "conf_side.txt", input.conf_side_html);
                    FileHelper.SaveFile(path, "conf_first.txt", input.conf_first_html);
                    FileHelper.SaveFile(path, "conf_tail.txt", input.conf_tail_html);
                    FileHelper.SaveFile(path, "conf.js", input.conf_js);
                }
                else
                {
                    FileHelper.SaveFile(path, "Mconf.css", input.conf_css);
                    FileHelper.SaveFile(path, "Mconf_side.txt", input.conf_side_html);
                    FileHelper.SaveFile(path, "Mconf_first.txt", input.conf_first_html);
                    FileHelper.SaveFile(path, "Mconf_tail.txt", input.conf_tail_html);
                    FileHelper.SaveFile(path, "Mconf.js", input.conf_js);
                }


                return "修改成功~";
            }
            catch (Exception ex)
            {
                LogSave.ErrLogSave("自定义样式出错", ex);
                return "修改失败~";
            }
        }

        /// <summary>
        /// 获取用户下的所有标签和文章分类
        /// </summary>
        /// <returns></returns>
        public UserTagTypesOutput GetUserTagTypes()
        {
            UserTagTypesOutput userTagTypes = new UserTagTypesOutput();
            userTagTypes.BlogTags = GetDataHelper.GetAllTag(BLLSession.UserInfoSessioin.Id).ToList();
            userTagTypes.BlogTypeS = GetDataHelper.GetAllType(BLLSession.UserInfoSessioin.Id).ToList();
            return userTagTypes;
        }
    }
}
