using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChatClient
{
    /// <summary>
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        public string UserName { get; set; }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(userNameTxt.Text))
            {
                this.UserName = userNameTxt.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("Proszę wprowadź nazwę użytkownika", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void extButton_Click(object sender, RoutedEventArgs e)
        {
            this.UserName = String.Empty;
            this.Close();
        }
    }
}
