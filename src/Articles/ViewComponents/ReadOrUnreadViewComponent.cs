using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Articles.Models;
using Articles.Models.Core;

namespace Articles.ViewComponents
{
    public class ReadOrUnread : ViewComponent
    {

        public ReadOrUnread()
        {
          
        }
        public async Task<IViewComponentResult> InvokeAsync(Message message)
        {

            if (message.Read)
            {
                return View("MarkUnreadButton", message);
            }
            else
            {
                return View("MarkReadButton", message);
            }
        }

    }
}