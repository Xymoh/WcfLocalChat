using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfChatAppc
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ChatService : IChatService
    {
        private ChatEngine chatEngine = new ChatEngine();

        /// <summary>
        /// Metoda która łączy użytkownika do czatu
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ChatUser ClientConnect(string userName)
        {
            return chatEngine.AddNewChatUser(new ChatUser() { UserName = userName });
        }

        public List<ChatUser> GetAllUsers()
        {
            return chatEngine.ConnectedUsers;
        }

        public List<ChatMessage> GetNewMessages(ChatUser user)
        {
            return chatEngine.GetNewMessages(user);
        }

        public void RemoveUser(ChatUser user)
        {
            chatEngine.RemoveUser(user);
        }

        public void SendNewMessage(ChatMessage newMessage)
        {
            chatEngine.AddNewMessage(newMessage);
        }
    }
}
