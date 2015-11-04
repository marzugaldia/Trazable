using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trazable.Engine.Parsing.Rules;

namespace Trazable.Engine.Parsing.Grammars
{
    public class OpticalGrammar : JsonGrammar
    {
        #region - Static Constructors -

        /// <summary>
        /// Initializes the <see cref="MiniGrammar"/> class.
        /// </summary>
        static OpticalGrammar()
        {
            InitGrammar(typeof(OpticalGrammar));
        }

        #endregion

        #region - Rules -

        public static Rule RecExpr = Recursive(() => Expr);

        public static Rule RecStatement = Recursive(() => Statement);

        public static Rule Literal = Recursive(() => String | Integer | Float | Object | Array | True | False | Null);

        public static Rule ParamList = Node(Parenthesize(CommaDelimited(Identifier + WS)));

        public static Rule NamedFunc = Node(Keyword("function") + Identifier + WS + ParamList + RecStatement);

        public static Rule Function = NamedFunc;

        public static Rule Empty = Node(WS + Eos);

        public static Rule BinaryOp = Node(MatchStringSet("+ - * /"));

        public static Rule PrefixOp = Node(MatchStringSet("! - ~"));

        public static Rule NewExpr = Node(Keyword("new") + Recursive(() => PostfixExpr));

        public static Rule ParenExpr = Node(CharToken('(') + RecExpr + WS + CharToken(')'));

        public static Rule LeafExpr = ParenExpr | NewExpr | Function | Literal | Identifier;

        public static Rule PrefixExpr = Node(PrefixOp + Recursive(() => PrefixOrLeafExpr));

        public static Rule Field = Node(CharToken('.') + Identifier);

        public static Rule Index = Node(CharToken('[') + RecExpr + CharToken(']'));

        public static Rule ArgList = Node(CharToken('(') + CommaDelimited(RecExpr) + CharToken(')'));

        public static Rule PostfixOp = Field | Index | ArgList;

        public static Rule PrefixOrLeafExpr = PrefixExpr | LeafExpr;

        public static Rule PostfixExpr = Node(PrefixOrLeafExpr + WS + OneOrMore(PostfixOp + WS));

        public static Rule UnaryExpr = PostfixExpr | PrefixOrLeafExpr;

        public static Rule BinaryExpr = Node(UnaryExpr + WS + BinaryOp + WS + RecExpr);

        public static Rule AssignOp = Node(MatchStringSet("="));

        public static Rule AssignExpr = Node((Identifier | PostfixExpr) + WS + AssignOp + WS + RecExpr);

        public static Rule TertiaryExpr = Node((AssignExpr | BinaryExpr | UnaryExpr) + WS + CharToken('?') + RecExpr + CharToken(':') + RecExpr + WS);

        public static Rule Expr = Node((TertiaryExpr | AssignExpr | BinaryExpr | UnaryExpr) + WS);

        public static Rule ExprStatement = Node(Expr + WS + Eos);

        //public static Rule Statement = Block | For | Foreach | While | If | Return | VarDecl | ExprStatement | Empty;

        public static Rule Statement = ExprStatement | Empty;

        public static Rule Script = Node(ZeroOrMore(Statement) + WS + End);

        #endregion
    }
}
