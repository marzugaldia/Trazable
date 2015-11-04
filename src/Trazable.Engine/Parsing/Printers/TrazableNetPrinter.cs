using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trazable.Engine.Parsing.Grammars;

namespace Trazable.Engine.Parsing.Printers
{
    /// <summary>
    /// Code Printer for javascript.
    /// </summary>
    public class TrazableNetPrinter : CodePrinter
    {

        #region - Methods -

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public static string ToString(Token token)
        {
            return new TrazableNetPrinter().Print(token).ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public static string ToString(string script)
        {
            return ToString(JavascriptGrammar.Script.Parse(script)[0]);
        }

        /// <summary>
        /// Prints the specified n.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Unrecognized node type  + n.Name</exception>
        public override CodePrinter Print(Token n)
        {
            switch (n.Name)
            {
                case "Script":
                    return Print(n.Tokens);
                case "Statement":
                    return Print(n[0]);
                case "Empty":
                    return Print(";\n");
                case "Return":
                    return (n.NodeCount > 0)
                        ? Print("return ").Print(n[0]).Print(";\n")
                        : Print("return;\n");
                case "ExprStatement":
                    return Print(n[0]).Print(";\n");
                case "If":
                    Print("if (").Print(n[0]).Print(")").Indent().Print(n[1]).Unindent();
                    return n.NodeCount > 2
                        ? Print(n[2])
                        : this;
                case "Else":
                    return Print("else").Indent().Print(n[0]).Unindent();
                case "For":
                    return Print("for (").Print(n[0]).Print(";").Print(n[1]).Print(";").Print(n[2]).Print(") ").Indent().Print(n[3]).Unindent();
                case "While":
                    return Print("while (").Print(n[0]).Print(") ").Indent().Print(n[1]).Unindent();
                case "VarDecl":
                    return (n.NodeCount > 1)
                        ? Print("var ").Print(n[0]).Print(" = ").Print(n[1]).Print(";\n")
                        : Print("var ").Print(n[0]).Print(";\n");
                case "Block":
                    if (n.NodeCount > 0)
                    {
                        Print("{").Indent();
                        foreach (var n2 in n.Tokens.Take(n.NodeCount - 1))
                            PrintLine(n2);
                        return Print(n[n.NodeCount - 1]).Unindent().Print("}");
                    }
                    else
                    {
                        return Print("{}");
                    }
                case "Expr":
                    return Print(n[0]);
                case "TertiaryExpr":
                    return (n.NodeCount == 3)
                        ? Print(n[0]).Print(" ? ").Print(n[1]).Print(" : ").Print(n[2])
                        : Print(n[0]);
                case "AssignOp":
                case "BinaryOp":
                    return Print(" ").Print(n.Text).Print(" ");
                case "PostfixOp":
                case "PrefixOp":
                    return Print(n.Text);
                case "BinaryExpr":
                case "AssignExpr":
                case "PostfixExpr":
                    return Print(n.Tokens);
                case "NewExpr":
                    return Print("new ").Print(n.Tokens);
                case "ParenExpr":
                    return Print("(").Print(n[0]).Print(")");
                case "Field":
                    return Print(".").Print(n.Tokens);
                case "Index":
                    return Print("[").Print(n.Tokens).Print("]");
                case "ArgList":
                    return Print("(").Print(n.Tokens, ", ").Print(")");
                case "NamedFunc":
                    return Print("function ").Print(n[0]).Print(n[1]).Print(" ").Indent().Print(n[2]).Unindent();
                case "AnonFunc":
                    return Print("function ").Print(n[0]).Print(" ").Indent().Print(n[1]).Unindent();
                case "ParamList":
                    return Print("(").Print(n.Tokens, ", ").Print(")");
                case "Object":
                    return Print("{").Indent().Print(n.Tokens, ", ").Unindent().Print("}");
                case "Array":
                    return Print("[").Indent().Print(n.Tokens, ", ").Unindent().Print("]");
                case "Pair":
                    return Print(n[0]).Print(" : ").Print(n[1]);
                case "PairName":
                    return Print(n[0]);
                case "Literal":
                    return Print(n[0]);
                case "Identifier":
                    string s = n.Text.ToLower();
                    switch (s)
                    {
                        case "input": return Print("INPUT");
                        case "output": return Print("OUTPUT");
                        case "sca": return Print("ParameterType.SCA");
                        case "pam": return Print("ParameterType.PAM");
                        case "ad": return Print("ParameterType.AD");
                        case "double": return Print("ParameterType.Real");
                        case "real": return Print("ParameterType.Real");
                        default: return Print(n.Text);
                    }
                case "String":
                case "Integer":
                case "Float":
                case "True":
                case "False":
                case "Null":
                    return Print(n.Text);
                default:
                    throw new Exception("Unrecognized node type " + n.Name);
            }
        }

        #endregion
    }

}
