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
            PagerViewModel pvm = new PagerViewModel();
            pvm.n_visible = false;
            pvm.p_visible = false;
            var queryStrings = Request.Query;
            var keys = queryStrings.Keys;
            if (keys.Contains("p"))
            {
                pvm.CurrentPage = int.Parse(queryStrings["p"]);
            }
            else
            {
                pvm.CurrentPage = 1;
            }

            pvm.TotalPages = Math.Ceiling((double)total_posts / 10);

            if (pvm.CurrentPage > 1 )
            {
                pvm.p_visible = true;
            }

            if (pvm.CurrentPage < pvm.TotalPages)
            {
                pvm.n_visible = true;
            }

            pvm.p = string.Format("p={0}", pvm.CurrentPage - 1);
            pvm.n = string.Format("p={0}", pvm.CurrentPage + 1);

            if (RouteData.Values["action"].ToString().Equals("search", StringComparison.OrdinalIgnoreCase))
            {
                var s = string.Format("?s={0}", queryStrings["s"]);
                pvm.p = string.Format("{0}&{1}", s, pvm.p);
                pvm.n = string.Format("{0}&{1}", s, pvm.n);
            }
            else if(RouteData.Values["action"].ToString().Equals("category", StringComparison.OrdinalIgnoreCase))
            {
                var c = string.Format("?category={0}", queryStrings["category"]);
                pvm.p = string.Format("{0}&{1}", c, pvm.p);
                pvm.n = string.Format("{0}&{1}", c, pvm.n);
            }
            else
            {
                pvm.p = string.Concat("?", pvm.p);
                pvm.n = string.Concat("?", pvm.n);
            }

            return View(pvm);
        }
    }
}
