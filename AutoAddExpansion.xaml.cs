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
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

namespace BoardGamePicker
{
    /// <summary>
    /// Logique d'interaction pour AutoAddExpansion.xaml
    /// </summary>
    public partial class AutoAddExpansion : Window
    {
        public AutoAddExpansion()
        {
            InitializeComponent();
        }

        public bool isClosedOnYesButton { get; private set; }
        public bool isClosedOnNoButton { get; private set; }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            isClosedOnYesButton = true;
            isClosedOnNoButton = false;
            Close();
        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            isClosedOnNoButton = true;
            isClosedOnYesButton=false;
            Close();
        }
    }
}
