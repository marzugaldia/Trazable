using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Trazable.Engine.Base;
using Trazable.Engine.Utils;

namespace Trazable.Engine.System
{
    public class VarSystem
    {

        private Dictionary<string, VarDef> variablesMap = new Dictionary<string, VarDef>();

        public string Name { get; set; }

        public IList<VarDef> Variables { get; private set; }

        public IList<string> Sentences { get; private set; }

        public VarSystem(string name)
        {
            this.Name = name;
            this.Variables = new List<VarDef>();
            this.Sentences = new List<string>();

        }

        public VarDef AddVariable(VarDef variable)
        {
            if (this.variablesMap.ContainsKey(variable.Name))
            {
                int i = this.Variables.IndexOf(this.variablesMap[variable.Name]);
                this.Variables[i] = variable;
                this.variablesMap[variable.Name] = variable;
            }
            else
            {
                this.Variables.Add(variable);
                this.variablesMap.Add(variable.Name, variable);
            }
            return variable;
        }

        public VarDef AddVariable(VarDirection direction, VarType type, string name, string text, params string[] parameters)
        {
            return this.AddVariable(new VarDef(direction, type, name, text, parameters));
        }

        public static VarType StrToVarType(string s)
        {
            s = s.ToLower();
            switch (s)
            {
                case "sca": return VarType.SCA;
                case "pam": return VarType.PAM;
                case "ad": return VarType.AD;
                case "double":
                case "real":
                    return VarType.Double;
                default: return VarType.Unknown;
            }
        }

        public VarDef Get(string name)
        {
            if (this.variablesMap.ContainsKey(name))
            {
                return this.variablesMap[name];
            }
            return null;
        }

        public VarDef Set(string name, object value)
        {
            if (this.variablesMap.ContainsKey(name))
            {
                VarDef result = this.variablesMap[name];
                result.Value = value;
                return result;
            }
            return null;
        }

        public T GetAs<T>(string name)
        {
            return (T)this.Get(name).Value;
        }

        public string GenerateCode()
        {
            string className = StringUtil.Pascalize(this.Name);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using Trazable.Engine.Base;");
            sb.AppendLine();
            sb.AppendLine("namespace Trazable.Engine.System");
            sb.AppendLine("{");
            sb.AppendLine("    public class " + className + " : OptSystem");
            sb.AppendLine("    {");
            sb.AppendLine("        public " + className + "(string name)");
            sb.AppendLine("            : base(name)");
            sb.AppendLine("        {");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        public override void Initialize()");
            sb.AppendLine("        {");
            foreach (VarDef variable in this.Variables)
            {
                sb.Append("            vars.AddVariable(VarDirection.");
                sb.Append(variable.Direction.ToString());
                sb.Append(", VarType.");
                sb.Append(variable.Type.ToString());
                sb.Append(", \"");
                sb.Append(variable.Name);
                sb.Append("\", \"");
                sb.Append(variable.Text);
                sb.Append("\"");
                foreach (string parameter in variable.Parameters)
                {
                    sb.Append(", \"");
                    sb.Append(parameter);
                    sb.Append("\"");
                }
                sb.AppendLine(");");
            }
            sb.AppendLine("        }");

            sb.AppendLine("        public override void Execute()");
            sb.AppendLine("        {");
            foreach (VarDef variable in this.Variables)
            {
                if (variable.Type == VarType.Double)
                {
                    sb.Append("            double ");
                }
                else
                {
                    sb.Append("            OpticalRepresentation ");
                }
                sb.Append(variable.Name);
                sb.Append(" = ");
                if (variable.Direction == VarDirection.Input)
                {
                    sb.Append("vars.GetAs<");
                    if (variable.Type == VarType.Double)
                    {
                        sb.Append("double");
                    } else
                    {
                        sb.Append("OpticalRepresentation");
                    }
                    sb.Append(">(\"");
                    sb.Append(variable.Name);
                    sb.AppendLine("\");");
                }
                else
                {
                    if (variable.Type == VarType.Double)
                    {
                        sb.AppendLine("0.0;");
                    }
                    else
                    {
                        sb.AppendLine("null;");
                    }
                }
            }
            // execution
            foreach (string sentence in this.Sentences)
            {
                sb.Append("            ");
                sb.Append(sentence);
                sb.AppendLine(";");
            }
            // finalization
            foreach (VarDef variable in this.Variables)
            {
                if (variable.Direction == VarDirection.Output)
                {
                    sb.Append("            vars.Set(\"");
                    sb.Append(variable.Name);
                    sb.Append("\", ");
                    sb.Append(variable.Name);
                    sb.AppendLine(");");
                }
            }
            sb.AppendLine("        }");

            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        public OptSystem Compile()
        {
            ScriptCompiler compiler = new ScriptCompiler(this.Name);
            compiler.AddAssembly("Trazable.Engine.dll");
            compiler.ScriptCode = this.GenerateCode();
            compiler.Compile();
            if (compiler.CompilerErrors.Count > 0)
            {
                throw new Exception("Error compiling");
            }
            Type type = compiler.GetTypeByName(StringUtil.Pascalize(this.Name));
            return (OptSystem)Activator.CreateInstance(type, this.Name);
        }

    }
}




//        // Finalization
//        vars.Set("ev", ev);
//        vars.Set("theoricalIOL", theoricalIOL);
//        vars.Set("resultSpectacle", resultSpectacle);
//    }

