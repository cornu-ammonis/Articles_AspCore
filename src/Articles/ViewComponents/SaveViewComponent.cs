using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Articles.Models;

namespace Articles.ViewComponents
{
    public class SaveViewComponent : ViewComponent
    {
        IBlogRepository _blogRepository;

        public SaveViewComponent(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(Post post)
        {
            if (User.Identity.IsAuthenticated)
            {
                if(_blogRepository.CheckIfSaved(post, User.Identity.Name))
                {
                    return View("Unsave", post);
                }
                else
                {
                    return View("Save", post);
                }
            }
            else
            {
                return View("Save", post);
            }
        }

    }
}