using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.BLL.Application.AdminHelper.Dto
{
    public class SaveConnectInput
    {
        public string DataSource { get; set; }
        public string InitialCatalog { get; set; }

        public string UserID { get; set; }
        public string Password { get; set; }
    }
}
