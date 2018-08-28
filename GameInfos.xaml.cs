using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

namespace BoardGamePicker
{
    /// <summary>
    /// Logique d'interaction pour GameInfos.xaml
    /// </summary>
    public partial class GameInfos : Window
    {
        List<Style> localStyleList = new List<Style>();
        List<Editor> localEditorsList = new List<Editor>();
        
        public GameInfos(List<Editor> editorsList, List<Style> styleList )
        {
            InitializeComponent();
            localEditorsList = editorsList;
            localStyleList = styleList;
            editorComboBox.ItemsSource = localEditorsList.OrderBy(o => o.Name);
            styleComboBox.ItemsSource = localStyleList.OrderBy(o => o.style);
            
        }

        public GameInfos()
        {
            InitializeComponent();
            
        }

        public bool isAllGood { get; set; }
        public int newMinPl { get; set; }
        public int newMaxPl { get; set; }
        public int newMinDur { get; set; }
        public int newMaxDur { get; set; }
        public BoardGame selectedGame { get; set; }
        public Expansion selectedExpansion { get; set; }
        public List<Editor> newEditorsList = new List<Editor>();
        public List<Style> newStyleList = new List<Style>();

        private void deleteEditor_Click(object sender, RoutedEventArgs e)
        {
            Editor selectedEditor = (Editor)(sender as System.Windows.Controls.Button).DataContext;
            ConfirmWindow confirmWindow = new ConfirmWindow();
            confirmWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var result = confirmWindow.ShowDialog();
            if(result == DialogResult.HasValue)
            {
                if(confirmWindow.isOK)
                {
                    selectedGame.localEditorList.Remove(selectedEditor);
                    selectedGame.refreshEditorListToString();
                    EditorsList.Items.Refresh();
                }
            }
        }

        public int i;
        private void Ok_Button_Click(object sender, RoutedEventArgs e)
        {
            isAllGood = true;
            if (Title.Text != string.Empty)
            {
                selectedGame.title = Title.Text.ToUpper();
            }
            else
            {
                NotOk(Title);
            }

            if(minPlayers.Text != string.Empty)
            {

                if(checkForParse(minPlayers,out i))
                {
                    selectedGame.minPlayers = i;
                }
            }
            else
            {
                NotOk(minPlayers);
            }

            if(MaxPlayers.Text!= string.Empty)
            {
                if(checkForParse(MaxPlayers,out i))
                {
                    selectedGame.maxPlayers = i;
                }
            }
            else
            {
                NotOk(MaxPlayers);
            }

            if(durationMin.Text != string.Empty)
            {
                if(checkForParse(durationMin, out i))
                {
                    selectedGame.durationMin = i;
                }
            }
            else { NotOk(durationMin); }

            if(durationMax.Text != string.Empty)
            {
                if(checkForParse(durationMax,out i))
                {
                    selectedGame.durationMax = i;
                }
            }
            else { NotOk(durationMax); }

            //NewEditors();
            //NewStyles();
            //rajouter nv styles

            if (isAllGood)
            {
                Close();
            }
        }

        private void Title_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Background != Brushes.White)
             {
            tb.Background = Brushes.White;
            }
        }

        public bool checkForParse(System.Windows.Controls.TextBox tb,out int value)
        {
            int parsed;
            bool result = Int32.TryParse(tb.Text, out parsed);
            if (result)
            {
                
               value = parsed;
                return true;
            }
            else
            {
                value = 0;
                notANumber(tb);
                return false;
            }
        }
        private void notANumber(System.Windows.Controls.TextBox tb)
        {
            isAllGood = false;
            tb.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFB, 0xCF, 0xE9));
            //tb.Text += " n'est pas un nombre, veuillez rentrer un nombre.";
        }
        private void NotOk(TextBox tb)
        {
            isAllGood = false;
            tb.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFB, 0xCF, 0xE9));
        }

        //private void NewEditors()
        //{
        //    if (editorAddInput.Text != string.Empty) 
        //    {
        //        List<string> splitList = editorAddInput.Text.ToUpper().Split(';', ',').Select(p => p.Trim()).ToList();
        //        foreach(string editorName in splitList)
        //        {
        //            bool alreadyExists = false;
        //            for (int i = 0; i< localEditorsList.Count; i++)
        //            {
        //                if (localEditorsList[i].Name == editorName)
        //                {
        //                    alreadyExists = true;
        //                    selectedGame.localEditorList.Add(localEditorsList[i]);
        //                    selectedGame.editorListToString += editorName + " ; ";
        //                    break;
        //                }
        //            }
        //            if(!alreadyExists)
        //            {
        //                Editor newEditor = new Editor();
        //                newEditor.Name = editorName;
        //                newEditorsList.Add(newEditor);
        //                selectedGame.localEditorList.Add(newEditor);
        //                selectedGame.editorListToString += editorName + " ; ";
        //            }
        //        }
        //    }
        //}

        private void deleteStyle_Click(object sender, RoutedEventArgs e)
        {
            Style selectedStyle = (Style)(sender as System.Windows.Controls.Button).DataContext;
            ConfirmWindow confirmWindow = new ConfirmWindow();
            confirmWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var result = confirmWindow.ShowDialog();
            if (result == DialogResult.HasValue)
            {
                if (confirmWindow.isOK)
                {
                    selectedGame.style.Remove(selectedStyle);
                    selectedGame.refreshStyleListToString();
                    StyleList.Items.Refresh();
                }
            }
        }

        //private void NewStyles()
        //{
        //    if (styleAddInput.Text != string.Empty)
        //    {
        //        List<string> splitList = styleAddInput.Text.ToUpper().Split(';', ',').Select(p => p.Trim()).ToList();
        //        foreach (string styl in splitList)
        //        {
        //            bool alreadyExists = false;
        //            for (int i = 0; i < localStyleList.Count; i++)
        //            {
        //                if (localStyleList[i].style == styl)
        //                {
        //                    alreadyExists = true;
        //                    selectedGame.style.Add(localStyleList[i]);
        //                    selectedGame.styleListToString += styl + " ; ";
        //                    break;
        //                }
        //            }
        //            if (!alreadyExists)
        //            {
        //                Style newStyle = new Style();
        //                newStyle.style = styl;
        //                newStyleList.Add(newStyle);
        //                selectedGame.style.Add(newStyle);
        //                selectedGame.styleListToString += styl + " ; ";
        //            }
        //        }
        //    }
        //}

        private void deleteExpansion_Click(object sender, RoutedEventArgs e)
        {
            ConfirmWindow confirm = new ConfirmWindow();
            confirm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var result = confirm.ShowDialog();
            if(result == DialogResult.HasValue)
            {
                if(confirm.isOK)
                {
                    Expansion selectedExpansion = (Expansion)(sender as Button).DataContext;
                    selectedGame.removeExpansion(selectedExpansion);
                }
            }
            ExpansionList.Items.Refresh();
        }

        private void modifyExpansion_Click(object sender, RoutedEventArgs e)
        {
            Expansion selectedExpansion = (Expansion)(sender as Button).DataContext;
            GameInfos InfoWindow = new GameInfos();
            InfoWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InfoWindow.Height = 300;
            InfoWindow.Ok_Button.Visibility = Visibility.Collapsed;
            InfoWindow.Ok_Button_Expansion.Visibility = Visibility.Visible;
            InfoWindow.Row5Grid.Visibility = Visibility.Collapsed;
            InfoWindow.durationTextBlock.Visibility = Visibility.Collapsed;
            InfoWindow.durationGrid.Visibility = Visibility.Collapsed;
            InfoWindow.selectedExpansion = selectedExpansion;
            InfoWindow.MaxPlayers.Text = selectedExpansion.maxPlayers.ToString();
            InfoWindow.minPlayers.Text = selectedExpansion.minPlayers.ToString();
            InfoWindow.Title.Text = selectedExpansion.title;

            InfoWindow.ShowDialog();
            ExpansionList.Items.Refresh();


        }

        private void Ok_Button_Expansion_Click(object sender, RoutedEventArgs e)
        {
            isAllGood = true;
            if (Title.Text != string.Empty)
            {
                selectedExpansion.title = Title.Text.ToUpper();
            }
            else
            {
                NotOk(Title);
            }
            if (minPlayers.Text != string.Empty)
            {

                if(checkForParse(minPlayers, out i))
                {
                    selectedExpansion.minPlayers = i;
                }
            }
            else
            {
                NotOk(minPlayers);
            }

            if (MaxPlayers.Text != string.Empty)
            {
                if(checkForParse(MaxPlayers, out i))
                {
                    selectedExpansion.maxPlayers = i;
                }
            }
            else
            {
                NotOk(MaxPlayers);
            }
            if (isAllGood)
            {
                Close();
            }
        }

        private void editorAddInput_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {


            string lastInput;
            //List<string> inputList = editorAddInput.Text.ToUpper().Split(',', ';').ToList();
            List<string> inputList = editorComboBox.Text.ToUpper().Split(',', ';').ToList();
            if (inputList.Count != 0)
            {
                lastInput = inputList[inputList.Count - 1];
            }
            else
            {
                //lastInput = editorAddInput.Text;
                lastInput = editorComboBox.Text;
            }
            List<Editor> matchingEditorsList = new List<Editor>();
            //EditorPopupStackPanel.Children.Clear();
            //editorAutocompleteLb.ItemsSource = matchingEditorsList;

            foreach(Editor editor in localEditorsList)
            {
                if(editor.Name.StartsWith(lastInput))
                {
                    matchingEditorsList.Add(editor);
                }
            }

            if(matchingEditorsList.Count!=0)
            {
                //EditorPopup.Visibility = Visibility.Visible;
                //editorAutocompleteLb.ItemsSource = matchingEditorsList;
                //ContextMenuEditor.IsOpen = true;
                //ContextMenuEditor.StaysOpen = true;
                foreach(Editor editor in matchingEditorsList)
                {
                    //EditorPopupStackPanel.Children.Add(new TextBlock() { Text = editor.Name });
                    //editorAutocompleteLb.Items.Add(new TextBlock() { Text = editor.Name });
                    //editorCbListBox.Items.Add(editor);
                }
                editorComboBox.IsDropDownOpen = true;
                
                //editorCbListBox.ItemsSource = matchingEditorsList;
                //EditorPopup.StaysOpen = true;
                //EditorPopup.IsOpen = true;
                
                
               
            }
            else
            {
                //EditorPopup.Visibility = Visibility.Collapsed;
                //EditorPopup.IsOpen = false;
            }
        }

        private void addEditor_Click(object sender, RoutedEventArgs e)
        {
            string input = editorComboBox.Text.ToUpper().Trim();
            bool AlreadyExists = false;
            foreach(Editor editor in localEditorsList)
            {
                if(editor.Name==input)
                {
                    selectedGame.localEditorList.Add(editor);
                    selectedGame.editorListToString += editor.Name + " ; ";
                    AlreadyExists = true;
                    EditorsList.ItemsSource = selectedGame.localEditorList;
                    EditorsList.Items.Refresh();
                }
            }
            if(!AlreadyExists)
            {
                Editor editor = new Editor();
                editor.Name = input;
                selectedGame.localEditorList.Add(editor);
                selectedGame.editorListToString += editor.Name + " ; ";
                newEditorsList.Add(editor);
                EditorsList.ItemsSource = selectedGame.localEditorList;
                EditorsList.Items.Refresh();
            }
            editorComboBox.Text = string.Empty;
        }

        private void addStyle_Click(object sender, RoutedEventArgs e)
        {
            string input = styleComboBox.Text.ToUpper().Trim();
            bool AlreadyExists = false;

            foreach(Style style in localStyleList)
            {
                if (style.style == input)
                {
                    selectedGame.style.Add(style);
                    selectedGame.styleListToString += style.style + " ; ";
                    AlreadyExists = true;
                    StyleList.ItemsSource = selectedGame.style;
                    StyleList.Items.Refresh();
                }
            }
            if(!AlreadyExists)
            {
                Style style = new Style();
                style.style = input;
                selectedGame.style.Add(style);
                selectedGame.styleListToString += style.style + " ; ";
                newStyleList.Add(style);
                    StyleList.ItemsSource = selectedGame.style;
                StyleList.Items.Refresh();
            }
            styleComboBox.Text = string.Empty;
        }
    }
}
