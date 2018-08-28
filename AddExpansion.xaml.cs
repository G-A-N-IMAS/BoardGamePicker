using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

namespace BoardGamePicker
{
    /// <summary>
    /// Logique d'interaction pour AddExpansion.xaml
    /// </summary>
    public partial class AddExpansion : Window
    {
        public AddExpansion(string gameTitle, int Playermin, int PlayerMax)
        {

            InitializeComponent();

            GameTitleDisplay.Text = gameTitle;
            minPlayerOriginalGame.Text = Playermin.ToString();
            maxPlayerOriginalGame.Text = PlayerMax.ToString();
        }

        public string title;
        public int min { get; set; }
        public int max { get; set; }
        public bool changesPlayerNb;
        public bool isAllGood = true;
        public bool OkbtnClicked = false;

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OkExpansion_Click(object sender, RoutedEventArgs e)
        {
            // tryparse
            isAllGood = true;
            if (titleInput.Text != string.Empty)
            {
                title = titleInput.Text.ToUpper();
            }
            else
            {
                titleInput.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFB, 0xCF, 0xE9));
                isAllGood = false;
            }
            int value;
            bool result = Int32.TryParse(minInput.Text, out value);
            if (result)
            {
                min = value;
            }
            else
            {
                
                notANumber(minInput);
            }
            result = Int32.TryParse(maxInput.Text, out value);
            if (result)
            {
                max = value;
            }
            else
            {

                notANumber(maxInput);
            }
            if(isAllGood)
            {
                OkbtnClicked = true;
                DialogResult = DialogResult.HasValue;
                Close();
            }
            else
            {

            }
        }

        private void notANumber(System.Windows.Controls.TextBox tb)
        {
            isAllGood = false;
            tb.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFB, 0xCF, 0xE9));

        }

        private void Yes_Checked(object sender, RoutedEventArgs e)
        {
            changesPlayerNb = true;
            newPlayerCount.Visibility = Visibility.Visible;
        }

        private void No_Checked(object sender, RoutedEventArgs e)
        {
            changesPlayerNb = false;
            newPlayerCount.Visibility = Visibility.Collapsed;
        }

        private void titleInput_GotFocus(object sender, RoutedEventArgs e)
        {
            titleInput.Background = new SolidColorBrush(Colors.White);
        }

        private void minInput_GotFocus(object sender, RoutedEventArgs e)
        {
            minInput.Text = string.Empty;
            minInput.Background = new SolidColorBrush(Colors.White);
        }

        private void maxInput_GotFocus(object sender, RoutedEventArgs e)
        {
            maxInput.Text = string.Empty;
            maxInput.Background = new SolidColorBrush(Colors.White);
        }
    }
}
