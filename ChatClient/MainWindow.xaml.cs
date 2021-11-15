using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.ServiceModel;
using WcfChatAppc;
using System.Windows.Threading;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            lblStatus.Text = "Nie połączony";

            dispatcherTimer.Tick += new EventHandler(userListTimer_Tick);
            dispatcherTimer.Tick += new EventHandler(messagesTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        }

        private ChannelFactory<IChatService> channelFactory;
        private IChatService remoteProxy;
        private ChatUser clientUser;
        private bool isConnected = false;

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblStatus.Text = "Łączenie...";
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();

                if (!String.IsNullOrEmpty(loginWindow.UserName))
                {
                    channelFactory = new ChannelFactory<IChatService>("ChatConfig");
                    remoteProxy = channelFactory.CreateChannel();
                    clientUser = remoteProxy.ClientConnect(loginWindow.UserName);

                    if (clientUser != null)
                    {
                        lstUsers.IsEnabled = true;
                        loginButton.IsEnabled = false;
                        msgButton.IsEnabled = true;
                        txtMessage.IsEnabled = true;
                        isConnected = true;
                        lblStatus.Text = "Podłączony jako: " + clientUser.UserName;
                        closeButton.Content = "Wyloguj";
                        dispatcherTimer.Start();
                    }
                }
                else
                {
                    lblStatus.Text = "Rozłączony";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd\nUżytkownik nie mógł się połączyć\nWiadomość: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void userListTimer_Tick(object sender, EventArgs e)
        {
            lstUsers.Items.Clear();
            List<ChatUser> listUsers = remoteProxy.GetAllUsers();
            foreach (var user in listUsers)
            {
                lstUsers.Items.Add(user.ToString());
            }
        }

        private void messagesTimer_Tick(object sender, EventArgs e)
        {
            List<ChatMessage> messages = remoteProxy.GetNewMessages(clientUser);

            if (messages != null)
            {
                foreach (var message in messages)
                {
                    InsertMessage(message);
                }
            }
        }

        private void InsertMessage(ChatMessage message)
        {
            txtChat.AppendText("\n" + message.User.UserName + " napisał (" + message.Date + "):\n" + message.Message + "\n");
        }

        private void msgButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtMessage.Text))
            {
                ChatMessage newMessage = new ChatMessage()
                {
                    Date = DateTime.Now,
                    Message = txtMessage.Text,
                    User = clientUser
                };
                remoteProxy.SendNewMessage(newMessage);
                InsertMessage(newMessage);
                txtMessage.Text = String.Empty;
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                remoteProxy.SendNewMessage(new ChatMessage()
                {
                    Date = DateTime.Now,
                    Message = "<<Użytkownik Wylogował się>>",
                    User = clientUser
                });
                remoteProxy.RemoveUser(clientUser);

                lstUsers.IsEnabled = false;
                loginButton.IsEnabled = true;
                msgButton.IsEnabled = false;
                txtMessage.IsEnabled = false;
                isConnected = false;
                lblStatus.Text = "Nie połączony";
                closeButton.Content = "Wyjdź";
            }
            else
            {
                this.Close();
            }
        }
    }
}
