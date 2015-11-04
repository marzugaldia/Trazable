using System.Collections.Generic;

namespace Trazable.Engine.System
{
    public class VarDef
    {
        public VarDirection Direction { get; set; }

        public VarType Type { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public object Value { get; set; }

        public List<string> Parameters { get; private set; }

        public VarDef(VarDirection direction, VarType type, string name, string text, params string[] parameters)
        {
            this.Parameters = new List<string>();
            this.Direction = direction;
            this.Type = type;
            this.Name = name;
            this.Text = text;
            this.Value = null;
            if (parameters != null)
            {
                foreach (string parameter in parameters)
                {
                    this.Parameters.Add(parameter);
                }
            }
        }
    }
}
