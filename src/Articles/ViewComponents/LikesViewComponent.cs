using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Articles.Models;
using Articles.Models.Core;

namespace Articles.ViewComponents
{
    public class LikesViewComponent : ViewComponent
    {
        IBlogRepository _blogRepository;

        public LikesViewComponent(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(Post post)
        {
            if (User.Identity.IsAuthenticated)
            {
                bool liked = await _blogRepository.CheckIfLikedAsync(post, User.Identity.Name);
                if (liked)
                {
                    return View("Unlike", post);
                }
                else
                {
                    return View("Like", post);
                }
            }
            else
            {
                return View("Like", post);
            }

           
        }

    }
}