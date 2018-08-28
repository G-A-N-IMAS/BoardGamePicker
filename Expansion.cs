using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace BoardGamePicker
{ //Dans BoardGamePicker
    [Serializable]
    public class Expansion : BoardGame
    {
        private bool IsExpansion = false;
        public bool isExpansion
        {
            get
            {
                return IsExpansion;
            }
            set
            {
                IsExpansion = value;
            }
        }
        public Expansion (string title)
        {
            Title = title;
            isExpansion = true;
        }
        public bool ChangesNbOfPlayers { get; protected set; }
        public void changePlayerNb(int min, int max)
        {

            ChangesNbOfPlayers = true;
            SetMinPlayerFromCsv(min);
            SetMaxPlayerFromCsv(max);

        }
    }
}
