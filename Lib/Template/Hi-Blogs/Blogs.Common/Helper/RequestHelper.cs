using Common.HiLogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Blogs.Common.Helper
{
    public class RequestHelper
    {
        /// <summary>
        /// 获取RequestIP
        /// </summary>
        /// <returns></returns>
        public static string GetIp()
        {
            string ip = string.Empty;
            try
            {
                if (HttpContext.Current.Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR") != null)
                    ip = HttpContext.Current.Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR").ToString().Trim();
                else
                    ip = HttpContext.Current.Request.ServerVariables.Get("Remote_Addr").ToString().Trim();
            }
            catch (Exception ex)
            {
                LogSave.ErrLogSave("获取IP异常:", ex);
            }
            return ip;
        }
    }
}
