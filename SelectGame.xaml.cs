using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

namespace BoardGamePicker
{
    /// <summary>
    /// Logique d'interaction pour SelectGame.xaml
    /// </summary>
    [Serializable]
    public partial class SelectGame : Window
    {

        string title2 { get; set; }

        public SelectGame(string title)
        {
            InitializeComponent();

        }

        public bool ClosedOnOk { get; private set; }
        public string title { get; set; }

        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            ClosedOnOk = false;
            //ça
            //this.title2= ((ListBoxItem)TextBoxList.SelectedItem).Content.ToString();
            //ou ça?
            if(TextBoxList.SelectedIndex == -1)
            {
                Indications.Text = "OUPS, veuillez sélectionner un jeu, merci";
            }
            else
            {

            
            this.title = TextBoxList.SelectedItem.ToString();
            //TextBoxList.ItemsSource = BoardGamesList.ToString()
            //return TextBoxList.SelectedItem;
            this.DialogResult = DialogResult.HasValue;
                ClosedOnOk = true;
            this.Close();
                }
        }
    }
}
