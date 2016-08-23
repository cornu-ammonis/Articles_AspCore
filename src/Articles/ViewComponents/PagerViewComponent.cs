using Articles.Models;
using Articles.Models.BlogViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.ViewComponents
{
    public class PagerViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(int total_posts)
        {
            PagerViewModel pagerViewModel = new PagerViewModel();
            pagerViewModel.n_visible = false;
            pagerViewModel.p_visible = false;
            var queryStrings = Request.Query;
            var keys = queryStrings.Keys;
            if (keys.Contains("p"))
            {
                pagerViewModel.CurrentPage = int.Parse(queryStrings["p"]);
            }
            else
            {
                pagerViewModel.CurrentPage = 1;
            }

            pagerViewModel.TotalPages = Math.Ceiling((double)total_posts / 10);

            if (pagerViewModel.CurrentPage > 1 )
            {
                pagerViewModel.p_visible = true;
            }

            if (pagerViewModel.CurrentPage < pagerViewModel.TotalPages)
            {
                pagerViewModel.n_visible = true;
            }

            pagerViewModel.p = string.Format("p={0}", pagerViewModel.CurrentPage - 1);
            pagerViewModel.n = string.Format("p={0}", pagerViewModel.CurrentPage + 1);

            if (RouteData.Values["action"].ToString().Equals("search", StringComparison.OrdinalIgnoreCase))
            {
                var s = string.Format("?s={0}", queryStrings["s"]);
                pagerViewModel.p = string.Format("{0}&{1}", s, pagerViewModel.p);
                pagerViewModel.n = string.Format("{0}&{1}", s, pagerViewModel.n);
            }
            else if(RouteData.Values["action"].ToString().Equals("category", StringComparison.OrdinalIgnoreCase))
            {
                var c = string.Format("?category={0}", queryStrings["category"]);
                pagerViewModel.p = string.Format("{0}&{1}", c, pagerViewModel.p);
                pagerViewModel.n = string.Format("{0}&{1}", c, pagerViewModel.n);
            }
            else if (RouteData.Values["action"].ToString().Equals("tag", StringComparison.OrdinalIgnoreCase))
            {
                var t = string.Format("?tag={0}", queryStrings["tag"]);
                pagerViewModel.p = string.Format("{0}&{1}", t, pagerViewModel.p);
                pagerViewModel.n = string.Format("{0}&{1}", t, pagerViewModel.n);
            }
            else
            {
                pagerViewModel.p = string.Concat("?", pagerViewModel.p);
                pagerViewModel.n = string.Concat("?", pagerViewModel.n);
            }

            return View(pagerViewModel);
        }
    }
}
