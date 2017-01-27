using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Articles.Models;
using Articles.Models.Core;

namespace Articles.ViewComponents
{
    public class AuthorAuthorizeViewComponent : ViewComponent
    {
        IBlogRepository _blogRepository;

        public AuthorAuthorizeViewComponent(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(string author_name)
        {
            if (User.Identity.IsAuthenticated && RouteData.Values["action"].ToString().ToLower().Equals("postsbyauthor"))
            {
                bool isAuthorized = await _blogRepository.CheckIfAuthorizedAsync(User.Identity.Name, author_name);
                if (isAuthorized)
                {
                    return View("UnAuthorize", author_name);
                }
                else
                {
                    return View("Authorize", author_name);
                }
            }
            else
            {
                return View("Empty");
            }
        }

    }
}