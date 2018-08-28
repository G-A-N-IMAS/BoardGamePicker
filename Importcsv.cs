using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace BoardGamePicker
{
    [Serializable]
    public class Importcsv
    {
        public string objectname { get; set; }
       
        public int minplayers { get; set; }
        public int maxplayers { get; set; }
       
        public int maxplaytime { get; set; }
        public int minplaytime { get; set; }
       
        public string itemtype { get; set; } // important, extension ou standalone
       
    }
}
