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

        public int ViewCount
        { get; set; } = 0;

        public List<PostUserSave> PostUserSaves { get; set; }
        public List<PostUserLike> PostUserLikes { get; set; }

        public double DaysSincePosted()
        {
            DateTime now = DateTime.Now;

            TimeSpan difference = now.Subtract(this.PostedOn);
            return difference.TotalDays;
        }

        public double LikesPerDay()
        {
            if (this.LikeCount == 0)
            {
                return 0;
            }
            else
            {
                double likesPerDay = this.LikeCount / this.DaysSincePosted();
                return likesPerDay;
            }
        }

        public string LikesPerDayString()
        {
           

            string likesPerDayString = String.Format("{0} likes/day", this.LikesPerDay());
            return likesPerDayString;
        }

        public double ViewsPerDay()
        {
            if(this.ViewCount == 0)
            {
                return 0;
            }
            else
            {
                double viewsPerDay = this.ViewCount / this.DaysSincePosted();
                return viewsPerDay;
            }
        }

        public string ViewsPerDayString()
        {
            string viewsPerDayString = String.Format("{0} views/day", this.ViewsPerDay());
            return viewsPerDayString;
        }
        
        public string SaveAjaxId()
        {
            return this.UrlSlug + "save";
        }

        public string LikeAjaxId()
        {
            return this.UrlSlug + "like";
        }
        
      /*  public double HeatIndex(string type = "default", string user_name = null)
        {
            double likesPerDay = this.LikesPerDay();
            double viewsPerDay = this.ViewsPerDay();
            double heat = 0;

            //handles case where someone has liked a post but hasnt viewed it in full. really, a like should also count as a view,
            //but they are handled separately for now 
            if(viewsPerDay < likesPerDay)
            {
                viewsPerDay = likesPerDay;
            }

            heat = likesPerDay * viewsPerDay;
            

        }  */

    }
}
