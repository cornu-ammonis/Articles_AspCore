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
            Tags = blogRepository.Tags();
            Posts = blogRepository.Posts(0, 5);
        }

        public IList<Category> Categories { get; private set; }
        public IList<Tag> Tags { get; private set; }
        public IList<Post> Posts { get; private set; }
    }
}