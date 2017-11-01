using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.BLL.Application.AdminHelper.Dto
{
    public class SaveEmailInput
    {
        public string MailHost { get; set; }
        public string MailFrom { get; set; }
        public string MailPwd { get; set; }
    }
}
