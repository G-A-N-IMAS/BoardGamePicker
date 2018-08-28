using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGamePicker
{
    [Serializable]
    public partial class Style
    {
        public string style { get; set; }

        public override string ToString()
        {
            return style;
        }

    }
}
