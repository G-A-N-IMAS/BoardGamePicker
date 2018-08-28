using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
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
    /// Logique d'interaction pour AddGame.xaml
    /// </summary>
    [Serializable]
    public partial class AddGame : Window
    {
        List<Editor> editorsList = new List<Editor>();
        List<Style> styleList = new List<Style>();
         public AddGame()
        { }

        public AddGame(List<Editor> importeditorsList, List<Style> importstyleList)
        {
            InitializeComponent();
            editorsList = importeditorsList;
            styleList = importstyleList;

            editorListDisplay.ItemsSource = editorsList;
            styleListDisplay.ItemsSource = styleList;

        }

        public string gameTitle { get; set; }
        public int minPlayer;
        public int maxPlayers;
        public int durationMin;
        public int durationMax;
        public bool isAllGood = true;


        private void Toggle()
        {
            if (Page1.Visibility == Visibility.Visible)
            {
                Page1.Visibility = Visibility.Collapsed;
                Page2.Visibility = Visibility.Visible;
            }
            else
            {
                Page2.Visibility = Visibility.Collapsed;
                Page1.Visibility = Visibility.Visible;
            }
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            isAllGood = true;
            if(gameTitle!= string.Empty)
            {
                gameTitle = this.titleAnswer.Text;
            }
            else
            {
                isAllGood = false;
                titleAnswer.Background =  new SolidColorBrush(Color.FromArgb(0xFF, 0xFB, 0xCF, 0xE9));
                titleAnswer.Text = "veuillez entrer un titre";
            }
            checkForParse(out minPlayer, minPLayersAnswer);
            checkForParse(out maxPlayers, maxPLayersAnswer);
            checkForParse(out durationMin, durationMinAnswer);
            checkForParse(out durationMax, durationMaxAnswer);

            if (isAllGood)
            {
                Toggle();
            }
        }

        public void checkForParse(out int value, TextBox tb)
            {
            //int parsed;
            bool result = Int32.TryParse(tb.Text, out value);
            if (result)
            {
               // value = parsed;
            }
            else
            {
                notANumber(tb);
            }
        }
        private void notANumber(TextBox tb)
        {
            isAllGood = false;
            tb.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFB, 0xCF, 0xE9));
            tb.Text += " n'est pas un nombre, veuillez rentrer un nombre.";
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.Background = new SolidColorBrush(Colors.White);
            //tb.GotFocus -= TextBox_GotFocus;
            
        }

        private void BackToP1_Click(object sender, RoutedEventArgs e)
        {
            Toggle();
        }

        public List<Editor> selectedEditor = new List<Editor>();
        public List<Style> selectedStyles = new List<Style>();
        public List<Editor> newEditors = new List<Editor>();
        private List<string> newEditorString;
        public List<Style> newStyles = new List<Style>();
        private List<string> newStylesString;
        public bool closedOnOkButtonClick { get; set; }


        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            AddEditor();
            AddStyle();
            this.DialogResult = DialogResult.HasValue;
            closedOnOkButtonClick = true;
            Close();
        }

        private void AddEditor()
        {


            if (editorListDisplay.SelectedItems.Count != 0)
            {
                foreach(Editor editor in editorListDisplay.SelectedItems)
                {
                    selectedEditor.Add(editor);
                }

               // selectedEditor.Add(editorListDisplay.SelectedItems as Editor);
            }
            if (editorManualInput.Text != string.Empty )
            {
                List<string> splitList = editorManualInput.Text.ToUpper().Split(';', ',').Select(p => p.Trim()).ToList();

                List<string> editToString = new List<string>();
                foreach(Editor editor in editorsList)
                {
                    editToString.Add(editor.Name);
                    if(splitList.Contains(editor.Name))
                    {
                        selectedEditor.Add(editor);
                    }
                }

                newEditorString = splitList.Except(editToString).ToList();
                foreach(string editor in newEditorString)
                {
                    Editor edit = new Editor();
                    edit.Name = editor;
                    newEditors.Add(edit);

                }
                selectedEditor.AddRange(newEditors);

            }
        }
        private void AddStyle()
        {

            if (styleListDisplay.SelectedItems.Count != 0)
            {
                foreach(Style style in styleListDisplay.SelectedItems)
                {
                    selectedStyles.Add(style);
                }
                //selectedStyles.Add(styleListDisplay.SelectedItems as Style);
            }
            if (styleManualInput.Text != string.Empty)
            {
                List<string> splitList = styleManualInput.Text.ToUpper().Split(';', ',').Select(p => p.Trim()).ToList();
                List<string> styleToString = new List<string>();
                foreach(Style style in styleList)
                {
                    styleToString.Add(style.style);
                    if(splitList.Contains(style.style))
                    {
                        selectedStyles.Add(style);
                    }
                }
                newStylesString = splitList.Except(styleToString).ToList();
                foreach(string style in newStylesString)
                {
                    Style styl = new Style();
                    styl.style = style;
                    newStyles.Add(styl);
                }
                
                selectedStyles.AddRange(newStyles);
            }
        }

    }
}
