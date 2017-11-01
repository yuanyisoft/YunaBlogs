using Blogs.BLL.Application.AdminHelper.Dto;
using Blogs.Common;
using Blogs.Helper;
using Blogs.ModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;


namespace Blogs.BLL.Application.AdminHelper
{
    public class AdminHelperApplication
    {
        /// <summary>
        /// 保存数据库连接
        /// </summary>
        /// <param name="input"></param>
        public void SaveConnect(SaveConnectInput input)
        {
            var connectionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};"
              , input.DataSource
              , input.InitialCatalog
              , input.UserID
              , input.Password);
            ConfigHelper.SetConnectionString("HiBlogsTemp", connectionString);
        }


        /// <summary>
        /// 测试数据库连接
        /// </summary>
        /// <returns></returns>
        public string TestConnect()
        {
            BlogDbContext db = new BlogDbContext();
            db.BlogUsers.FirstOrDefault();
            return "ok";
        }
        /// <summary>
        /// 保存邮件配置
        /// </summary>
        /// <param name="input"></param>
        public void SaveEmail(SaveEmailInput input)
        {
            ConfigHelper.Custom.Mail.From = input.MailFrom;
            ConfigHelper.Custom.Mail.Pwd = input.MailPwd;
            ConfigHelper.Custom.Mail.Host = input.MailHost;
            ConfigHelper.Sava();
        }
        /// <summary>
        /// 测试邮件
        /// </summary>
        public bool TestEmail()
        {
            EmailHelper email = new EmailHelper()
            {
                mailPwd = ConfigHelper.Custom.Mail.Pwd, //
                host = ConfigHelper.Custom.Mail.Host, //
                mailFrom = ConfigHelper.Custom.Mail.From,//
                mailSubject = "邮件测试",
                mailBody = EmailHelper.tempBody("", "恭喜您邮箱配置正确，可以正常使用hi-blogs系统了！", "", isShow: false),
                mailToArray = new string[] { ConfigHelper.Custom.Mail.From }
            };
            return email.Send();
        }
    }
}
