using Blogs.ModelDB.DTO;
using Blogs.ModelDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.BLL.Application.Admin.Dto
{
    public class ReleaseOutput
    {
        public List<BlogTag> BlogTags { get; set; }

        public List<BlogType> BlogTypeS { get; set; }

        public BlogInfo BlogInfo { get; set; }

        public List<BlogsDTO> BlogInfos { get; set; }
    }
}
