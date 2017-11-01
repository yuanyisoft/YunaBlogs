using Blogs.BLL.Common;
using Blogs.Common;
using Blogs.Common.CustomModel;
using Blogs.Common.Helper;
using Blogs.Helper;
using Blogs.ModelDB.Entities;
using Common.HiLogHelper;
using GeRenXing.OpenPlatform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;


namespace Blogs.BLL.Application.UserManage
{
    public class UserManageApplication
    {
        public static string tempUserinfo = "tempUserinfo";
        private static string jihuoma = "jihuoma";
        /// <summary>
        /// 默认密码
        /// </summary>
        private static string defaulPass = "admin";
        private HttpSessionState Session = HttpContext.Current.Session;
        private HttpRequest Request = HttpContext.Current.Request;

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

        #region 登录

        public JSData Login(BlogUser user, string ischeck)
        {
            JSData objJson = new JSData();
            var pass = user.UserPass.MD5().MD5();
            var listUser = GetDataHelper.GetAllUser(t => t.BlogUserInfo).Where(t => (t.UserName == user.UserName || t.UserMail == user.UserName) && t.UserPass == pass);

            if (listUser.Count() > 0)
            {
                HttpContext.Current.Session[tempUserinfo] = listUser.FirstOrDefault();

                //验证邮箱是否有效  无效则跳转到邮箱验证页面
                if (listUser.Where(t => t.UserMail == "无效" || string.IsNullOrEmpty(t.UserMail)).Count() > 0)
                {
                    objJson.State = EnumState.失败;
                    objJson.Messg = "检测到你注册的邮箱无效~请输入正确的邮箱~";
                    objJson.JSurl = "/UserManage/EmailValidation";
                }
                else if (listUser.Where(t => t.IsLock == true).Count() > 0)//用户是否是激活状态 否:发送激活码 并跳转到激活页面                                     
                    GetActivate(ref objJson);
                else//登录成功
                {
                    #region 登录成功
                    BLLSession.UserInfoSessioin = listUser.FirstOrDefault();  //Messg = "登录成功",  //不给提示     直接跳转                   
                    objJson.State = EnumState.正常重定向;
                    if (!string.IsNullOrEmpty(Request.QueryString["href"]))
                        objJson.JSurl = Request.QueryString["href"];
                    else
                        objJson.JSurl = "/";
                    if (ischeck == "on")
                    {
                        HttpCookie Cookie = CookiesHelper.GetCookie("userInfo");
                        if (Cookie == null)
                        {
                            Cookie = new HttpCookie("userInfo");
                            Cookie.Values.Add("userName", user.UserName);
                            Cookie.Values.Add("userPass", user.UserPass);
                            //设置Cookie过期时间
                            Cookie.Expires = DateTime.Now.AddDays(365);
                            CookiesHelper.AddCookie(Cookie);
                        }
                        else
                        {
                            if (!Cookie.Values["userName"].Equals(user.UserName))
                                CookiesHelper.SetCookie("userInfo", "userName", user.UserName);
                            if (!Cookie.Values["userPass"].Equals(user.UserPass))
                                CookiesHelper.SetCookie("userInfo", "userPass", user.UserPass);
                        }
                    }
                    else
                    {
                        Helper.CookiesHelper.RemoveCookie("userInfo");
                    }
                    #endregion
                }
            }
            else
            {
                objJson.Messg = "用户名或密码错误~";
                objJson.State = EnumState.失败;
            }
            return objJson;
        }

        #region 第三方登录
        public string ThirdLogin(string loginType)
        {
            String authorizeUrl = String.Empty;
            IOAuthClient oauthClient = GetOAuthClient(loginType);
            oauthClient.Option.State = loginType;
            //第一步：获取开放平台授权地址
            authorizeUrl = oauthClient.GetAuthorizeUrl(ResponseType.Code);
            return authorizeUrl;
        }

        public string ThirdLoginCallback(string code, string loginType = "qq")
        {
            //第三步：获取开放平台授权令牌
            IOAuthClient oauthClient = GetOAuthClient(loginType);
            AuthToken accessToken = oauthClient.GetAccessTokenByAuthorizationCode(code);
            dynamic oauthProfile = oauthClient.User.GetUserInfo();
            if (accessToken != null && !string.IsNullOrEmpty(oauthClient.Token.User.OAuthId))
            {
                BLL.BaseBLL<BlogUserInfo> userInfo = new BLL.BaseBLL<BlogUserInfo>();
                var userInfoMode = userInfo.GetList(t => t.OAuthId == oauthClient.Token.User.OAuthId && t.OAuthName == loginType).FirstOrDefault();
                if (null == userInfoMode)
                {
                    BLL.BaseBLL<BlogUser> user = new BLL.BaseBLL<BlogUser>();
                    user.Insert(new BlogUser()
                    {
                        IsLock = true,
                        UserMail = "无效",
                        UserName = oauthClient.Token.User.OAuthId + "_" + loginType,
                        UserNickname = oauthClient.Token.User.Nickname,
                        UserPass = "pass".MD5().MD5(),
                        IsDelte = false,
                        BlogUserInfo = new BlogUserInfo()
                        {
                            OAuthId = oauthClient.Token.User.OAuthId,
                            OAuthName = loginType,
                            BlogUpNum = 0,
                            IsDelte = false
                        }
                    });
                    user.save(false);
                    userInfoMode = userInfo.GetList(t => t.OAuthId == oauthClient.Token.User.OAuthId && t.OAuthName == loginType).FirstOrDefault();
                    //BLL.Common.CacheData.GetAllUserInfo(true);
                }

                #region 设置cookie（等于设置了session，因为读session的时候会检测cookie）
                var userTemp = userInfoMode.BlogUser;
                HttpCookie Cookie = CookiesHelper.GetCookie("userInfo");
                if (Cookie == null)
                {
                    Cookie = new HttpCookie("userInfo");
                    Cookie.Values.Add("userName", userTemp.UserName);
                    Cookie.Values.Add("userPass", "pass");
                    //设置Cookie过期时间
                    Cookie.Expires = DateTime.Now.AddDays(365);
                    CookiesHelper.AddCookie(Cookie);
                }
                else
                {
                    if (!Cookie.Values["userName"].Equals(userTemp.UserName))
                        CookiesHelper.SetCookie("userInfo", "userName", userTemp.UserName);
                    if (!Cookie.Values["userPass"].Equals("pass"))
                        CookiesHelper.SetCookie("userInfo", "userPass", "pass");
                }
                //BLLSession.UserInfoSessioin = userInfoMode.BlogUser;
                #endregion

                return "ok";
            }
            return "no";
        }

        public IOAuthClient GetOAuthClient(string loginType)
        {
            #region 初始化参数
            string ClientId = string.Empty, ClientScrert = string.Empty, CallbackUrl = string.Empty;
            if (loginType == "qq")
            {
                ClientId = ConfigHelper.Custom.OAuthQQ.ClientId; //.GetAppSettings("OAuth_QQ_ClientId");
                ClientScrert = ConfigHelper.Custom.OAuthQQ.ClientScrert;//.GetAppSettings("OAuth_QQ_ClientScrert");
                CallbackUrl = ConfigHelper.Custom.OAuthQQ.CallbackUrl;//.GetAppSettings("OAuth_QQ_CallbackUrl");
            }
            else if (loginType == "sinaweibo")
            {
                ClientId = ConfigHelper.Custom.OAuthSina.ClientId;//.GetAppSettings("OAuth_Sina_ClientId");
                ClientScrert = ConfigHelper.Custom.OAuthSina.ClientScrert;//.GetAppSettings("OAuth_Sina_ClientScrert");
                CallbackUrl = ConfigHelper.Custom.OAuthSina.CallbackUrl;//.GetAppSettings("OAuth_Sina_CallbackUrl");
            }
            else
                return null;

            var iswww = Request.Url.AbsoluteUri.Substring(Request.Url.AbsoluteUri.IndexOf("//") + 2, 3) == "www";
            if (iswww)
                CallbackUrl = "http://www." + CallbackUrl;
            else
                CallbackUrl = "http://" + CallbackUrl;
            #endregion

            Dictionary<String, IOAuthClient> m_oauthClients = new Dictionary<string, IOAuthClient>();
            if (loginType == "qq")
                m_oauthClients[loginType] = new GeRenXing.OpenPlatform.OAuthClient.TencentQQClient(ClientId, ClientScrert, CallbackUrl);
            else if (loginType == "sinaweibo")
                m_oauthClients[loginType] = new GeRenXing.OpenPlatform.OAuthClient.SinaWeiBoClient(ClientId, ClientScrert, CallbackUrl);
            IOAuthClient oauthClient = m_oauthClients[loginType];
            return oauthClient;
        }
        #endregion


        #endregion

        #region 注册

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>      
        public JSData Regis(BlogUser blog)
        {
            var json = new JSData();

            #region 1.数据检验
            if (GetDataHelper.GetAllUser().Where(t => t.UserMail == blog.UserMail).Count() > 0)
                json.Messg = "此邮箱已经被注册~换个邮箱吧~";
            else if (GetDataHelper.GetAllUser().Where(t => t.UserName == blog.UserName).Count() > 0)
                json.Messg = "此用户名已经存在~";
            if (!string.IsNullOrEmpty(json.Messg))
            {
                json.State = EnumState.失败;
                return json;
            }
            #endregion

            BlogUser user = new BlogUser()
            {
                UserName = blog.UserName,
                UserPass = blog.UserPass,
                UserMail = blog.UserMail,
                UserNickname = blog.UserNickname,
                IsLock = true,
                BlogUserInfo = new BlogUserInfo()
            };
            HttpContext.Current.Session[tempUserinfo] = user;
            JSData jsdata = new JSData();

            #region 2.邮件发送失败
            if (!GetActivate(ref jsdata)) //
            {
                jsdata.State = EnumState.失败;
                jsdata.Messg = jsdata.Messg + " ~请重新输入邮箱~";
            }
            #endregion

            #region 3.邮件发送成功
            else
            {

                BLL.BaseBLL<BlogUser> userBll = new BLL.BaseBLL<BlogUser>();
                userBll.Insert(user);
                //在保存前 再做次验证
                if (GetDataHelper.GetAllUser().Where(t => t.UserName == blog.UserName || t.UserMail == blog.UserMail).Count() > 0)
                {
                    json.Messg = "此用户名后邮箱已经存在~";
                    json.State = EnumState.失败;
                    return json;
                }
                else
                    userBll.save();

                //验证是否注册成功 （并重新加载缓存信息）
                if (GetDataHelper.GetAllUser().Where(t => t.UserName == blog.UserName && t.UserPass == blog.UserPass).Count() > 0)
                {
                    BLLSession.UserInfoSessioin = user;
                    json.JSurl = "/UserManage/Activate";
                    json.Messg = "请查收邮件，完成账号激活~";
                }
                else
                {
                    json.Messg = "注册失败";
                    json.State = EnumState.失败;
                    return json;
                }
            }
            #endregion

            return json;
        }

        #endregion

        #region 激活 (实际上是验证激活码后  修改用户信息：包括是否激活IsLock、邮箱地址、密码 修改值是根据 Session[tempUserinfo] 里的值 )

        /// <summary>
        /// 激活 (实际上是验证激活码后  修改用户信息：包括是否激活IsLock、邮箱地址、密码 )
        /// </summary>
        /// <returns></returns>    
        public string Activate()
        {
            var json = new JSData();

            #region 1.判断是否从正常途径访问此页面 如果是的话 默认存在 Session[tempUserinfo]  2.如果是已经登录状态则直接无视 跳转

            if (null == Session[tempUserinfo])
            {
                json.State = EnumState.失败;//json.Messg = "请您通过正常途径访问激活页面~";
                json.JSurl = "/";
                return json.ToJson();
            }
            if (BLLSession.UserInfoSessioin != null && !BLLSession.UserInfoSessioin.IsLock)
            {
                json.State = EnumState.失败; //json.Messg = "您都已经登录的还想获取激活码？别玩了~";
                json.JSurl = "/";
                return json.ToJson();
            }
            #endregion

            var tempuser = ((BlogUser)HttpContext.Current.Session[tempUserinfo]);
            var activate = HttpContext.Current.Request.Form["txt_activate"];//激活码

            #region 2.验证激活码  (更新缓存 发送通知邮件 清空无用session)
            if (activate.Trim() == Session[jihuoma].ToString().Trim()) //验证激活码
            {
                BLL.BaseBLL<BlogUser> user = new BLL.BaseBLL<BlogUser>();
                var objuser = user.GetList(t => t.Id == tempuser.Id, isAsNoTracking: false).FirstOrDefault();
                var isEffectiveEmail = !string.IsNullOrEmpty(objuser.UserMail) && objuser.UserMail != "无效";//是否是有效邮箱（迁移用户是无效邮箱）
                if (null != objuser)
                {
                    objuser.IsLock = false;
                    objuser.UserPass = isEffectiveEmail ? tempuser.UserPass.MD5().MD5() : defaulPass.MD5().MD5();
                    objuser.UserMail = tempuser.UserMail;
                    if (objuser.BlogUserInfo == null)
                    {
                        objuser.BlogUserInfo = new BlogUserInfo()
                        {
                            BlogUpNum = 0
                        };
                    }
                }
                user.save();
                #region bug 记录
                //BlogUsers objuser = new BlogUsers();
                //objuser.Id = id;
                //objuser.IsLock = false;
                // user.Up(objuser, "IsLock");  //这个方法 正常情况用没问题，如果先添加   然后修改就有问题  （不能用）    
                #endregion
                bool islock = GetDataHelper.GetAllUser().Where(t => t.Id == tempuser.Id).FirstOrDefault().IsLock;
                if (!islock)
                {
                    #region 发送邮件 告知 激活成功

                    var tempSessionUser = (BlogUser)Session[tempUserinfo];
                    var nickName = string.IsNullOrEmpty(tempSessionUser.UserNickname) ? tempSessionUser.UserName : tempSessionUser.UserName;
                    Helper.EmailHelper email = new Helper.EmailHelper()
                    {
                        mailPwd = s_mailPwd,
                        host = s_host,
                        mailFrom = s_mailFrom,
                        mailSubject = "欢迎您注册 嗨-博客",
                        mailBody = EmailHelper.tempBody(nickName, "欢迎注册 嗨-博客",
                        "您注册的的帐号：" + objuser.UserName + "   密码是：" + (isEffectiveEmail ? tempuser.UserPass : defaulPass), "请您妥善保管~"),
                        mailToArray = new string[] { tempSessionUser.UserMail }
                    };

                    try
                    {
                        email.Send(t =>
                        {
                            LogSave.TrackLogSave("IP:" + RequestHelper.GetIp() + "\r\nToMail:" + tempSessionUser.UserMail + "\r\nBody:" + t.Body, "发送成功的邮件");
                        },
                                        t =>
                                        {
                                            LogSave.TrackLogSave("IP:" + RequestHelper.GetIp() + "\r\nToMail:" + tempSessionUser.UserMail + "\r\nBody:" + t.Body, "发送失败的邮件");
                                        }
                                   );
                    }
                    catch (Exception)
                    { }

                    #endregion

                    Session[jihuoma] = null;
                    Session[tempUserinfo] = null;

                    BLLSession.UserInfoSessioin = objuser;
                    return new JSData()
                    {
                        Messg = "恭喜您~激活成功~",
                        State = EnumState.正常重定向,
                        JSurl = "/"
                    }.ToJson();
                }
                else
                {
                    return new JSData()
                    {
                        Messg = "激活失败，请联系管理员~",
                        State = EnumState.失败
                    }.ToJson();
                }
            }
            #endregion

            return new JSData()
            {
                Messg = "您输入的激活码错误，你可以重新激活~",
                State = EnumState.失败
            }.ToJson();
        }

        #endregion

        #region  获取激活码
        /// <summary>
        /// 获取激活码 （邮件发送成功 默认跳转到激活页面）
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="mail">邮箱地址</param>
        /// <returns></returns>
        public bool GetActivate(ref JSData jsdata)
        {
            //验证是否正常途径访问获取激活码 方法
            if (null == Session[tempUserinfo])
            {
                jsdata.State = EnumState.失败; //json.Messg = "请您通过正常途径访问激活页面~";
                jsdata.JSurl = "/";
                return false;
            }

            #region 频繁获取验证处理
            if (Session["GetActivateTime"] == null)
                Session["GetActivateTime"] = DateTime.Now;
            if (Convert.ToDateTime(Session["GetActivateTime"]) > DateTime.Now)
            {
                jsdata.State = EnumState.失败;
                jsdata.Messg = "您获取激活码太过频繁，请15秒后再尝试。";
                LogSave.WarnLogSave("IP:" + RequestHelper.GetIp() + "\r\n获取激活码频繁", "获取激活码频繁");
                return false;
            }
            Session["GetActivateTime"] = DateTime.Now.AddSeconds(15);//设置时间 
            #endregion

            //session记录激活码
            Session[jihuoma] = new Random().Next(999999999).ToString();

            #region 发送邮件 如果邮件发送成功    默认跳转到 激活页面

            var tempSessionUser = (BlogUser)Session[tempUserinfo];
            var nickName = string.IsNullOrEmpty(tempSessionUser.UserNickname) ? tempSessionUser.UserName : tempSessionUser.UserName;
            Helper.EmailHelper email = new Helper.EmailHelper()
            {
                mailPwd = s_mailPwd,
                host = s_host,
                mailFrom = s_mailFrom,
                mailSubject = "[嗨-博客]激活码",
                mailBody = EmailHelper.tempBody(nickName, " 激活码：" + Session[jihuoma].ToString()),
                mailToArray = new string[] { tempSessionUser.UserMail }
            };

            try
            {
                email.Send(t =>
                {
                    LogSave.TrackLogSave("IP:" + RequestHelper.GetIp() + "\r\nToMail:" + tempSessionUser.UserMail + "\r\nBody:" + t.Body, "发送成功的邮件");
                }, t =>
                {
                    LogSave.TrackLogSave("IP:" + RequestHelper.GetIp() + "\r\nToMail:" + tempSessionUser.UserMail + "\r\nBody:" + t.Body, "发送失败的邮件");
                }
                                    );
                jsdata.State = EnumState.正常重定向;
                jsdata.Messg = "激活码已经发送邮箱~请注意查收~";
                jsdata.JSurl = "/UserManage/Activate";
                return true;
            }
            catch (Exception ex)
            {
                jsdata.State = EnumState.失败;
                jsdata.Messg = ex.Message;
                return false;
            }

            #endregion
        }
        /// <summary>
        /// 主要是通过post  且不需要返回值的时候
        /// </summary>
        /// <returns></returns>              
        public string GetActivate()
        {
            JSData jsdata = new JSData();
            GetActivate(ref jsdata);
            return jsdata.ToJson();
        }
        #endregion

        #region 重置密码

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>       
        public JSData ResetPass(BlogUser blog)
        {

            var pass = blog.UserPass;
            var email = blog.UserMail;

            var objJson = new JSData();

            #region 1.数据验证
            if (string.IsNullOrEmpty(pass.Trim()))
                objJson.Messg = "新密码不能为空~";
            if (string.IsNullOrEmpty(email.Trim()))
                objJson.Messg = "邮箱不能为空~";
            if (!string.IsNullOrEmpty(objJson.Messg))
            {
                objJson.State = EnumState.失败;
                return objJson;
            }
            #endregion

            var obj = GetDataHelper.GetAllUser().Where(t => t.UserMail == email);
            if (null == obj || obj.Count() <= 0)
            {
                objJson.State = EnumState.失败;
                objJson.Messg = "您输入的邮箱不是注册时候的邮箱~";
            }
            else
            {
                Session[tempUserinfo] = obj.FirstOrDefault();
                var userobj = (BlogUser)Session[tempUserinfo];
                userobj.UserPass = pass;//z                
                GetActivate(ref  objJson);
            }

            return objJson;
        }
        #endregion

        #region （无效邮箱）重新绑定邮箱


        /// <summary>
        /// （无效邮箱）重新绑定邮箱  邮箱发送成功 默认跳转到激活页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>       
        public JSData EmailValidation(string UserMail)
        {
            JSData jsdata = new JSData();
            jsdata.State = EnumState.失败;

            if (null == Session[tempUserinfo])
                jsdata.JSurl = "/UserManage/Login";
            else if (string.IsNullOrEmpty(UserMail) || string.IsNullOrEmpty(UserMail.Trim()))
                jsdata.Messg = "邮箱不能为空~";
            else if (GetDataHelper.GetAllUser().Where(t => t.UserMail == UserMail.Trim()).Count() >= 1)
                jsdata.Messg = "此邮箱已被占用~";
            else
            {
                var userobj = (BlogUser)Session[tempUserinfo];
                userobj.UserMail = UserMail;
                GetActivate(ref jsdata);
            }
            return jsdata;
        }
        #endregion

        #region 用户注销
        /// <summary>
        /// 用户注销
        /// </summary>
        public void Cancellation()
        {
            Helper.CookiesHelper.RemoveCookie("userInfo");
            BLL.Common.BLLSession.UserInfoSessioin = null;//注销
        }
        #endregion
    }
}
