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
    /// Logique d'interaction pour ConfirmWindow.xaml
    /// </summary>
    public partial class ConfirmWindow : Window
    {
        public ConfirmWindow()
        {
            InitializeComponent();
        }
        public bool isOK { get; set; }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            isOK = true;
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            isOK = false;
            Close();
        }
    }
}
