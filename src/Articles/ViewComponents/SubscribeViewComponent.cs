using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Articles.Models;
using Articles.Models.Core;

namespace Articles.ViewComponents
{
    public class SubscribeViewComponent : ViewComponent
    {
        IBlogRepository _blogRepository;

        public SubscribeViewComponent(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(BlogUser author)
        {
            if (User.Identity.IsAuthenticated)
            {
                bool subscribed = await _blogRepository.CheckIfSubscribedAsync(User.Identity.Name, author.user_name);
                if(subscribed)
                {
                    return View("Unsubscribe", author);
                }
                else
                {
                    return View("Subscribe", author);
                }
            }
            else
            {
                return View("Subscribe", author);
            }
        }

    }
}