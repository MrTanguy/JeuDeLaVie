using System;
using System.Threading;
using System.Windows.Forms;

namespace JeuDeLaVie
{
    internal static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Création du serveur Socket
            Thread serverThread = new Thread(() =>
            {
                SocketServeur server = new SocketServeur(9999);
                server.Start();
            });
            serverThread.Start();

            // Attendez quelques secondes pour vous assurer que le serveur est démarré
            Thread.Sleep(2000);

            // Ouvrir le formulaire FormSocket
            Application.Run(new FormSocket());
        }
    }
}
