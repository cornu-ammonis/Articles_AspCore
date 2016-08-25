using Articles.Models;
using Articles.Models.BlogViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.ViewComponents
{
    public class PaginationViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(int total_posts)
        {
            PaginationViewModel paginationViewModel = new PaginationViewModel();
            paginationViewModel.n_visible = false;
            paginationViewModel.p_visible = false;

            var queryStrings = Request.Query;
            var keys = queryStrings.Keys;
            if (keys.Contains("p"))
            {
                paginationViewModel.current_page = int.Parse(queryStrings["p"]);
            }
            else
            {
                paginationViewModel.current_page = 1;
            }

            paginationViewModel.total_pages = Math.Ceiling((double)total_posts / 10);

            if (paginationViewModel.current_page > 1 )
            {
                paginationViewModel.p_visible = true;
            }

            if (paginationViewModel.current_page < paginationViewModel.total_pages)
            {
                paginationViewModel.n_visible = true;
            }

            paginationViewModel.previous_page = string.Format("p={0}", paginationViewModel.current_page - 1);
            paginationViewModel.next_page = string.Format("p={0}", paginationViewModel.current_page + 1);

             if (RouteData.Values["action"].ToString().Equals("category", StringComparison.OrdinalIgnoreCase))
            {
                var c = string.Format("?category={0}", queryStrings["category"]);
                paginationViewModel.previous_page = string.Format("{0}&{1}", c, paginationViewModel.previous_page);
                paginationViewModel.next_page = string.Format("{0}&{1}", c, paginationViewModel.next_page);
            }
            else if (RouteData.Values["action"].ToString().Equals("search", StringComparison.OrdinalIgnoreCase))
            {
                var s = string.Format("?s={0}", queryStrings["s"]);
                paginationViewModel.previous_page = string.Format("{0}&{1}", s, paginationViewModel.previous_page);
                paginationViewModel.next_page = string.Format("{0}&{1}", s, paginationViewModel.next_page);
            }
            
            else if (RouteData.Values["action"].ToString().Equals("tag", StringComparison.OrdinalIgnoreCase))
            {
                var t = string.Format("?tag={0}", queryStrings["tag"]);
                paginationViewModel.previous_page = string.Format("{0}&{1}", t, paginationViewModel.previous_page);
                paginationViewModel.next_page = string.Format("{0}&{1}", t, paginationViewModel.next_page);
            }
            else
            {
                paginationViewModel.previous_page = string.Concat("?", paginationViewModel.previous_page);
                paginationViewModel.next_page = string.Concat("?", paginationViewModel.next_page);
            }

            return View(paginationViewModel);
        }
    }
}
