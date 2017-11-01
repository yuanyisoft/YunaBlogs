using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.BLL.Application.Admin.Dto
{
    public class UserTagTypesOutput
    {
        public List<ModelDB.Entities.BlogTag> BlogTags { get; set; }

        public List<ModelDB.Entities.BlogType> BlogTypeS { get; set; }
    }
}
