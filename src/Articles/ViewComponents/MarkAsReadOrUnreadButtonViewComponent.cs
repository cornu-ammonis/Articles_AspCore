using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Articles.Models;
using Articles.Models.Core;

namespace Articles.ViewComponents
{
    public class MarkAsReadOrUnreadButton : ViewComponent
    {
        IMessageRepository _messageRepository;

        public MarkAsReadOrUnreadButton(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(int messageId)
        {
            bool isRead = await _messageRepository.CheckIfReadAsync(messageId);

            if(isRead)
            {
                return View("MarkUnreadButton", messageId);
            }
            else
            {
                return View("MarkReadButton", messageId);
            }
        }

    }
}