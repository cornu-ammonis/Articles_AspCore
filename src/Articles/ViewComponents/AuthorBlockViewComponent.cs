using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Articles.Models;
using Articles.Models.Core;

namespace Articles.ViewComponents
{
    public class AuthorBlockViewComponent : ViewComponent
    {
        IBlogRepository _blogRepository;

        public AuthorBlockViewComponent(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(string author_name)
        {
            if (User.Identity.IsAuthenticated && RouteData.Values["action"].ToString().ToLower().Equals("postsbyauthor"))
            {
                bool isBlocked = await _blogRepository.CheckIfBlockedAsync(User.Identity.Name, author_name);
                if (isBlocked)
                {
                    return View("Unblock", author_name);
                }
                else
                {
                    return View("Block", author_name);
                }
            }
            else
            {
                return View("Empty");
            }
        }

    }
}