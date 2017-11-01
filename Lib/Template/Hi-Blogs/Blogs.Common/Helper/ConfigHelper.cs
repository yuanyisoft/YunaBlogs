using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Configuration;

namespace Blogs.Common
{
    public class ConfigHelper
    {
        private static Configuration config = WebConfigurationManager.OpenWebConfiguration("/");

        /// <summary>
        /// 静态文件版本控制号
        /// </summary>
        public static string EditionNumber
        {
            get
            {
                return GetAppSettings("EditionNumber");
            }
        }

        public static CustomCon _custom;

        /// <summary>
        /// 自定义配置（修改需要手动调用Sava保存）
        /// </summary>
        public static CustomCon Custom
        {
            get
            {
                if (_custom == null)
                    _custom = (CustomCon)config.GetSection("customCon");
                return _custom;
            }
        }

        /// <summary>
        /// 根据key获取AppSettings中的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSettings(string key)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
                return ConfigurationManager.AppSettings[key].ToString();
            return string.Empty;
        }

        /// <summary>
        /// 修改AppSettings中的值（如果不错在key，则新建）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetAppSetting(string key, string value)
        {
            AppSettingsSection appSetting = (AppSettingsSection)config.GetSection("appSettings");
            if (appSetting.Settings[key] == null)//如果不存在此节点,则添加  
            {
                appSetting.Settings.Add(key, value);
            }
            else//如果存在此节点,则修改  
            {
                appSetting.Settings[key].Value = value;
            }
            config.Save();
        }

        /// <summary>
        /// 修改数据库连接
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionString"></param>
        /// <param name="providerName"></param>
        public static void SetConnectionString(string key, string connectionString, string providerName = "System.Data.SqlClient")
        {
            ConnectionStringsSection connectionSetting = (ConnectionStringsSection)config.GetSection("connectionStrings");
            if (connectionSetting.ConnectionStrings[key] == null)//如果不存在此节点,则添加  
            {
                ConnectionStringSettings connectionStringSettings = new ConnectionStringSettings(key, connectionString, providerName);
                connectionSetting.ConnectionStrings.Add(connectionStringSettings);
            }
            else//如果存在此节点,则修改  
            {
                connectionSetting.ConnectionStrings[key].ConnectionString = connectionString;
                connectionSetting.ConnectionStrings[key].ProviderName = providerName;
            }
            config.Save();
        }

        /// <summary>
        /// 保存
        /// </summary>
        public static void Sava()
        {
            config.Save();
        }
    }

    /// <summary>
    /// 自定义配置
    /// </summary>
    public class CustomCon : ConfigurationSection
    {
        [ConfigurationProperty("mail", IsRequired = true)]
        public MailElement Mail
        {
            get { return (MailElement)this["mail"]; }
        }

        [ConfigurationProperty("oAuthQQ", IsRequired = true)]
        public OAuthQQElement OAuthQQ
        {
            get { return (OAuthQQElement)this["oAuthQQ"]; }
        }

        [ConfigurationProperty("oAuthSina", IsRequired = true)]
        public OAuthSinaElement OAuthSina
        {
            get { return (OAuthSinaElement)this["oAuthSina"]; }
        }
    }

    #region MailElement(邮箱)
    public class MailElement : ConfigurationElement
    {
        /// <summary>
        /// 发件人密码
        /// </summary>
        [ConfigurationProperty("mailPwd", IsRequired = true)]
        public string Pwd
        {
            get { return this["mailPwd"].ToString(); }
            set { this["mailPwd"] = value; }
        }

        /// <summary>
        /// SMTP邮件服务器
        /// </summary>
        [ConfigurationProperty("mailHost", IsRequired = true)]
        public string Host
        {
            get { return this["mailHost"].ToString(); }
            set { this["mailHost"] = value; }
        }

        /// <summary>
        ///发件人邮箱
        /// </summary>
        [ConfigurationProperty("mailFrom", IsRequired = true)]
        public string From
        {
            get { return this["mailFrom"].ToString(); }
            set { this["mailFrom"] = value; }
        }
    }
    #endregion

    #region OAuthQQElement（QQ）
    public class OAuthQQElement : ConfigurationElement
    {

        [ConfigurationProperty("OAuth_QQ_ClientId", IsRequired = true)]
        public string ClientId
        {
            get { return this["OAuth_QQ_ClientId"].ToString(); }
            set { this["OAuth_QQ_ClientId"] = value; }
        }


        [ConfigurationProperty("OAuth_QQ_ClientScrert", IsRequired = true)]
        public string ClientScrert
        {
            get { return this["OAuth_QQ_ClientScrert"].ToString(); }
            set { this["OAuth_QQ_ClientScrert"] = value; }
        }


        [ConfigurationProperty("OAuth_QQ_CallbackUrl", IsRequired = true)]
        public string CallbackUrl
        {
            get { return this["OAuth_QQ_CallbackUrl"].ToString(); }
            set { this["OAuth_QQ_CallbackUrl"] = value; }
        }
    }
    #endregion

    #region OAuthSinaElement(新浪)
    public class OAuthSinaElement : ConfigurationElement
    {

        [ConfigurationProperty("OAuth_Sina_ClientId", IsRequired = true)]
        public string ClientId
        {
            get { return this["OAuth_Sina_ClientId"].ToString(); }
            set { this["OAuth_Sina_ClientId"] = value; }
        }


        [ConfigurationProperty("OAuth_Sina_ClientScrert", IsRequired = true)]
        public string ClientScrert
        {
            get { return this["OAuth_Sina_ClientScrert"].ToString(); }
            set { this["OAuth_Sina_ClientScrert"] = value; }
        }


        [ConfigurationProperty("OAuth_Sina_CallbackUrl", IsRequired = true)]
        public string CallbackUrl
        {
            get { return this["OAuth_Sina_CallbackUrl"].ToString(); }
            set { this["OAuth_Sina_CallbackUrl"] = value; }
        }
    }
    #endregion

}
