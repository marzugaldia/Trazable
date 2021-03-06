﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Trazable.Engine.Parsing;
using Trazable.Engine.Parsing.Grammars;
using Trazable.Engine.Utils;

namespace Trazable.Engine.System
{
    public class UserScript
    {
        public string Name { get; set; }

        public string Script { get; set; }


        public OptSystem Process()
        {
            IList<Token> tokens = JavascriptGrammar.Script.Parse(this.Script);
            VarSystem varSystem = new VarSystem(this.Name);
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
                                    processed = true;
                                }
                                else if (subtoken.Tokens[0].Text.ToLower().Equals("output"))
                                {
                                    VarType type = VarSystem.StrToVarType(subtoken.Tokens[1].Tokens[0].Text);
                                    if (type == VarType.Unknown)
                                    {
                                        throw new Exception("Unexpected variable type: " + token.Text);
                                    }
                                    varSystem.AddVariable(VarDirection.Output, type, subtoken.Tokens[1].Tokens[1].Text, StringUtil.Unquote(subtoken.Tokens[1].Tokens[2].Text));
                                    processed = true;
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
                        varSystem.Sentences.Add(token.Text);
                    }
                }

            }
            return varSystem.Compile();
        }
    }
}