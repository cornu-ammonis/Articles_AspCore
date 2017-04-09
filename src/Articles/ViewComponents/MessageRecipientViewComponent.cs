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

                //if the user has input a user they are not allowed the message, the model state 
                //will be invalid, and in that case we want to show recipient input 
                //so that they may change it.
                if(ViewData.ModelState.IsValid)
                {
                    return View("DontShowRecipientFormInput", viewModel);
                }
                else
                {
                    return View("ShowRecipientFormInput");
                }
                
            }
               
        }

    }
}