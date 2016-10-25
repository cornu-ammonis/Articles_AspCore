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
    public class Post
    {

        public  int PostId
        { get; set; }

        public  string Title
        { get; set; }

        public  string ShortDescription
        { get; set; }

        public  string Description
        { get; set; }

        public  string Meta
        { get; set; }

        public  string UrlSlug
        { get; set; }

        public  bool Published
        { get; set; }

        public virtual DateTime PostedOn
        { get; set; }

        public virtual DateTime? Modified
        { get; set; }

        public virtual Category Category
        { get; set; }
        
        public List<PostTag> PostTags 
        { get; set; }

        public virtual BlogUser Author
        { get; set; }

        public int LikeCount
        { get; set; } = 0;

        public List<PostUserSave> PostUserSaves { get; set; }
        public List<PostUserLike> PostUserLikes { get; set; }

    }
}
