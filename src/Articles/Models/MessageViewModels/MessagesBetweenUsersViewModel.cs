using Articles.Models.Core;

namespace Articles.Models.MessageViewModels
{
    public class MessagesBetweenUsersViewModel : MessageListViewModel
    {
        public MessagesBetweenUsersViewModel(IMessageRepository _messageRepository, string user_name1, string user_name2)
        {
            populateMessageList(_messageRepository.RetrieveMessagesBetweenUsers(user_name1, user_name2));
        }
    }
}
