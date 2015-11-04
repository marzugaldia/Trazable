using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trazable.Engine.Utils;

namespace Trazable.Bed
{
    public class VarSystem
    {
        public List<VarDef> Variables { get; private set; }

        private Dictionary<string, VarDef> variableMap = new Dictionary<string, VarDef>();

        public VarSystem()
        {
            this.Variables = new List<VarDef>();
        }

        public VarDef AddInput(string typename, string name, string text)
        {
            string lowtype = typename.ToLower();
            VarType type = VarType.Unknown;
            switch (lowtype)
            {
                case "sca":
                    type = VarType.SCA;
                    break;
                case "pam":
                    type = VarType.PAM;
                    break;
                case "ad":
                    type = VarType.AD;
                    break;
                case "double":
                    type = VarType.Double;
                    break;
                case "real":
                    type = VarType.Double;
                    break;
                default: throw new Exception("Unknown variable type: " + typename);
            }
            VarDef varDef = new VarDef();
            varDef.IsOutput = false;
            varDef.VarType = type;
            varDef.Name = name;
            varDef.Text = StringUtil.Unquote(text);
            this.Variables.Add(varDef);
            return varDef;
        }
    }
}
