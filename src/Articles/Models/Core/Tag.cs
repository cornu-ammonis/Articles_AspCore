using Articles.Data;
using Articles.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Articles.Models
{
   public class Tag
    {

        public  int TagId { get; set; }
        public  string  Name { get; set; }
        public  string UrlSlug  { get; set; }
        public  string Description { get; set; }

       
        public List<PostTag> PostTags { get; set; }



    }
}
