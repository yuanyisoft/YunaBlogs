using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Blogs.BLL.WebApi
{
    public class TokenValidateAttribute : ActionFilterAttribute
    {
        public const string SessionKeyName = "toKen";
        public const string LogonUserName = "LogonUser";

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            var qs = HttpUtility.ParseQueryString(filterContext.Request.RequestUri.Query);
            string key = qs[SessionKeyName]; 
            if (string.IsNullOrEmpty(key))
                throw new Exception("无toKen");
            var user = UserOperation.GetLoginUser(key);
            filterContext.ControllerContext.RouteData.Values[LogonUserName] = user;
        }
    }
}
