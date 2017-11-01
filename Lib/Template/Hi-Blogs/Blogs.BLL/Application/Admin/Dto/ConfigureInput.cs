using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.BLL.Application.Admin.Dto
{
    public class ConfigureInput
    {
        public string IsShowCSS { get; set; }

        public string IsDisCSS { get; set; }

        public string TerminalType { get; set; }

        public string conf_css { get; set; }

        public string conf_side_html { get; set; }

        public string conf_first_html { get; set; }

        public string conf_tail_html { get; set; }

        public string conf_js { get; set; }
    }
}
