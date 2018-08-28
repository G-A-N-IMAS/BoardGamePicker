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
    /// Logique d'interaction pour Help.xaml
    /// </summary>
    public partial class Help : Window
    {
        public Help()
        {
            InitializeComponent();
            HelpTb.Text = help();
        }
        private string help()
        {
            string help =
                header
                + "\n"
                + string1
                + "\n \n"
                + string2
                + "\n"
                + "\n"
                + string3
                + "\n"
                + "\n"
                + string4
                + "\n"
                + string5
                + "\n"
                + "\n"
                + string6
                + "\n"
                + "\n"
                + string7
                + "\n"
                + "\n"
                + string8
                + "\n"
                + string9
                + "\n"
                + "\n"
                + string10
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + "\n"
                + string11;

            return help;

        }

        string header = "Aide :";
        string string1 = "Ce programme a pour but de vous aider à trouver des jeux dans votre ludothèque.";
        string string2 = "Vous pouvez créer votre liste de jeux directement dans le programme" +
            " à l'aide du bouton \"+\", " +
            "ou bien, si vous avez un compte sur le site BoardGameGeek.com, " +
            "et que vous y avez entré votre liste de jeux, vous pouvez l'importer " +
            "à l'aide du bouton \"synchroniser\" (en bas à gauche).";
        string string3 = "La page principale vous permet de rentrer un nombre de joueur " +
            "puis de trouver soit la liste de jeux compatibles, soit d'en sélectionner un au hasard. " +
            "Si le jeu en question nécessite une extension pour jouer au nombre de joueurs souhaité, " +
            "vous en serez informé.";
        string string4 = "Vous pouvez également afficher la liste de tous vos jeux en cliquant sur le bouton " +
            "correspondant dans la marge.";
        string string5 = "Vous pouvez modifier ou supprimer les jeux à votre convenance (nombre de joueurs," +
            " éditeurs, genre, ajouter/supprimer une extension...) sans affecter votre liste sur BGG.";
        string string6 = "Il est possible de rajouter des critères de recherche pour les jeux, " +
            "vous pouvez demander des jeux par durée sur la page principale, " +
            "sur la page où s'affiche la liste des jeux vous pouvez sélectionner des critères supplémentaires " +
            "qui s'ajoutent aux critères déjà choisis (nombre de joueurs et durée)." +
            "Pour que cela fonctionne il faut au préalable remplir pour chaque jeu les critères que vous souhaitez, " +
            "ceux-ci s'afficheront ensuite dans la liste.";
        string string7 = "Vous pouvez également renseigner le nom de l'éditeur pour chaque jeu.";
        string string8 = "Les informations sont enregistrées en local dans votre ordinateur, sous forme de fichiers .bin " +
            "que vous trouverez dans le dossier nommé \"binLists\" situé à l'emplacement du .exe du programme.";
        string string9 = "Vous pouvez supprimer toute votre collection en effaçant les trois fichiers .bin " +
            "situés dans ce dossier.";
        string string10 = "Maintenant il ne vous reste plus qu'à jouer :) ";
        string string11 = "Ce programme a été codé de manière totalement amateure, il est possible qu'il ne soit pas du tout" +
            " optimisé et que malgré ma vigilance certains bugs persistent, je m'en excuse par avance.";
    }


}
