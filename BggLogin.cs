using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Net;
using System.Xml;
using BoardGamePicker;
using System.ComponentModel;

namespace importBGG
{
    partial class BggLogin
    {
        string UserName { get; set; }
        private string urlboardgames()
        {
            string url = string.Format("https://www.boardgamegeek.com/xmlapi2/collection?username={0}&excludesubtype=boardgameexpansion&own=1&stats=1", UserName);
            return url;
        }
        private string urlexpansions()
        {
            string url = string.Format("https://www.boardgamegeek.com/xmlapi2/collection?username={0}&subtype=boardgameexpansion&own=1&stats=1", UserName);
            return url;
        }
       
        public bool waiting = false;
        public List<BoardGame> importedList = new List<BoardGame>();
        //public List<Expansion> importedExpansionList = new List<Expansion>();
        public List<Importcsv> importedExpansionsList = new List<Importcsv>();

        public BggLogin(string usrNm)
        {
            UserName = usrNm;
        }

        private List<BoardGame> SyncWithBgg( List<BoardGame> BoardGamesList, out List<Importcsv> ExpansionList) //!!! en cas d'invalid username, la réponse est 200!
            //vérifier qu'il n'y ait pas de message d'erreur
            //vérifier aussi que la liste ne soit pas vide...
        {
            try
            {
                List<BoardGame> BgList = new List<BoardGame>();
                List<Importcsv> ExpList = new List<Importcsv>();

                XmlTextReader reader = new XmlTextReader(urlboardgames());

                while(reader.Read())
                {
                    switch(reader.NodeType)
                    {
                        case XmlNodeType.Element:

                            {
                                if (reader.Name == "error")
                                {
                                    Dispatcher.BeginInvoke(new Action(delegate() { this.messageTb.Text += "oups, êtes vous sûr d'avoir rentré le bon nom d'utilisateur ? \n Il est possible que le site BoardGameGeek ait un soucis, réessayez plus tard."; })) ;
                                    break;
                                }
                                if (reader.Name == "message")//demande d'attente
                                {
                                    while (reader.Name == "message")
                                    {
                                        //this.messageTb.Text += "Connexion à BGG, cela peut prendre quelques minutes...";
                                        //WaitingMessage();
                                        System.Threading.Thread.Sleep(4000);
                                        reader = new XmlTextReader(urlboardgames());
                                    }
                                }
                                else if (reader.Name == "items")
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        if (reader.Name == "totalitems")
                                        {
                                            if (reader.Value == 0.ToString())
                                            {
                                                //la liste est vide, afficher un message
                                                Dispatcher.BeginInvoke(new Action(delegate() { this.messageTb.Text += "Il semble que vous n'ayez pas rentré de jeux sur bgg"; })) ;
                                                break;
                                            }
                                            else
                                            {
                                                break;
                                                //reader.MoveToElement();
                                            }
                                        }
                                    }
                                }

                                else if (reader.Name == "name")
                                {
                                    BoardGame game = new BoardGame();
                                    bool keepreading = true;
                                    bool alreadyExists=false;
                                    while (reader.Read()&&keepreading&&!alreadyExists)
                                    {  
                                        switch(reader.NodeType)
                                        {
                                            case XmlNodeType.Text:
                                                if (BoardGamesList.Count != 0)
                                                {
                                                    foreach (BoardGame Bg in BoardGamesList)
                                                    {
                                                        if (reader.Value.ToUpper() == Bg.title)
                                                        {
                                                            alreadyExists = true;
                                                            break;
                                                        }
                                                    }
                                                    if (!alreadyExists)
                                                    {
                                                        game.title = reader.Value.ToUpper();
                                                    }
                                                }
                                                else
                                                {
                                                    game.title = reader.Value.ToUpper();
                                                }
                                                break;
                                            case XmlNodeType.EndElement:
                                                keepreading = false;
                                                break;
                                            default:break;
                                        }
                                    }
                                    bool keepReadingToTheEnd = true;
                                    while(reader.Read()&&keepReadingToTheEnd&&!alreadyExists)
                                    {
                                        if(reader.Name=="stats")
                                        {
                                            while(reader.MoveToNextAttribute())
                                            {
                                                switch(reader.Name)
                                                {
                                                    case "minplayers":
                                                        game.minPlayers = Convert.ToInt32(reader.Value);
                                                        break;
                                                    case "maxplayers":
                                                        game.maxPlayers = Convert.ToInt32(reader.Value);
                                                        break;
                                                    case "minplaytime":
                                                        game.durationMin = Convert.ToInt32(reader.Value);
                                                        break;
                                                    case "maxplaytime":
                                                        game.durationMax = Convert.ToInt32(reader.Value);
                                                        BgList.Add(game);
                                                        break;
                                                    case "numowned":
                                                        keepReadingToTheEnd = false;
                                                        break;
                                                    default:break;
                                                }
                                            }
                                        }
                                    }
                                }
                                    break;
                            }

                        default: break;
                    }
                }
                //string titleList= null;
                //foreach(BoardGame game in BgList)
                //{
                //    titleList += game.title + " | ";
                //}

                XmlTextReader expReader = new XmlTextReader(urlexpansions());
                while(expReader.Read())
                {
                    switch (expReader.NodeType)
                    {
                        case XmlNodeType.Element:

                            {
                                if (expReader.Name == "error")
                                {
                                    Dispatcher.BeginInvoke(new Action(delegate () { this.messageTb.Text += "Oups, il semble y avoir une erreur lors de l'import, désolé.\n Il est possible que le site BoardGameGeek ait un soucis, réessayez plus tard."; }));
                                    break;
                                }
                                if (expReader.Name == "message")//demande d'attente
                                {
                                    while (expReader.Name == "message")
                                    {
                                        
                                        System.Threading.Thread.Sleep(4000);
                                        expReader = new XmlTextReader(urlexpansions());
                                    }
                                }
                                else if (expReader.Name == "items")
                                {
                                    while (expReader.MoveToNextAttribute())
                                    {
                                        if (expReader.Name == "totalitems")
                                        {
                                            if (expReader.Value == 0.ToString())
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                break;
                                                //reader.MoveToElement();
                                            }
                                        }
                                    }
                                }
                             
                                else if (expReader.Name == "name")
                                {
                                    //string temptitle = null;
                                    Importcsv expansion = new Importcsv();
                                    //Expansion expansion = new Expansion(temptitle);
                                    bool keepreading = true;
                                    bool alreadyExist = false;
                                    while (expReader.Read() && keepreading&&!alreadyExist)
                                    {
                                        switch (expReader.NodeType)
                                        {
                                            case XmlNodeType.Text:
                                                if (BoardGamesList.Count != 0)
                                                {
                                                    foreach (BoardGame game in BoardGamesList)
                                                    {
                                                        if (game.expansionList.Count != 0)
                                                        {
                                                            for (int i = 0; i < game.expansionList.Count; i++)
                                                            {
                                                                if (game.expansionList[i].title.ToUpper() == expReader.Value.ToUpper())
                                                                {
                                                                    alreadyExist = true;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        if (!alreadyExist)
                                                        {
                                                            expansion.objectname = expReader.Value.ToUpper();
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    expansion.objectname = expReader.Value.ToUpper();
                                                }
                                                break;
                                            case XmlNodeType.EndElement:
                                                keepreading = false;
                                                break;
                                            default: break;
                                        }
                                    }
                                    bool keepReadingToTheEnd = true;
                                    while (expReader.Read() && keepReadingToTheEnd&&!alreadyExist)
                                    {
                                        if (expReader.Name == "stats")
                                        {
                                            while (expReader.MoveToNextAttribute())
                                            {
                                                switch (expReader.Name)
                                                {
                                                    case "minplayers":
                                                        expansion.minplayers = Convert.ToInt32(expReader.Value);
                                                        break;
                                                    case "maxplayers":
                                                        expansion.maxplayers = Convert.ToInt32(expReader.Value);
                                                        break;
                                                    case "minplaytime":
                                                        expansion.minplaytime = Convert.ToInt32(expReader.Value);
                                                        break;
                                                    case "maxplaytime":
                                                        expansion.maxplaytime = Convert.ToInt32(expReader.Value);
                                                        ExpList.Add(expansion);
                                                        break;
                                                    case "numowned":
                                                        keepReadingToTheEnd = false;
                                                        break;
                                                    default: break;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            }

                        default: break;
                    }
                }


                ExpansionList = ExpList;
                //Dispatcher.BeginInvoke(new Action(delegate () { this.messageTb.Text = titleList; }));
                IsWaiting = false;
                Dispatcher.BeginInvoke(new Action(delegate () { this.messageTb.Text ="Les jeux ont été importés avec succès! \n rappuyez sur OK pour fermer"; }));
                IsAllGood = true;
                return BgList;
                //this.messageTb.Text = titleList;
            }
            catch(WebException we)
            {
                List<BoardGame> emptyList = new List<BoardGame>();
                List<Importcsv> emptyExpList = new List<Importcsv>();
                string errorMessage = "Oups, êtes vous sûr d'être connecté à internet? \n Erreur : \n";
                Dispatcher.BeginInvoke(new Action(delegate() { this.messageTb.Text += errorMessage+ we.ToString(); }));
                ExpansionList = emptyExpList;
                IsWaiting = false;
                return emptyList;
            }
        }

        private void WaitingMessage()
        {
            IsWaiting = true;
            
            while(IsWaiting)
            {
                Dispatcher.BeginInvoke(new Action(delegate () { this.messageTb.Clear(); }));
                Dispatcher.BeginInvoke(new Action(delegate () { this.messageTb.Text = "Connexion à Bgg, cela peut prendre quelques minutes... \n"; }));
                Dispatcher.BeginInvoke(new Action(delegate () { this.messageTb.Text += "\n"; }));
                for (int i = 0; i < 3; i++)
                {
                    System.Threading.Thread.Sleep(1000);
                    Dispatcher.BeginInvoke(new Action(delegate () { this.messageTb.Text += "."; }));
                }
                System.Threading.Thread.Sleep(1000);
                
            }
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            importedList = SyncWithBgg(BoardGamesList , out importedExpansionsList);
        }
        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsWaiting = false;
        }

        private void WaitingWork(object sender, DoWorkEventArgs e)
        {
            WaitingMessage();
        }
        private void WaitingCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsWaiting = false;
        }


    }

}
