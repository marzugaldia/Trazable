using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Trazable.Engine.Parsing;
using Trazable.Engine.Parsing.Grammars;
using Trazable.Engine.System;
using Trazable.Engine.Utils;

namespace Trazable.Bed
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IList<Token> tokens = JavascriptGrammar.Script.Parse(this.richTextBox1.Text);
            VarSystem varSystem = new VarSystem("Test");
            if (tokens != null && tokens.Count == 1)
            {
                tokens = tokens[0].Tokens;
                foreach (Token token in tokens)
                {
                    bool processed = false;
                    if (token.Name.Equals("ExprStatement") && (token.NodeCount == 1))
                    {
                        Token subtoken = token.Tokens[0];
                        if (subtoken.Name.Equals("Expr") && (token.NodeCount == 1))
                        {
                            subtoken = subtoken.Tokens[0];
                            if (subtoken.Name.Equals("PostfixExpr"))
                            {
                                if (subtoken.Tokens[0].Text.ToLower().Equals("input"))
                                {
                                    Token parameters = subtoken.Tokens[1];
                                    VarType type = VarSystem.StrToVarType(parameters.Tokens[0].Text);
                                    if (type == VarType.Unknown)
                                    {
                                        throw new Exception("Unexpected variable type: " + token.Text);
                                    }
                                    VarDef variable = varSystem.AddVariable(VarDirection.Input, type, subtoken.Tokens[1].Tokens[1].Text, StringUtil.Unquote(subtoken.Tokens[1].Tokens[2].Text));
                                    if (parameters.NodeCount > 3)
                                    {
                                        for (int i = 3; i < parameters.NodeCount; i++)
                                        {
                                            variable.Parameters.Add(parameters.Tokens[i].Text);
                                        }

                                    }
                                }
                                else if (subtoken.Tokens[0].Text.ToLower().Equals("output"))
                                {
                                    VarType type = VarSystem.StrToVarType(subtoken.Tokens[1].Tokens[0].Text);
                                    if (type == VarType.Unknown)
                                    {
                                        throw new Exception("Unexpected variable type: " + token.Text);
                                    }
                                    varSystem.AddVariable(VarDirection.Output, type, subtoken.Tokens[1].Tokens[1].Text, StringUtil.Unquote(subtoken.Tokens[1].Tokens[2].Text));
                                }
                            }
                            else if (subtoken.Name.Equals("AssignExpr"))
                            {
                                string s = subtoken.Text;
                                s = Regex.Replace(s, Regex.Escape(".propinv("), ".PropagateReverse(", RegexOptions.IgnoreCase);
                                s = Regex.Replace(s, Regex.Escape(".prop("), ".Propagate(", RegexOptions.IgnoreCase);
                                s = Regex.Replace(s, Regex.Escape(".ref("), ".Refracte(", RegexOptions.IgnoreCase);
                                s = Regex.Replace(s, Regex.Escape(".refinv("), ".RefracteReverse(", RegexOptions.IgnoreCase);
                                varSystem.Sentences.Add(s);
                                processed = true;
                            }
                        }
                    }
                    if (!processed)
                    {
                    }
                }
            }
            OptSystem system = varSystem.Compile();
            FormOptSystem optForm = new FormOptSystem(system);
            optForm.ShowDialog();
        }
    }
}
