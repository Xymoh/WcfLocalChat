using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfChatAppc
{
    public class ChatEngine
    {
        /// <summary>
        /// Przechowywanie podłączonych użytkowników
        /// </summary>
        private List<ChatUser> connectedUsers = new List<ChatUser>();

        /// <summary>
        /// Słownik do przechowywania wiadomości do które są do dostarczenia
        /// </summary>
        private Dictionary<string, List<ChatMessage>> incomingMessages = new Dictionary<string, List<ChatMessage>>();

        public List<ChatUser> ConnectedUsers
        {
            get { return connectedUsers; }
            set { connectedUsers = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public ChatUser AddNewChatUser(ChatUser newUser)
        {
            var exists =
                from ChatUser e in ConnectedUsers
                where e.UserName == newUser.UserName
                select e;

            if (exists.Count() == 0)
            {
                ConnectedUsers.Add(newUser);
                incomingMessages.Add(newUser.UserName, new List<ChatMessage>()
                {
                    new ChatMessage() { User = newUser, Date = DateTime.Now, Message = "Witaj w Chatroomie!" }
                });

                Console.WriteLine("\nNowy użytkownik dołączył do czatu: " + newUser.UserName);
                return newUser;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newMessage"></param>
        public void AddNewMessage(ChatMessage newMessage)
        {
            Console.Write(newMessage.User.UserName + " says: " + newMessage.Message + " at " + newMessage.Date);

            foreach (var user in ConnectedUsers)
            {
                if (!newMessage.User.UserName.Equals(user.UserName))
                {
                    incomingMessages[user.UserName].Add(newMessage);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<ChatMessage> GetNewMessages(ChatUser user)
        {
            List<ChatMessage> myNewMessages = incomingMessages[user.UserName];
            incomingMessages[user.UserName] = new List<ChatMessage>();

            if (myNewMessages.Count > 0)
            {
                return myNewMessages;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void RemoveUser(ChatUser user)
        {
            ConnectedUsers.RemoveAll(u => u.UserName == user.UserName);
        }
    }
}
