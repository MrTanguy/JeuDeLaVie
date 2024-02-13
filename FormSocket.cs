using System;
using System.Threading;
using System.Windows.Forms;

namespace JeuDeLaVie
{
    public partial class FormSocket : Form
    {
        private Client clientSocket;
        // private System.Windows.Forms.Timer timer;
        private Game game;
        private System.Windows.Forms.Timer timer;
        private int currentStep;

        public FormSocket()
        {
            InitializeComponent();
            clientSocket = new Client("127.0.0.1", 9999);
            clientSocket.StartListening(AfficherMessage);
            buttonSend.Click += buttonSend_Click;
            buttonStart.Click += buttonStart_Click;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string messageToSend = richTextBoxInput.Text;
            clientSocket.Send(messageToSend);
            richTextBoxInput.Clear();
        }

        private void AfficherMessage(string message)
        {
            if (richTextBoxOutput.InvokeRequired)
            {
                richTextBoxOutput.Invoke((Action)(() => AfficherMessage(message)));
            }
            else
            {
                richTextBoxOutput.AppendText(message + Environment.NewLine);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Effectuer une étape du jeu de la vie
            int[,] newState = game.NewState();

            // Afficher le jeu
            AfficherJeuDeLaVie(newState);

            // Incrémenter le compteur d'étapes
            currentStep++;

            // Arrêter le timer après 10 étapes
            if (currentStep >= 10)
            {
                timer.Stop();
            }
        }

        private void AfficherJeuDeLaVie(int[,] games)
        {
            string jeu = "";

            for (int i = 0; i < games.GetLength(0); i++)
            {
                for (int j = 0; j < games.GetLength(1); j++)
                {
                    jeu += games[i, j];
                }
                jeu += "\n";
            }
            richTextBoxGameOfLife.Text = jeu;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            game = new Game(x: 20, y: 40);

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // Interval en millisecondes entre chaque étape
            timer.Tick += Timer_Tick;

            currentStep = 0; // Réinitialiser le compteur d'étapes
            richTextBoxGameOfLife.Clear(); // Effacer la zone de texte

            timer.Start(); // Démarrer le timer
        }
    }
}
