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
                string current_action = RouteData.Values["action"].ToString().ToLower();
                if (current_action == "savedposts" || current_action=="undounsave")
                {

                    return View("UnsaveFromSavedList", post);


                }

                bool saved = await _blogRepository.CheckIfSavedAsync(post, User.Identity.Name);
               
                
                if(saved)
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