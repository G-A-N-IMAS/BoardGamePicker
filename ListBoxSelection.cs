using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using System.ComponentModel;
//using Microsoft.WindowsAPICodePack.Dialogs;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;
//using CsvHelper;

namespace BoardGamePicker
{
    public partial class ListBoxSelection
    {

        static List<BoardGame> toResultDisplayList = new List<BoardGame>();
        ListCollectionView ResultDisplayListCV = new ListCollectionView(toResultDisplayList);

        public ListBoxSelection(List<BoardGame> FullBoardGameList, out List<BoardGame> currentBgList, ListBox resultDisplayList, bool cbshortIsChecked, bool cbmediumIsChecked, bool cblongIsChecked)
        {
            List<BoardGame> toList = new List<BoardGame>();
            List<BoardGame> BoardGameList = new List<BoardGame>();

            if(cbshortIsChecked)
            {
                foreach(BoardGame game in FullBoardGameList)
                {
                    if(game.durationMin <= 20 && game.durationMax <= 20)
                    {
                        bool alreadyExist = BoardGameList.Contains(game);
                        if (!alreadyExist)
                        {
                            BoardGameList.Add(game);
                        }
                    }
                }
            }
            if(cbmediumIsChecked)
            {
                foreach(BoardGame game in FullBoardGameList)
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
            if(cblongIsChecked)
            {
                foreach(BoardGame game in FullBoardGameList)
                {
                    if(game.durationMin>= 60  )
                    {
                        bool alreadyExist = BoardGameList.Contains(game);
                        if (!alreadyExist)
                        {
                            BoardGameList.Add(game);
                        }
                    }
                }
            }
            if(!cbshortIsChecked && !cbmediumIsChecked && !cblongIsChecked)
            {
                BoardGameList = FullBoardGameList;
            }
            

            List<BoardGame> sortedList = BoardGameList.OrderBy(o => o.title).ToList();

            toList.AddRange(sortedList);
            foreach (BoardGame game in BoardGameList)
            {
                game.expansionDisclaimer = null;
                string expansionListTitles = string.Empty;
                if (game.nbExpansions != 0)
                {
                    foreach (Expansion expansion in game.expansionList)
                    {
                        expansionListTitles += expansion.title + " ; ";
                    }
                }
                game.expansionListTitles = expansionListTitles;

            }
            ListCollectionView todisplay = new ListCollectionView(toList);
            currentBgList = toList;
            //resultDisplayList.ItemsSource = todisplay;
            //toResultDisplayList = toList;
            //resultDisplayList.ItemsSource = ResultDisplayListCV;
        }

        public ListBoxSelection(List<BoardGame> FullBoardGamesList, out List<BoardGame> currentBgList, ListBox resultDisplayList, int playerNb, bool cbshortIsChecked, bool cbmediumIsChecked, bool cblongIsChecked)
        {
            BoardGame bgtemp = new BoardGame();

            List<BoardGame> BoardGamesList = new List<BoardGame>();

            if (cbshortIsChecked)
            {
                foreach (BoardGame game in FullBoardGamesList)
                {
                    if (game.durationMin <= 20 && game.durationMax <= 20)
                    {
                        bool alreadyExist = BoardGamesList.Contains(game);
                        if (!alreadyExist)
                        {
                            BoardGamesList.Add(game);
                        }
                    }
                }
            }
            if (cbmediumIsChecked)
            {
                foreach (BoardGame game in FullBoardGamesList)
                {
                    if (game.durationMax <= 60)
                    {
                        bool alreadyExist = BoardGamesList.Contains(game);
                        if (!alreadyExist)
                        {
                            BoardGamesList.Add(game);
                        }
                    }
                }
            }
            if (cblongIsChecked)
            {
                foreach (BoardGame game in FullBoardGamesList)
                {
                    if (game.durationMin >= 60)
                    {
                        bool alreadyExist = BoardGamesList.Contains(game);
                        if (!alreadyExist)
                        {
                            BoardGamesList.Add(game);
                        }
                    }
                }
            }
            if (!cbshortIsChecked && !cbmediumIsChecked && !cblongIsChecked)
            {
                BoardGamesList = FullBoardGamesList;
            }

            List<BoardGame> result = new List<BoardGame>();
            List<BoardGame> result2 = new List<BoardGame>();
            FindByPlayerNb(playerNb, result, result2);
            List<BoardGame> FindByPlayerNb(int nbOfPlayers, List<BoardGame> list1, List<BoardGame> list2)
            {
                for (int i = 0; i < BoardGamesList.Count; i++)
                {
                    BoardGamesList[i].expansionDisclaimer = null;
                    if (BoardGamesList[i].minPlayers <= nbOfPlayers && nbOfPlayers <= BoardGamesList[i].maxPlayers)
                    {
                        list1.Add(BoardGamesList[i]);
                    }
                    if (BoardGamesList[i].nbExpansions != 0)
                    {
                        
                        for (int j = 0; j < BoardGamesList[i].expansionList.Count; j++)
                        {
                            bool alreadyExist = list1.Contains(BoardGamesList[i]);
                            if (!alreadyExist)
                            {
                                if (BoardGamesList[i].expansionList[j].ChangesNbOfPlayers == true)
                                {
                                    if (BoardGamesList[i].expansionList[j].minPlayers <= nbOfPlayers && nbOfPlayers <= BoardGamesList[i].expansionList[j].maxPlayers)
                                    {
                                        list2.Add(BoardGamesList[i]);
                                        if (BoardGamesList[i].expansionDisclaimer == null)
                                        {
                                            BoardGamesList[i].expansionDisclaimer = "Avec : " + BoardGamesList[i].expansionList[j].title;
                                        }
                                        else
                                        {
                                            BoardGamesList[i].expansionDisclaimer += " ou " + BoardGamesList[i].expansionList[j].title;
                                        }

                                    }
                                }
                            }
                        }

                    }
                }
                var noDoubles = list2.Except(list1).ToList();
                list1.AddRange(noDoubles);
                //list1.Sort();
                return list1;
            }
            List<BoardGame> sortedList = result.OrderBy(o => o.title).ToList();
            //ListCollectionView todisplay = new ListCollectionView(sortedList);
            //resultDisplayList.ItemsSource = todisplay;
            currentBgList = sortedList;
            //toResultDisplayList = sortedList;
            //resultDisplayList.ItemsSource = ResultDisplayListCV;
        }


        public ListBoxSelection() { }

        public ListBoxSelection(List<BoardGame> FullBoardGamesList, out List<BoardGame> currentBgList, List<Style> selectedStyles, List<Editor> selectedEditors, int playerNb, bool cbshortIsChecked, bool cbmediumIsChecked, bool cblongIsChecked)
        {
            BoardGame bgtemp = new BoardGame();

            List<BoardGame> BoardGamesList = new List<BoardGame>();

            if (cbshortIsChecked)
            {
                foreach (BoardGame game in FullBoardGamesList)
                {
                    if (game.durationMin <= 20 && game.durationMax <= 20)
                    {
                        bool alreadyExist = BoardGamesList.Contains(game);
                        if (!alreadyExist)
                        {
                            BoardGamesList.Add(game);
                        }
                    }
                }
            }
            if (cbmediumIsChecked)
            {
                foreach (BoardGame game in FullBoardGamesList)
                {
                    if (game.durationMax <= 60)
                    {
                        bool alreadyExist = BoardGamesList.Contains(game);
                        if (!alreadyExist)
                        {
                            BoardGamesList.Add(game);
                        }
                    }
                }
            }
            if (cblongIsChecked)
            {
                foreach (BoardGame game in FullBoardGamesList)
                {
                    if (game.durationMin >= 60)
                    {
                        bool alreadyExist = BoardGamesList.Contains(game);
                        if (!alreadyExist)
                        {
                            BoardGamesList.Add(game);
                        }
                    }
                }
            }
            if (!cbshortIsChecked && !cbmediumIsChecked && !cblongIsChecked)
            {
                BoardGamesList = FullBoardGamesList;
            }

            List<BoardGame> filteredByStyles = new List<BoardGame>();
            if (selectedStyles.Count != 0)
            {
                foreach (BoardGame game in BoardGamesList)
                {
                    if (game.style.Count != 0)
                    {
                        for (int i = 0; i < game.style.Count; i++)
                        {
                            foreach (Style style in selectedStyles)
                            {
                                if (game.style[i].style == style.style)
                                {
                                    filteredByStyles.Add(game);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                filteredByStyles = BoardGamesList;
            }

            List<BoardGame> filteredByEditor = new List<BoardGame>();
            if(selectedEditors.Count!=0)
            {
                foreach(BoardGame game in filteredByStyles)
                {
                    if(game.localEditorList.Count!=0)
                    {
                        for(int i=0; i<game.localEditorList.Count; i++)
                        {
                            foreach(Editor editor in selectedEditors)
                            {
                                if(game.localEditorList[i].Name == editor.Name)
                                {
                                    filteredByEditor.Add(game);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                filteredByEditor = filteredByStyles;
            }

            List<BoardGame> result = new List<BoardGame>();
            if (playerNb != 0)
            {
                List<BoardGame> result2 = new List<BoardGame>();
                FindByPlayerNb(playerNb, result, result2);
                List<BoardGame> FindByPlayerNb(int nbOfPlayers, List<BoardGame> list1, List<BoardGame> list2)
                {
                    for (int i = 0; i < filteredByEditor.Count; i++)
                    {
                        filteredByEditor[i].expansionDisclaimer = null;
                        if (filteredByEditor[i].minPlayers <= nbOfPlayers && nbOfPlayers <= filteredByEditor[i].maxPlayers)
                        {
                            list1.Add(filteredByEditor[i]);
                        }
                        if (filteredByEditor[i].nbExpansions != 0)
                        {

                            for (int j = 0; j < filteredByEditor[i].expansionList.Count; j++)
                            {
                                bool alreadyExist = list1.Contains(filteredByEditor[i]);
                                if (!alreadyExist)
                                {
                                    if (filteredByEditor[i].expansionList[j].ChangesNbOfPlayers == true)
                                    {
                                        if (filteredByEditor[i].expansionList[j].minPlayers <= nbOfPlayers && nbOfPlayers <= filteredByEditor[i].expansionList[j].maxPlayers)
                                        {
                                            list2.Add(filteredByEditor[i]);
                                            if (filteredByEditor[i].expansionDisclaimer == null)
                                            {
                                                filteredByEditor[i].expansionDisclaimer = "Avec : " + filteredByEditor[i].expansionList[j].title;
                                            }
                                            else
                                            {
                                                filteredByEditor[i].expansionDisclaimer += " ou " + filteredByEditor[i].expansionList[j].title;
                                            }

                                        }
                                    }
                                }
                            }

                        }
                    }
                    var noDoubles = list2.Except(list1).ToList();
                    list1.AddRange(noDoubles);
                    //list1.Sort();
                    return list1;
                }
            }
            else { result = filteredByEditor; }
            List<BoardGame> sortedList = result.OrderBy(o => o.title).ToList();
            //ListCollectionView todisplay = new ListCollectionView(sortedList);
            //resultDisplayList.ItemsSource = todisplay;
            currentBgList = sortedList;
        }
    }
    

}
