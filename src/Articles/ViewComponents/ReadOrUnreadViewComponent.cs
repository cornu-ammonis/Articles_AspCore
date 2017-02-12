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

                //does not render full contents if greater than certain length
                if(message.Contents.Length > 50)
                {
                    return View("ReadMessageHiddenContents", message);
                }
                else
                {
                    return View("ReadMessage", message);
                }
                
            }
            else
            {
                if(message.Contents.Length > 50)
                {
                    return View("UnreadMessageHiddenContents", message);
                }
                return View("UnreadMessage", message);
            }
        }

    }
}