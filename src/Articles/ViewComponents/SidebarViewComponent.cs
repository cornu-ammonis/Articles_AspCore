using Articles.Models;
using Articles.Models.BlogViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        IBlogRepository _blogRepository;   

            public SidebarViewComponent(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
           /* if (HttpContext.Items.ContainsKey("Widget.IsCustomizing"))
            {
                if ((bool)HttpContext.Items["Widget.IsCustomizing"])
                {
                    CustomizeViewModel customViewModel = new CustomizeViewModel(_blogRepository, User.Identity.Name);
                }
            } */
            WidgetViewModel widgetViewModel = new WidgetViewModel(_blogRepository);
            

            return View("_Sidebars", widgetViewModel);
        }
    }
}
