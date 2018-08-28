using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;


namespace importBGG
{
    /// <summary>
    /// Logique d'interaction pour BggLogin.xaml
    /// </summary>
    public partial class BggLogin : Window
    {
        List<BoardGamePicker.BoardGame> BoardGamesList = new List<BoardGamePicker.BoardGame>();

        public BggLogin(List<BoardGamePicker.BoardGame> BgList)
        {
            InitializeComponent();
            BoardGamesList = BgList;
        }

        
        public bool closedOnOkButton;
        private bool IsWaiting;
        public bool IsAllGood;


        private void logginOkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsAllGood)
            {
                if (usernameInput.Text != string.Empty)
                {
                    UserName = usernameInput.Text;

                    //this.messageTb.Text = "Connexion à BGG, veuillez patienter, cela peut prendre plusieurs minutes...";
                    System.ComponentModel.BackgroundWorker waitingWorker = new System.ComponentModel.BackgroundWorker();
                    waitingWorker.DoWork += WaitingWork;
                    waitingWorker.RunWorkerCompleted += WaitingCompleted;
                    waitingWorker.RunWorkerAsync();
                    //WaitingMessage();

                    System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
                    worker.DoWork += DoWork;
                    worker.RunWorkerCompleted += WorkerCompleted;
                    worker.RunWorkerAsync();

                    IsAllGood = true;

                }
                else
                {
                    messageTb.Text = "OUPS, veuillez rentrer un nom d'utilisateur";
                }
            }

            else
            {
                closedOnOkButton = true;
                Close();
            }

        }
       
    }
}
