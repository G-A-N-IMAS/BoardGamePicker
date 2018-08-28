using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Controls;
//using System.Windows.Forms;
//using CsvHelper;
//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Windows;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using System.ComponentModel;

namespace BoardGamePicker
{ //BoardGamePicker
    [Serializable] // pour écriture fichier binaire, voir https://openclassrooms.com/fr/courses/379507-la-programmation-reseau-en-net/379505-la-serialisation-des-objets
    public class BoardGame 
    {
        public BoardGame()//Constructeur pour récupérer les méthodes
        {

        }
        public List<Expansion> expansionList= new List<Expansion>();

        public BoardGame(string title)//Constructeur pour création de jeu
        {
            Title = title.ToUpper();
        }

        public string expansionListTitles { get; set; }
        public string expansionDisclaimer { get; set; }

        public override string ToString()
        {
            return Title;
        }

        public int Converted { get; set; }
        public void Convert(string chaine)
        {
            bool test = int.TryParse(chaine, out int result);
            if (test)
            {
                Converted = result;
            }
            else
            {
                Console.WriteLine("Oups, j'ai besoin d'un nombre, réessayez:");
                Convert(Console.ReadLine());
            }
        }

        public string title
        {
            get
            {
                return Title;
            }
            set
            {
                Title = value;
            }
        }
        protected string Title;

        public List<Editor> localEditorList = new List<Editor>();
        public string editorListToString { get; set; }
        public void SetEditor(List<Editor> thisEditorsList)
        {

            localEditorList.AddRange(thisEditorsList);
            foreach(Editor editor in thisEditorsList)
            {
                editorListToString += editor.Name + " | ";
            }
        }

        public void refreshEditorListToString()
        {
            editorListToString = string.Empty;
            foreach (Editor editor in localEditorList)
            {
                editorListToString += editor.Name + " | ";
            }

        }

        public void NbOfPLayers()
        {
            string min = minPlayers.ToString();
            string max = maxPlayers.ToString();

            if(nbExpansions !=0)
            {

                Console.WriteLine("Le jeu de base se joue de {0} à {1} joueurs.", min, max);
                for(int i = 0; i<expansionList.Count; i++)
                    if(expansionList[i].ChangesNbOfPlayers == true)
                    {
                        string expmin = expansionList[i].minPlayers.ToString();
                        string expmax = expansionList[i].maxPlayers.ToString();
                        Console.WriteLine("l'extension {0} permet de jouer de {1] à {2} joueurs.",expansionList[i].title, expmin, expmax);
                    }    
            }
            else
            {
                Console.WriteLine("Ce jeu se joue de {0} à {1} joueurs.", min, max);
            }
        }//Appeler pour connaitre le nombre de joueurs avec/sans extenstion.

        public int minPlayers { get;  set; }
        public void SetMinPlayer(int min)
        {
            //Console.WriteLine("Nb de joueurs minimum?");
            //Convert(Console.ReadLine());
            minPlayers = min;
        }
        public void SetMinPlayerFromCsv(int i)
        {
            minPlayers = i;
        }

        public int maxPlayers {get;  set;}
        public void SetMaxPlayer(int max)
        {
            //Console.WriteLine("Nombre de joueurs max?");
            //Convert(max);
            maxPlayers = max;
        }
        public void SetMaxPlayerFromCsv(int i)
        {
            maxPlayers = i;
        }

        public int durationMin {get;  set;}
        public int durationMax { get;  set; }
        public void SetDuration(int min, int max)
        {
            //Console.WriteLine("Combien de temps dure une partie au minimum?");
            //Convert(Console.ReadLine());
            durationMin = min;
            //Console.WriteLine("Combien de temps dure une partie au maximum?");
            //Convert(Console.ReadLine());
            durationMax = max;
        }
        public void SetDurationFromCsv(int i, int j)
        {
            durationMin = i;
            durationMax = j;
        }

        public List<Style> style = new List<Style>();//coop, deckbuilding, enchères ...
        public string styleListToString { get; set; }
        public void SetStyle(List<Style> thisstyleList)
        {
            style.AddRange(thisstyleList);
            foreach(Style style in thisstyleList)
            {
                styleListToString += style.style + " | "; 
            }
        }

        public void refreshStyleListToString()
        {
            styleListToString = string.Empty;
            foreach(Style style in style)
            {
                styleListToString += style.style + " | ";
            }
        }

        public void AddExpansion(Importcsv imported)
        {
            //Indications.Clear();
            //TextBoxList.Items.Clear();
            bool alreadyExists = false;
            foreach(Expansion exp in this.expansionList)
            {
                if(exp.title.ToUpper()==imported.objectname.ToUpper())
                {
                    alreadyExists = true;
                    break;
                }
            }
            if (!alreadyExists)
            {
                Expansion expansion = new Expansion(imported.objectname);
                nbExpansions++;
                int min = imported.minplayers;
                int max = imported.maxplayers;
                if (this.minPlayers != min || this.maxPlayers != max)
                {
                    expansion.changePlayerNb(min, max);
                }
                expansionList.Add(expansion);
            }
        }

        public void AddExp(BoardGame currentBoardGame, string title, bool changesPlayerNb, int min, int max)
        {
            Expansion expansion = new Expansion(title);
            currentBoardGame.nbExpansions++;
            currentBoardGame.expansionList.Add(expansion);
            if(changesPlayerNb)
            {
                expansion.changePlayerNb(min, max);
            }
            expansionListTitles += title + " ; ";
        }

        public int nbExpansions { get; protected set; }


        public string UppercaseFirst(string s) //Méthode récupérée sur internet pour mettre première lettre en maj
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
        public void removeExpansion(Expansion expansion)
        {
            nbExpansions--;
            expansionList.Remove(expansion);
        }
    }
}
