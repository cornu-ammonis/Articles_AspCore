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
        public async Task<IViewComponentResult> InvokeAsync(BlogUser author)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (await _blogRepository.CheckIfBlockedAsync(User.Identity.Name, author.user_name))
                {
                    return View("Unblock", author);
                }
                else
                {
                    return View("Block", author);
                }
            }
            else
            {
                return View("Empty");
            }
        }

    }
}