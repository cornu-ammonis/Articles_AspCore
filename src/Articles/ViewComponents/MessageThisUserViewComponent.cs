using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Articles.Models;
using Articles.Models.Core;
using Articles.Models.MessageViewModels;

namespace Articles.ViewComponents
{
    public class MessageThisUserViewComponent : ViewComponent
    {
        IMessageRepository _messageRepository;

        public MessageThisUserViewComponent(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(string potentialRecipientName)
        {
         
            if (await _messageRepository.CanMessageAsync(User.Identity.Name, potentialRecipientName))
            {
                return View("MessageThisUser", potentialRecipientName);
            }
            else
            {
                return View("Empty");
            }
               
        }

    }
}