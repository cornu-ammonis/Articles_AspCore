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

        public async Task<IViewComponentResult> InvokeAsync(int total_posts, int page_size)
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

            paginationViewModel.total_pages = Math.Ceiling((double)total_posts / page_size);

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

            string action = RouteData.Values["action"].ToString().ToLower();
            string query_concat;
            switch (action)
            {
                case "postsbyauthor":
                    query_concat = string.Format("?author={0}", queryStrings["author"]);
                    break;
                case "category":
                    query_concat = string.Format("?category={0}", queryStrings["category"]);
                    break;
                case "search":
                    query_concat = string.Format("?s={0}", queryStrings["s"]);
                    break;
                case "tag":
                    query_concat = string.Format("?tag={0}", queryStrings["tag"]);
                    break;
                default:
                    query_concat = null;
                    break;

            }
            if(query_concat == null)
            {
                paginationViewModel.previous_page = string.Concat("?", paginationViewModel.previous_page);
                paginationViewModel.next_page = string.Concat("?", paginationViewModel.next_page);
            }
            else
            {
                paginationViewModel.previous_page = string.Format("{0}&{1}", query_concat, paginationViewModel.previous_page);
                paginationViewModel.next_page = string.Format("{0}&{1}", query_concat, paginationViewModel.next_page);
            }
            

            return View(paginationViewModel);
        }
    }
}
