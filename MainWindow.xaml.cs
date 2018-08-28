using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using System.ComponentModel;
//using System.Windows.Forms;
//using Microsoft.WindowsAPICodePack.Dialogs;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
//using CsvHelper;


namespace BoardGamePicker
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    [Serializable]
    public partial class MainWindow : Window
    {
        static List<BoardGame> BoardGamesList = new List<BoardGame>();
        List<Style> styleList = new List<Style>();
        List<Editor> editorsList = new List<Editor>();
        static string path = AppDomain.CurrentDomain.BaseDirectory;//récupérer le path du fichier .exe
        string ListPath = path + @"\binLists\";//path du dossier pour stocker les 3 listes
        List<BoardGame> currentBgList = new List<BoardGame>();
        List<Style> currentStyleList = new List<Style>();
        List<Editor> currentEditorList = new List<Editor>();

        public MainWindow()
        {



            InitializeComponent();


            if(!Directory.Exists(ListPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(ListPath);
            }

            #region Démarrage Chargement 
            #region Chargement de la liste de jeux
            BoardGamesList = Load<List<BoardGame>>(@ListPath +"BoardGamesList.bin");//Tentative de chargement de la liste de jeux
            if (BoardGamesList == null)
            {
                BoardGamesList = new List<BoardGame>();//si elle n'existe pas, on en créé une nouvelle
            }
            #endregion

            #region Chargement de la liste de styles de jeux
            styleList = Load<List<Style>>(@ListPath + "styleList.bin");
            if (styleList == null)
            {
                styleList = new List<Style>();
            }
            #endregion

            #region Chargement de la liste des éditeurs
            editorsList = Load<List<Editor>>(@ListPath + "editorsList.bin");
            if (editorsList == null)
            {
                editorsList = new List<Editor>();
            }
            #endregion





            #endregion


        }
       
        public T Load<T>(string path) // méthode pour charger les différentes listes
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream flux = null;
            try
            {
                flux = new FileStream(path, FileMode.Open, FileAccess.Read);
                return (T)formatter.Deserialize(flux);
            }
            catch //(Exception ex)
            {
               // Error_Window errorWindow = new Error_Window();

                //errorWindow.errorWindow.Text=ex.ToString();
               // errorWindow.ShowDialog();
               return default(T);
            }
            finally
            {
                if (flux != null)
                    flux.Close();
            }
        }

        #region Différentes propriétés utilisées dans le code
        public bool isCollapsed = true; //état du volet latéral
        public bool MainDisplayHidden { get; set; }//état de la page principale
        public bool SecondaryDisplayHidden { get; set; }//état de la page secondaire
        //la page principale et la secondaire son en fait la même page, mais avec des <Grid> dont la visibilité est "Visible" ou "Collapsed"
        
            //public bool ThirdDisplayHidden = false; NE SERT PLUS?
        private bool isAllGood = true; //bool servant de sécurité pour ne pas continuer le code si il est sur false quand demandé
        public int userPlayerNbInput; 
        private bool FiltersDiplayed; //Pas implémenté, peut être plus tard
        Random random = new Random();
        ListCollectionView GameList = new ListCollectionView(BoardGamesList);
        //Ci-dessous les bool concernant l'état des 3 checkbox dans le volet "Plus d'options"
        public bool cbshortIsChecked { get; set; }
        public bool cbmediumIsChecked { get; set; }
        public bool cblongIsChecked { get; set; }
        #endregion





        public void ShowMainScren() //afficher la page principale
        {
            _3ColumnsGrid.Visibility = Visibility.Visible;
            MainDisplayHidden = false;
            List_Display_Grid.Visibility = Visibility.Collapsed;
            SecondaryDisplayHidden = true;
        }


        private void CollapseLeftGrid_Click(object sender, RoutedEventArgs e) //bouton pour ouvrir/fermer le volet latéral
        {
            if (isCollapsed)
            {
                CollapsableColumn.Width = new GridLength(1, GridUnitType.Star);
                Collapsed_Grid.Visibility = Visibility.Collapsed;
                ExpandedGrid.Visibility = Visibility.Visible;
                isCollapsed = false;
                CollapseLeftGrid.Content = "<";
            }
            else
            {
                CollapsableColumn.Width = new GridLength(0.2, GridUnitType.Star);
                ExpandedGrid.Visibility = Visibility.Collapsed;
                Collapsed_Grid.Visibility = Visibility.Visible;
                isCollapsed = true;
                CollapseLeftGrid.Content = ">";
            }
        }

        public void ToggleDisplay()//passer de la page principale à la page secondaire
        {
            if (!MainDisplayHidden)
            {
                //Les deux pages sont en fait sur la même page
                //c'est la propriété Visibility qui est utilisée
                //pour masquer l'une ou l'autre
                _3ColumnsGrid.Visibility = Visibility.Collapsed;
                List_Display_Grid.Visibility = Visibility.Visible;
                MainDisplayHidden = true;
                resultDisplayList.Focus();
            }
            else
            {
                List_Display_Grid.Visibility = Visibility.Collapsed;
                _3ColumnsGrid.Visibility = Visibility.Visible;
                MainDisplayHidden = false;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)//bouton retour en arrière
        {
            ToggleDisplay();
        }

        private void ShowBoardGameList_Click(object sender, RoutedEventArgs e)//bouton pour voir la liste de tous les jeux
        {
            currentBgList = BoardGamesList;
            if (BoardGamesList.Count == 0)//Si la liste est vide on prévient l'utilisateur
            {
                noGame();
            }
            else //Sinon on l'affiche
            {
                ToggleDisplay();

                ShowBoardGamesList();
            }
        }

        private void Matching_list_Click(object sender, RoutedEventArgs e)//Bouton page principale pour voir une liste en fonction du nombre de joueurs 
        {
            if(BoardGamesList.Count == 0)//Si la liste est vide on prévient l'utilisateur
            {
                noGame();
            }
            else
            { 
            isAllGood = true;
            ClearAll();//efface une liste préalablement affichée pour éviter les doublons
            checkForParse(out userPlayerNbInput, NbOfPlayers);//vérifie que l'utilisateur a bien entré un nombre
                //si ce n'est pas le cas, le bool isAllGood devient faux

                if (isAllGood)
                {
                    resultDisplayList.ItemsSource = null;//reset de la source de la liste
                    resultDisplayList.Items.Clear();//efface l'ancienne liste
                    MatchingListTextBox.Clear();//reset
                    ToggleDisplay();
                    
                    //création d'une nouvelle liste
                    ListBoxSelection listSelection = new ListBoxSelection(BoardGamesList,out currentBgList, resultDisplayList, userPlayerNbInput, cbshortIsChecked,cbmediumIsChecked,cblongIsChecked);
                    displayList();
                    //resultDisplayList.ItemsSource = currentBgList;
                    //setCurrentStyleList(out currentStyleList);
                    //setCurrentEditorList(out currentEditorList);
                    
                    MatchingListTextBox.Text = "Voici la liste des jeux auxquels vous pouvez jouer à " + userPlayerNbInput + " joueurs :";
                    displayGameCount.Text = "La liste contient " + resultDisplayList.Items.Count + " jeux";
                }
                }

        }

        public void checkForParse(out int value, System.Windows.Controls.TextBox tb) //vérifie
            //que l'entrée est bien un nombre
        {
            bool result = Int32.TryParse(tb.Text, out value);
            if (result)
            {
                //rien de spécial, on a déjà retourné la valeur avec le mot clé out
            }
            else
            {
                notANumber(tb);//affiche l'erreur de saisie
            }
        }
        private void notANumber(System.Windows.Controls.TextBox tb)//signale l'erreur visuellement
        {
            isAllGood = false;// pour que le code ne continue pas à la sortie
            tb.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFB, 0xCF, 0xE9));//couleur rouge
            tb.Text += " n'est pas un nombre, veuillez rentrer un nombre.";
        }


        private void AddNewGame_Click(object sender, RoutedEventArgs e)//bouton ajouter un jeu
        {
            AddGame addGamewindow = new AddGame(editorsList, styleList);//fenêtre pour ajouter un nvo jeu
            addGamewindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;//centrer la fenêtre
            var result = addGamewindow.ShowDialog();//ouvrir la fenêtre
            if (result == DialogResult.HasValue && addGamewindow.closedOnOkButtonClick == true)//la deuxième condition s'assure que le code ne continue pas si la fenêtre est fermée par la croix
            {
                //récupère toutes les données entrées par l'utilisateur
                string title = addGamewindow.gameTitle;
                int minPlayers = addGamewindow.minPlayer;
                int maxPlayers = addGamewindow.maxPlayers;
                int durationMin = addGamewindow.durationMin;
                int durationMax = addGamewindow.durationMax;
                List<Style> thisstyleList = addGamewindow.selectedStyles;
                List<Editor> thiseditorsList = addGamewindow.selectedEditor;
                //Ajoute aux listes générales les nouveautés
                styleList.AddRange(addGamewindow.newStyles);
                editorsList.AddRange(addGamewindow.newEditors);

                //Créé le nouveau jeu
                newGame(title, minPlayers, maxPlayers, durationMin, durationMax, thisstyleList, thiseditorsList);
            }

            SaveAll();

        }

        #region oldImportCsvCode
        //private void Load_Game_List_Click(object sender, RoutedEventArgs e)//bouton pour importer un .csv
        //{
        //    //créé une nouvelle fenêtre de navigation
        //    var dialog = new CommonOpenFileDialog();
        //    dialog.IsFolderPicker = false; //ça j'avoue je l'ai copié sans savoir pourquoi, mais ça fonctionne...
        //    dialog.Filters.Add(new CommonFileDialogFilter("*", ".csv"));//n'autorise que les .csv

        //    CommonFileDialogResult result = dialog.ShowDialog();//affiche la fenêtre
        //    string path = dialog.FileName;//récupère le chemin d'accès
        //    CsvLoad(path);//importe le fichier
        //}
        #endregion

           //nouvelle méthode pour importer directement depuis le site
        private void Load_Game_List_Click(object sender, RoutedEventArgs e)
        {
            importBGG.BggLogin BggSync = new importBGG.BggLogin(BoardGamesList);
            BggSync.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var result = BggSync.ShowDialog();
            if(result.HasValue && BggSync.closedOnOkButton)
            {
                //on récupère le tout
                BoardGamesList.AddRange(BggSync.importedList);
                MatchExpansion(BggSync.importedExpansionsList);//Count == 0 ????????????????
                SaveAll();
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)//rétablit le fond blanc quand on sélectionne la textbox après une erreur de Tryparse
        {
            System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;
            tb.Text = string.Empty;
            tb.Background = new SolidColorBrush(Colors.White);
        }


        public void ShowBoardGamesList()//affiche la liste de tous les jeux
        {           
            //créé la nouvelle liste, avec le constructor "général" qui ne prend pas de nombre de joueurs
            //prend tout de même l'état des checkboxes pour n'afficher qu'une partie des 
            //jeux selon ce qui a été coché ou non

                ListBoxSelection listBoxSelection = new ListBoxSelection(currentBgList, out currentBgList, resultDisplayList,cbshortIsChecked,cbmediumIsChecked,cblongIsChecked);
            displayList();
            //setCurrentStyleList(out currentStyleList);
            //    setCurrentEditorList(out currentEditorList);
                MatchingListTextBox.Text = "Voici la liste des jeux :";
                displayGameCount.Text = "La liste contient " + resultDisplayList.Items.Count + " jeux";
        }

        #region oldImportCsvCode
        ////Importer csv , trouvé ici: https://www.supinfo.com/articles/single/2066-travailler-avec-fichier-csv-c
        //public void CsvLoad(string Path)//importe les nouveaux jeux depuis le .csv
        //{

        //    List<Importcsv> importedList = ReadFile<Importcsv>(Path);//lit le fichier trouvé et le map sur une liste d'objets "ImportCsv"
        //    for (int i = 0; i < importedList.Count; i++)
        //    {
        //        if (importedList[i].itemtype != "expansion")//si jeu de base
        //        {
        //            //alors on créé 
        //            if (BoardGamesList.Count == 0)
        //            {
        //                BoardGame newBg = new BoardGame(importedList[i].objectname.ToUpper());
        //                newBg.SetMinPlayerFromCsv(importedList[i].minplayers);
        //                newBg.SetMaxPlayerFromCsv(importedList[i].maxplayers);
        //                newBg.SetDurationFromCsv(importedList[i].minplaytime, importedList[i].maxplaytime);
        //                BoardGamesList.Add(newBg);
        //            }
        //            else
        //            {
        //                //on commence par vérifier que le jeu n'existe pas déjà dans la liste
        //                bool alreadyExist = false;
        //                for(int j = 0; j<BoardGamesList.Count;j++)
        //                {
        //                    //compare le titre de chaque jeu avec le titre du nouveau jeu
        //                    if(BoardGamesList[j].title == importedList[i].objectname.ToUpper())
        //                    {
        //                        //si on trouve une correspondance, le bool passe à true
        //                        alreadyExist = true;
        //                    }

        //                }
        //                if (!alreadyExist)//si le jeu n'existe pas encore, on le créé
        //                {
        //                    BoardGame newBg = new BoardGame(importedList[i].objectname.ToUpper());
        //                    newBg.SetMinPlayerFromCsv(importedList[i].minplayers);
        //                    newBg.SetMaxPlayerFromCsv(importedList[i].maxplayers);
        //                    newBg.SetDurationFromCsv(importedList[i].minplaytime, importedList[i].maxplaytime);
        //                    BoardGamesList.Add(newBg);//on l'ajoute à la liste des jeux
        //                }
        //            }
        //        }

        //    }
        //    for (int i = 0; i < importedList.Count; i++)
        //    {
        //        if (importedList[i].itemtype == "expansion")
        //        {
        //            bool match = false;
        //            List<string> expansionTitle = importedList[i].objectname.ToUpper().Split(' ',':').Select(p => p.Trim()).ToList();
        //            for(int j =0; j < BoardGamesList.Count; j++)
        //            {
        //                List<string> gameTitle = BoardGamesList[j].title.Split(' ', ':').Select(p => p.Trim()).ToList();
        //                if(expansionTitle[0] == gameTitle[0])
        //                {
        //                    NEwExpansionAuto(importedList[i], BoardGamesList[j]);
        //                    match = true;
        //                    break;
        //                }
        //            }
        //            if(!match)
        //            {
        //                newExpansion(importedList[i]);
        //            }
                    
        //        }
        //    }
        //    SaveAll();
        //}

        //List<T> ReadFile<T>(string path) where T : class//lit le fichier csv et le map
        //{
        //    //J'avoue avoir copié collé ce code et je n'y comprends pas grand chose
        //    List<T> result = new List<T>();
        //    using (TextReader tr = new StreamReader(path, Encoding.GetEncoding(1252)))
        //    {
        //        var csv = new CsvReader(tr);
        //        try
        //        {
        //            result = csv.GetRecords<T>().ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            Error_Window error = new Error_Window();
        //            error.errorWindow.Text = ex.ToString(); //affiche l'erreur si elle se produit
        //            error.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //            error.ShowDialog();
        //        }
        //    }
        //    return result;
        //}
        #endregion

        void SaveAll()//enregistre les 3 listes
        {
            //vérifie si il n'y a pas des objets inutiles
            if (editorsList.Count != 0)
            {
                List<Editor> toRemove = new List<Editor>();
                foreach (Editor editor in editorsList)
                {
                    int occurence = 0;
                    for (int i = 0; i < BoardGamesList.Count; i++)
                    {
                        if (BoardGamesList[i].localEditorList.Count != 0)
                        {
                            for (int j = 0; j < BoardGamesList[i].localEditorList.Count; j++)
                            {
                                if (BoardGamesList[i].localEditorList[j].Name == editor.Name)
                                {
                                    occurence++;
                                }
                            }
                        }
                    }
                    if (occurence == 0)
                    {
                        toRemove.Add(editor);
                    }
                }
                if (toRemove.Count != 0)
                {
                    foreach (Editor editor in toRemove)
                    {
                        editorsList.Remove(editor);
                    }
                }
            }
            if (styleList.Count != 0)
            {
                List<Style> toRemove = new List<Style>();
                foreach (Style genre in styleList)
                {
                    int occurence = 0;
                    for (int i = 0; i < BoardGamesList.Count; i++)
                    {
                        if (BoardGamesList[i].style.Count != 0)
                        {
                            for (int j = 0; j < BoardGamesList[i].style.Count; j++)
                            {
                                if (BoardGamesList[i].style[j].style == genre.style)
                                {
                                    occurence++;
                                }
                            }
                        }
                    }
                    if (occurence == 0)
                    {
                        toRemove.Add(genre);
                    }
                }
                if (toRemove.Count != 0)
                {
                    foreach (Style style in toRemove)
                    {
                        styleList.Remove(style);
                    }
                }
            }
            //pour finir on enregistre les 3 listes
            Save(BoardGamesList, @ListPath + "BoardGamesList.bin");
            Save(styleList, @ListPath + "styleList.bin");
            Save(editorsList, @ListPath + "editorsList.bin");
        } 

        void Save(object toSave, string path)//méthode générale pour enregistrer en local
        {
            //code copié sans tout y comprendre
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream flux = null;
            try
            {
                flux = new FileStream(path, FileMode.Create, FileAccess.Write);
                formatter.Serialize(flux, toSave);
                flux.Flush();
            }
            catch //(Exception e) //utilisé pendant le débugage mais plus utilisé
            {
                //Error_Window errorWindow = new Error_Window();
                //errorWindow.errorWindow.Text = e.ToString();
                //errorWindow.ShowDialog();
            }
            finally
            {
                if (flux != null)
                    flux.Close();
            }
        }

        public void NEwExpansionAuto(Importcsv imported, BoardGame foundGame)//méthode pour lier une extension à un jeu automatiquement quand importé par .csv
        {
            AutoAddExpansion autoAdd = new AutoAddExpansion();//fenêtre pour proposer un choix
            autoAdd.expansiopnTitleDisplay.Text = imported.objectname.ToUpper();
            autoAdd.gameTitleDisplay.Text = foundGame.title;
            autoAdd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var result = autoAdd.ShowDialog();
            if(result == autoAdd.DialogResult)
            {
                if(autoAdd.isClosedOnNoButton)//on bifurque sur un choix manuel
                {
                    newExpansion(imported);
                }
                else if(autoAdd.isClosedOnYesButton)//l'extension est ajoutée directement
                {
                    foundGame.AddExpansion(imported);
                }
                else//au cas où la fenêtre soit fermée avec la croix
                {
                    newExpansion(imported);
                }
            }
        }

        public void newExpansion(Importcsv imported)// Méthode pour lier l'extension à son jeu depuis csv quand l'auto n'a pas fonctionné
        {
            string title = null;
            SelectGame selectGameScreen = new SelectGame(title);
            selectGameScreen.Indications.Text = " Vous allez rentrer une extension dont le titre est : \n " + imported.objectname + " \n Pour quel jeu? \n Voici la liste des jeux :";
            
            //créé une liste avec seulement les titres des jeux, 
            List<string> titlelist = new List<string>();
            foreach ( BoardGame game in BoardGamesList)
            {
                titlelist.Add(game.title);
            }
            titlelist.Sort();

            //injecte la liste dans la textboxlist
            selectGameScreen.TextBoxList.ItemsSource = titlelist;
            selectGameScreen.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            selectGameScreen.TextBoxList.Focus();//permet d'avoir le focus sur la liste pour 
            //pouvoir faire une recherche au clavier
            var result  = selectGameScreen.ShowDialog();
            if(result == DialogResult.HasValue && selectGameScreen.ClosedOnOk)//deuxième condition
                //s'assure que la fenêtre n'est pas fermée par clic sur la croix
            {
                addNEwExp(selectGameScreen.title.ToUpper(), imported);//créé l'extension dans le jeu
            }
        }


        public void addNEwExp(string userInput, Importcsv imported)//ajoute l'extension dans le jeu
        {
            //retrouve le jeu par son titre
            for (int i = 0; i < BoardGamesList.Count; i++)
            {
                if (BoardGamesList[i].title == userInput)
                {
                    BoardGamesList[i].AddExpansion(imported);//ajoute l'extension par la méthode interne
                    break;
                }
            }
        }


        //méthode pour créer un nouveau jeu manuellement
        void newGame(string title, int min, int max, int durMin, int durMax, List<Style> thisStyleList, List<Editor> thisEditorsList)
        {
            BoardGame newBg = new BoardGame(title);//Nouvel objet "BoardGame"
            newBg.SetMinPlayer(min);
            newBg.SetMaxPlayer(max);
            newBg.SetDuration(durMin, durMax);
            newBg.SetStyle(thisStyleList);
            newBg.SetEditor(thisEditorsList);
            BoardGamesList.Add(newBg);//Ajouter à la liste des jeux

        }


        //méthode qui retourne une liste en fonction du nb de joueurs
        List<BoardGame> FindByPlayerNb(int nbOfPlayers, List<BoardGame> list1, List<BoardGame> list2)
        {
            List<BoardGame> BoardGameList = new List<BoardGame>();//à ne pas confondre
            //avec BOardGamesList, ici il s'abit d'une liste de travail

            //vérifie si des options de recherche sont cochées
            if (cbshortIsChecked)
            {
                foreach (BoardGame game in BoardGamesList)
                {
                    if (game.durationMin <= 20 && game.durationMax <= 20)
                    {
                        //s'assure de ne pas créer de doublon
                        bool alreadyExist = BoardGameList.Contains(game);
                        if (!alreadyExist)
                        {
                            BoardGameList.Add(game);
                        }
                    }
                }
            }
            if (cbmediumIsChecked)//idem pour durée médium
            {
                foreach (BoardGame game in BoardGamesList)
                {
                    if (game.durationMax <= 60)
                    {
                        bool alreadyExist = BoardGameList.Contains(game);
                        if (!alreadyExist)
                        {
                            BoardGameList.Add(game);
                        }
                    }
                }

            }
            if (cblongIsChecked)//idem pour durée longue
            {
                foreach (BoardGame game in BoardGamesList)
                {
                    if (game.durationMin >= 60)
                    {
                        bool alreadyExist = BoardGameList.Contains(game);
                        if (!alreadyExist)
                        {
                            BoardGameList.Add(game);
                        }
                    }
                }
            }
            if (!cbshortIsChecked && !cbmediumIsChecked && !cblongIsChecked)
            {
                //si rien de coché on utilise la liste complète
                BoardGameList = BoardGamesList;
            }

            for (int i = 0; i < BoardGameList.Count; i++)
            {
                //d'abord les jeux qui remplissent les conditions sans avoir
                //besoin d'extension
                if (BoardGameList[i].minPlayers <= nbOfPlayers && nbOfPlayers <= BoardGameList[i].maxPlayers)
                {
                    list1.Add(BoardGameList[i]);
                }
                //puis les jeux avec extension
                if (BoardGameList[i].nbExpansions != 0)
                {
                    for (int j = 0; j < BoardGameList[i].expansionList.Count; j++)
                    {
                        //dont l'extension modifie le nombre de joueurs min ou max
                        if (BoardGameList[i].expansionList[j].ChangesNbOfPlayers == true)
                        {
                            //si l'extension permet de jouer au nombre de joueurs
                            //voulu, on ajoute le jeu à la seconde liste
                            if (BoardGameList[i].expansionList[j].minPlayers <= nbOfPlayers && nbOfPlayers <= BoardGameList[i].expansionList[j].maxPlayers)
                            {
                                list2.Add(BoardGameList[i]);
                            }
                        }
                    }
                }
            }
            //trie la deuxième liste pour enlever les doublons
            var noDoubles = list2.Except(list1).ToList();
            list1.AddRange(noDoubles);//ajoute ce qu'il reste de la deuxième liste
            return list1;
        }

        private void RandomGame_Click(object sender, RoutedEventArgs e)//bouton random
        {
            if (BoardGamesList.Count == 0)//si aucun jeu dans la liste
            {
                noGame();//erreur
            }
            else
            {
                isAllGood = true;
                ClearAll();//efface l'affichage précédent
                checkForParse(out userPlayerNbInput, NbOfPlayers);//vérifie l'entrée utilisateur

                if (isAllGood)
                {
                    rdmGame(userPlayerNbInput);//méthode pour trouver et afficher un jeu
                }
            }
        }

        void rdmGame(int nbOfPlayers)//trouve et affiche un jeu en fonction 
            //du nombre de joueurs voulu
        {
           
            BoardGame bgtemp = new BoardGame();
           
            //on cherche la liste des jeux qui correspond
            List<BoardGame> result = new List<BoardGame>();
            List<BoardGame> result2 = new List<BoardGame>();
            FindByPlayerNb(nbOfPlayers, result, result2);

            //puis on en choisit un
            int gamePicked = random.Next(0, result.Count);
            resultHeader.Text = "Vous pouvez jouer à : ";

            //affichage spécial si besoin d'une extension 
            if (result[gamePicked].maxPlayers < nbOfPlayers)
            {
                List<string> titres = new List<string>();
                //trouve toutes les extensions du jeu qui permettent d'y jouer
                for (int i = 0; i < result[gamePicked].expansionList.Count; i++)
                {
                    if (result[gamePicked].expansionList[i].maxPlayers >= nbOfPlayers)
                    {
                        titres.Add(result[gamePicked].expansionList[i].title);
                    }
                }
                string liste = null;//créé une liste des titres
                foreach (string titre in titres)
                {
                    liste += (titre + "  ");
                }
                if (titres.Count > 1)//si il y en a plusieurs
                {
                    Result.Text = result[gamePicked].title ;
                    resultExpansion.Text = " avec les extensions " + liste;
                }
                else //si il n'y en a qu'une
                {
                    Result.Text = result[gamePicked].title;
                    resultExpansion.Text = " avec l' extension " + liste;
                }
            }
            else//si pas besoin d'extension
            {
                Result.Text =  result[gamePicked].title;
            }
        }

        void ClearAll()//méthode pour nettoyer l'affichage de la page principale
        {
            resultHeader.Clear();
            Result.Clear();
            resultExpansion.Clear();
        }

        private void RandomGame_GotFocus(object sender, RoutedEventArgs e)//si le bouton 
            //random a le focus, il devient default
            //permet d'utiliser la touche entrée pour
            //relancer une recherche
        {
            Matching_list.IsDefault = false;
            RandomGame.IsDefault = true;
        }

        private void Matching_list_GotFocus(object sender, RoutedEventArgs e)//idem au dessus
        {
            RandomGame.IsDefault = false;
            Matching_list.IsDefault = true;
        }

        private void addExpansion_Click(object sender, RoutedEventArgs e)//bouton ajouter extension
        {
            //on récupère le jeu dont il s'agit
            BoardGame selectedGame = (BoardGame)(sender as System.Windows.Controls.Button).DataContext;
            AddExpansion addExpansionWindow = new AddExpansion(selectedGame.title, selectedGame.minPlayers,selectedGame.maxPlayers);
            addExpansionWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var result = addExpansionWindow.ShowDialog();

            //deuxième condition pour ne pas continuer quand femré par la croix
            if(result== DialogResult.HasValue && addExpansionWindow.OkbtnClicked)
            {
                //appel la méthode interne pour créer l'extension
                selectedGame.AddExp(selectedGame, addExpansionWindow.title, addExpansionWindow.changesPlayerNb, addExpansionWindow.min, addExpansionWindow.max);
            }
            resultDisplayList.Items.Refresh();//rafraichit la liste
            SaveAll();//enregistre toutes les listes
        }

        private void ModifyButton_Click(object sender, RoutedEventArgs e)//bouton modifier
        {
            //récupère le jeu
            BoardGame selectedGame = (BoardGame)(sender as System.Windows.Controls.Button).DataContext;
            GameInfos gameInfosWindow = new GameInfos(editorsList, styleList);

            //affiches les propriétés du jeu dans les différentes zones
            gameInfosWindow.Title.Text = selectedGame.title;
            gameInfosWindow.minPlayers.Text = selectedGame.minPlayers.ToString();
            gameInfosWindow.MaxPlayers.Text = selectedGame.maxPlayers.ToString();
            gameInfosWindow.durationMin.Text = selectedGame.durationMin.ToString();
            gameInfosWindow.durationMax.Text = selectedGame.durationMax.ToString();
            gameInfosWindow.EditorsList.ItemsSource = selectedGame.localEditorList;
            gameInfosWindow.StyleList.ItemsSource = selectedGame.style;
            gameInfosWindow.ExpansionList.ItemsSource = selectedGame.expansionList;

            //passe l'objet sélectionné pour le modifier depuis la nouvelle fenêtre
            gameInfosWindow.selectedGame = selectedGame;

            var result = gameInfosWindow.ShowDialog();
            if (result == DialogResult.HasValue)
            {
                if(gameInfosWindow.newEditorsList.Count != 0)//si nouveaux éditeurs
                {
                    //on les ajoute
                    editorsList.AddRange(gameInfosWindow.newEditorsList);
                }
                if(gameInfosWindow.newStyleList.Count != 0)//idem avec les genres de jeu
                {
                    styleList.AddRange(gameInfosWindow.newStyleList);
                }
            }
            //recharge la liste 
            ListBoxSelection list = new ListBoxSelection(BoardGamesList,out currentBgList, resultDisplayList,cbshortIsChecked,cbmediumIsChecked,cblongIsChecked);
            displayList();
            SaveAll();//enregistre
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)//bouton effacer
        {
            //demande une confirmation
            ConfirmWindow confirm = new ConfirmWindow();
            confirm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var result = confirm.ShowDialog();
            if(result == DialogResult.HasValue)
            {
                if(confirm.isOK)//si le bouton oui a été cliqué
                {
                    //enlève le jeu de la liste
                    BoardGame selectedGame = (BoardGame)(sender as System.Windows.Controls.Button).DataContext;
                    BoardGamesList.Remove(selectedGame);
                }
            }
            //rafraichit la liste
            ListBoxSelection list = new ListBoxSelection(BoardGamesList,out currentBgList, resultDisplayList,cbshortIsChecked,cbmediumIsChecked,cblongIsChecked);
            //rafraichit l'affichage
            displayList();
            displayGameCount.Text = "La liste contient " + resultDisplayList.Items.Count + " jeux";
            SaveAll();
        }

        private void MoreFiltersButton_Click(object sender, RoutedEventArgs e)//pas encore implémenté
        {
            toggleFilters();//affiche/cache les filtres suplémentaires
        }
        private void toggleFilters()//affiche/cache les filtres
        {
            if(FiltersDiplayed)
            {
                FiltersOff.Visibility = Visibility.Visible;
                FiltersOn.Visibility = Visibility.Collapsed;
                FiltersGrid.Visibility = Visibility.Collapsed;
                CollapsableFiltersColumn.Width = new GridLength(0);
                FiltersDiplayed = false;
            }
            else
            {
                FiltersOff.Visibility = Visibility.Collapsed;
                FiltersOn.Visibility = Visibility.Visible;
                FiltersGrid.Visibility = Visibility.Visible;
                CollapsableFiltersColumn.Width = new GridLength(0.3, GridUnitType.Star);
                FiltersDiplayed = true;
            }
        }

        private void noGame()//erreur si aucun jeu dans la liste
        {
            Error_Window noGame = new Error_Window();
            noGame.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            noGame.errorWindow.Text = " OUPS, il n'y a aucun jeu dans la liste, commencez par entrer des jeux manuellement ou bien en synchronisant avec BoardGameGeek ;)";
            noGame.ShowDialog();
        }

        private void MoreOptions_Click(object sender, RoutedEventArgs e)//affiche les options
        {
            MoreOptionText.Visibility = Visibility.Collapsed;
            LessOptionsText.Visibility = Visibility.Visible;
            MoreOptions.Visibility = Visibility.Collapsed;
            LessOptions.Visibility = Visibility.Visible;
            CheckBoxGrid.Visibility = Visibility.Visible;
        }

        private void LessOptions_Click(object sender, RoutedEventArgs e)//cache les options
        {
            MoreOptionText.Visibility = Visibility.Visible;
            LessOptionsText.Visibility = Visibility.Collapsed;
            MoreOptions.Visibility = Visibility.Visible;
            LessOptions.Visibility = Visibility.Collapsed;
            CheckBoxGrid.Visibility = Visibility.Collapsed;
            //et remet tout à faux
            cbLong.IsChecked = false;
            cbMedium.IsChecked = false;
            cbShort.IsChecked = false;
        }

        private void cbShort_Checked(object sender, RoutedEventArgs e)//checkbox durée courte
        {
            if (cbshortIsChecked)
            {
                cbshortIsChecked = false;
                if (sender != shortChekBox)
                {
                    shortChekBox.Unchecked -= cbShort_Checked;
                    shortChekBox.IsChecked = false;
                    shortChekBox.Unchecked += cbShort_Checked;
                    
                }
                if (sender != cbShort)
                {
                    cbShort.Unchecked -= cbShort_Checked;
                    cbShort.IsChecked = false;
                    cbShort.Unchecked += cbShort_Checked;
                }
            }
            else
            {
                cbshortIsChecked = true;
                if (sender != shortChekBox)
                {
                    shortChekBox.Checked -= cbShort_Checked;
                    shortChekBox.IsChecked = true;
                    shortChekBox.Checked += cbShort_Checked;
                }
                if (sender != cbShort)
                {
                    cbShort.Checked -= cbShort_Checked;
                    cbShort.IsChecked = true;
                    cbShort.Checked += cbShort_Checked;
                }
            }
        }
        

        private void cbMedium_Checked(object sender, RoutedEventArgs e)//checkbox durée moyenne
        {
            if (cbmediumIsChecked)
            {
                cbmediumIsChecked = false;
                if (sender != middleCheckBox)
                {
                    middleCheckBox.Unchecked -= cbMedium_Checked;
                    middleCheckBox.IsChecked = false;
                    middleCheckBox.Unchecked += cbMedium_Checked;
                }
                if (sender != cbMedium)
                {
                    cbMedium.Unchecked -= cbMedium_Checked;
                    cbMedium.IsChecked = false;
                    cbMedium.Unchecked += cbMedium_Checked;
                }
            }
            else
            {
                cbmediumIsChecked = true;
                if (sender !=middleCheckBox)
                {
                    middleCheckBox.Checked -= cbMedium_Checked;
                    middleCheckBox.IsChecked = true;
                    middleCheckBox.Checked += cbMedium_Checked;
                }
                if (sender != cbMedium)
                {
                    cbMedium.Checked -= cbMedium_Checked;
                    cbMedium.IsChecked = true;
                    cbMedium.Checked += cbMedium_Checked; ;
                }
            }
        }


        private void cbLong_Checked(object sender, RoutedEventArgs e)//checkbox durée longue
        {
            if (cblongIsChecked)
            {
                cblongIsChecked = false;
                if (sender != longCheckBox)
                {
                    longCheckBox.Unchecked -= cbLong_Checked;
                    longCheckBox.IsChecked = false;
                    longCheckBox.Unchecked+= cbLong_Checked;
                }
                if (sender != cbLong)
                {
                    cbLong.Unchecked-= cbLong_Checked;
                    cbLong.IsChecked = false;
                    cbLong.Unchecked += cbLong_Checked;
                }
            }
            else
            {
                cblongIsChecked = true;
                if (sender != longCheckBox)
                {
                    longCheckBox.Checked -= cbLong_Checked;
                    longCheckBox.IsChecked = true;
                    longCheckBox.Checked+= cbLong_Checked;
                }
                if (sender != cbLong)
                {
                    cbLong.Checked-= cbLong_Checked;
                    cbLong.IsChecked = true;
                    cbLong.Checked += cbLong_Checked;
                }
            }
        }


        private void MatchExpansion(List<Importcsv> importedList)
        {
            for (int i = 0; i < importedList.Count; i++)
            {
                bool match = false;
                List<string> expansionTitle = importedList[i].objectname.ToUpper().Split(' ', ':').Select(p => p.Trim()).ToList();
                for (int j = 0; j < BoardGamesList.Count; j++)
                {
                    List<string> gameTitle = BoardGamesList[j].title.Split(' ', ':').Select(p => p.Trim()).ToList();
                    if (expansionTitle[0] == gameTitle[0])
                    {
                        NEwExpansionAuto(importedList[i], BoardGamesList[j]);
                        match = true;
                        break;
                    }
                }
                if (!match)
                {
                    newExpansion(importedList[i]);
                }
            }
        }

        private void setCurrentStyleList(out List<Style> currentStyleList)
        {
            
            List<Style> tempList = new List<Style>();
            styleFilterListBox.ItemsSource = tempList;
            foreach (BoardGame game in currentBgList)
            {

                if(game.style.Count!=0)
                {
                    if (tempList.Count != 0)
                    {
                        List<Style> newTempList = new List<Style>();
                        //foreach (Style style in game.style)
                        //{
                        //    bool alreadyExists = false;
                        //    for (int i = 0; i < tempList.Count; i++)
                        //    {
                        //        if(style.style == tempList[i].style)
                        //        {
                        //            alreadyExists = true;
                        //        }
                        //    }
                        //    if(!alreadyExists)
                        //    {
                        //        newTempList.Add(style);
                        //    }
                        //}
                        for(int i =0; i < game.style.Count; i++ )
                        {
                            bool alreadyExists = false;
                            foreach(Style style in tempList)
                            {
                                if(style.style == game.style[i].style)
                                {
                                    alreadyExists = true;
                                    break;
                                }
                            }
                            if(!alreadyExists)
                            {
                                newTempList.Add(game.style[i]);
                            }
                        }
                        if(newTempList.Count!=0)
                        {
                            tempList.AddRange(newTempList);
                        }
                    }
                    else
                    {
                        tempList.AddRange(game.style);
                    }
                }
            }
            currentStyleList = tempList.OrderBy(o=>o.style).ToList();
            styleFilterListBox.ItemsSource = currentStyleList;
        }
        private void setCurrentEditorList(out List<Editor> currentEditorList)
        {
            
            List<Editor> TempList = new List<Editor>();
            editorFilterListBox.ItemsSource = TempList;

            foreach (BoardGame game in currentBgList)
            {
                if(game.localEditorList.Count!=0)
                {
                    if(TempList.Count!=0)
                    {
                        List<Editor> newTemp = new List<Editor>();
                       
                        for(int i=0; i< game.localEditorList.Count;i++)
                        {
                            bool alreadyExists = false;
                            foreach(Editor editor in TempList)
                            {
                                if(game.localEditorList[i].Name == editor.Name)
                                {
                                    alreadyExists = true;
                                    break;
                                }
                            }
                            if(!alreadyExists)
                            {
                                newTemp.Add(game.localEditorList[i]);
                            }
                        }
                        if (newTemp.Count != 0) 
                        {
                            TempList.AddRange(newTemp);
                        }
                    }
                    else
                    {
                        TempList.AddRange(game.localEditorList);
                    }
                }
            }
            currentEditorList = TempList.OrderBy(o=>o.Name).ToList();
            editorFilterListBox.ItemsSource = currentEditorList;
        }

        private void displayList()
        {
            resultDisplayList.ItemsSource = currentBgList;
            setCurrentStyleList(out currentStyleList);
            setCurrentEditorList(out currentEditorList);
            displayGameCount.Text = "La liste contient " + currentBgList.Count + " jeux.";
        }


        private void RefreshResult_Click(object sender, RoutedEventArgs e)
        {
            List<Style> selectedStyles = new List<Style>();
            List<Editor> selectedEditors = new List<Editor>();
            
            if(styleFilterListBox.SelectedItems.Count !=0)
            {
                foreach(Style style in styleFilterListBox.SelectedItems)
                {
                    selectedStyles.Add(style);
                }
            }

            if(editorFilterListBox.SelectedItems.Count != 0)
            {
                foreach(Editor editor in editorFilterListBox.SelectedItems)
                {
                    selectedEditors.Add(editor);
                }
            }
            ListBoxSelection selection = new ListBoxSelection(BoardGamesList, out currentBgList, selectedStyles, selectedEditors, userPlayerNbInput, cbshortIsChecked, cbmediumIsChecked, cblongIsChecked);
            displayList();
        }

        List<Style> temp = new List<Style>();

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Help help = new Help();
            help.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            help.ShowDialog();
        }

        private void styleFilterSearchTb_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(styleFilterSearchTb.Text != string.Empty)
            {
                List<Style> tempList = new List<Style>();
                string input = styleFilterSearchTb.Text.ToUpper();
                foreach(Style style in currentStyleList)
                {
                    if(style.style.StartsWith(input))
                    {
                        tempList.Add(style);
                    }
                }

                styleFilterListBox.ItemsSource = tempList;
                styleFilterListBox.Items.Refresh();
            }
            else
            {
                styleFilterListBox.ItemsSource = currentStyleList;
            }
        }

        private void EditorFilterSearchTb_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (EditorFilterSearchTb.Text != string.Empty)
            {
                List<Editor> tempList = new List<Editor>();
                string input = EditorFilterSearchTb.Text.ToUpper();
                foreach (Editor editor in currentEditorList)
                {
                    if (editor.Name.StartsWith(input))
                    {
                        tempList.Add(editor);
                    }
                }

                editorFilterListBox.ItemsSource = tempList;
                editorFilterListBox.Items.Refresh();
            }
            else
            {
                editorFilterListBox.ItemsSource = currentEditorList;
            }
        }
    }
}
