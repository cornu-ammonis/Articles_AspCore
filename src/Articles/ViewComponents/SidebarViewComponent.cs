using Articles.Models;
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
            WidgetViewModel widgetViewModel = new WidgetViewModel(_blogRepository);

            return View("_Sidebars", widgetViewModel);
        }
    }
}
