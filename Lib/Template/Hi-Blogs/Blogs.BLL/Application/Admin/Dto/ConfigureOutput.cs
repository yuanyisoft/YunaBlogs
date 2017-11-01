using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.BLL.Application.Admin.Dto
{
    public class ConfigureOutput
    {
        public string confcss { get; set; }

        public string confhtml { get; set; }

        public string confjs { get; set; }

        public string confsidehtml { get; set; }

        public string conffirsthtml { get; set; }

        public string conftailhtml { get; set; }

        public bool? IsShowCSS { get; set; }

        public bool? IsDisCSS { get; set; }

        public string TerminalType { get; set; }
    }
}
