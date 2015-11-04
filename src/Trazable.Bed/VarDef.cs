using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trazable.Bed
{
    public class VarDef
    {
        public bool IsOutput { get; set; }

        public VarType VarType { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

    }
}
