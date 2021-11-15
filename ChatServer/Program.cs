using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using System.Security.Principal;
using System.Windows.Forms;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Program obj = new Program();

            if (obj.IsCurrentlyRunningAsAdmin())
            {
                obj.Run();
            }
            else
            {
                MessageBox.Show("Proszę aktywuj program jako administrator.", "ALERT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Run()
        {
            using (ServiceHost host = new ServiceHost(typeof(WcfChatAppc.ChatService)))
            {
                host.Open();

                Console.WriteLine("Inicjalizacja serwera");
                Console.WriteLine("Naciśnij dowolny klawisz aby zamknąć ...");
                Console.ReadLine();

                host.Close();
            }
        }

        private bool IsCurrentlyRunningAsAdmin()
        {
            bool isAdmin = false;
            WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
            if (currentIdentity != null)
            {
                WindowsPrincipal principal = new WindowsPrincipal(currentIdentity);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
                principal = null;
            }
            return isAdmin;
        }
    }
}
