using System;
using System.Collections.Generic;
using System.Linq;

using Articles.Models.Core;

namespace Articles.Models
{
    public class WidgetViewModel
    {
        public WidgetViewModel(IBlogRepository blogRepository)
        {
            Categories = blogRepository.Categories();
            CategoryCounts = blogRepository.CategoryCounts(Categories);

            //changes each category name to category name + ("{0} posts", total_posts)
            foreach (Category category in Categories)
            {
                category.Name = category.Name + " " + CategoryCounts[category.UrlSlug];
            }

            Tags = blogRepository.Tags();
            Posts = blogRepository.Posts(0, 5);
        }

        public IList<Category> Categories { get; private set; }
        public IDictionary<string, string> CategoryCounts { get; private set; }
        public IList<Tag> Tags { get; private set; }
        public IList<Post> Posts { get; private set; }
    }
}