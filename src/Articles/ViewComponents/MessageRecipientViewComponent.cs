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
    public class MessageRecipientViewComponent : ViewComponent
    {
        IMessageRepository _messageRepository;

        public MessageRecipientViewComponent(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(MessageCreationViewModel viewModel)
        {
         
            if(viewModel.RecipientName == null)
            {
                return View("ShowRecipientFormInput");
            }
            else
            {
                return View("DontShowRecipientFormInput", viewModel);
            }
               
        }

    }
}