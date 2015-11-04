using Trazable.Engine.Parsing.Rules;

namespace Trazable.Engine.Parsing.Grammars
{
    /// <summary>
    /// Grammar for Javascript Language
    /// </summary>
    public class JavascriptGrammar : JsonGrammar
    {
        #region - Static constructor -

        /// <summary>
        /// Initializes the <see cref="JavascriptGrammar"/> class.
        /// </summary>
        static JavascriptGrammar()
        {
            InitGrammar(typeof(JavascriptGrammar));
        }

        #endregion

        #region - Rules -

        /// <summary>
        /// Rule for a recursive expression.
        /// </summary>
        public static Rule RecExpr = Recursive(() => Expr);

        /// <summary>
        /// Rule for a recursive statement.
        /// </summary>
        public static Rule RecStatement = Recursive(() => Statement);

        /// <summary>
        /// Rule for a Literal.
        /// </summary>
        public static Rule Literal = Recursive(() => String | Integer | Float | Object | Array | True | False | Null);

        /// <summary>
        /// Rule for an identifier.
        /// </summary>
        public new static Rule Identifier = Node(CommonGrammar.Identifier);

        /// <summary>
        /// Rule for the name of a pair.
        /// </summary>
        public static Rule PairName = Identifier | DoubleQuotedString | SingleQuotedString;

        /// <summary>
        /// Rule for a pair name : value.
        /// </summary>
        public new static Rule Pair = Node(PairName + WS + CharToken(':') + RecExpr + WS);

        /// <summary>
        /// Rule for an array.
        /// </summary>
        public new static Rule Array = Node(CharToken('[') + CommaDelimited(RecExpr) + WS + CharToken(']'));

        /// <summary>
        /// Rule for an object.
        /// </summary>
        public new static Rule Object = Node(CharToken('{') + CommaDelimited(Pair) + WS + CharToken('}'));

        /// <summary>
        /// Rule for a parameter list.
        /// </summary>
        public static Rule ParamList = Node(Parenthesize(CommaDelimited(Identifier + WS)));

        /// <summary>
        /// Rule for a named function.
        /// </summary>
        public static Rule NamedFunc = Node(Keyword("function") + Identifier + WS + ParamList + RecStatement);

        /// <summary>
        /// Rule for an anonymous function.
        /// </summary>
        public static Rule AnonFunc = Node(Keyword("function") + ParamList + RecStatement);

        /// <summary>
        /// Rule for function.
        /// </summary>
        public static Rule Function = NamedFunc | AnonFunc;

        /// <summary>
        /// Rule for an argument list.
        /// </summary>
        public static Rule ArgList = Node(CharToken('(') + CommaDelimited(RecExpr) + CharToken(')'));

        /// <summary>
        /// Rule for the index of an array.
        /// </summary>
        public static Rule Index = Node(CharToken('[') + RecExpr + CharToken(']'));

        /// <summary>
        /// Rule for a field of an object.
        /// </summary>
        public static Rule Field = Node(CharToken('.') + Identifier);

        /// <summary>
        /// Rule for a prefix operator.
        /// </summary>
        public static Rule PrefixOp = Node(MatchStringSet("! - ~"));

        /// <summary>
        /// Rule for an expression between parenthesis.
        /// </summary>
        public static Rule ParenExpr = Node(CharToken('(') + RecExpr + WS + CharToken(')'));

        /// <summary>
        /// Rule for a new expression.
        /// </summary>
        public static Rule NewExpr = Node(Keyword("new") + Recursive(() => PostfixExpr));

        /// <summary>
        /// Rule for a leaf expression.
        /// </summary>
        public static Rule LeafExpr = ParenExpr | NewExpr | Function | Literal | Identifier;

        /// <summary>
        /// Rule for a prefix expression.
        /// </summary>
        public static Rule PrefixExpr = Node(PrefixOp + Recursive(() => PrefixOrLeafExpr));

        /// <summary>
        /// Rule for a prefix or leaf expression.
        /// </summary>
        public static Rule PrefixOrLeafExpr = PrefixExpr | LeafExpr;

        /// <summary>
        /// Rule for a postfix operand.
        /// </summary>
        public static Rule PostfixOp = Field | Index | ArgList;

        /// <summary>
        /// Rule for a postfix expression.
        /// </summary>
        public static Rule PostfixExpr = Node(PrefixOrLeafExpr + WS + OneOrMore(PostfixOp + WS));

        /// <summary>
        /// Rule for an unary expression.
        /// </summary>
        public static Rule UnaryExpr = PostfixExpr | PrefixOrLeafExpr;

        /// <summary>
        /// Rule for a binary operator.
        /// </summary>
        public static Rule BinaryOp = Node(MatchStringSet("<= >= == != << >> && || < > & | + - * % /"));

        /// <summary>
        /// Rule for a binary expression.
        /// </summary>
        public static Rule BinaryExpr = Node(UnaryExpr + WS + BinaryOp + WS + RecExpr);

        /// <summary>
        /// Rule for an assignation operator.
        /// </summary>
        public static Rule AssignOp = Node(MatchStringSet("&&= ||= >>= <<= += -= *= %= /= &s= |= ^= ="));

        /// <summary>
        /// Rule for an assignation expression.
        /// </summary>
        public static Rule AssignExpr = Node((Identifier | PostfixExpr) + WS + AssignOp + WS + RecExpr);

        /// <summary>
        /// Rule for a tertiary operator expression.
        /// </summary>
        public static Rule TertiaryExpr = Node((AssignExpr | BinaryExpr | UnaryExpr) + WS + CharToken('?') + RecExpr + CharToken(':') + RecExpr + WS);

        /// <summary>
        /// Rule for an expression.
        /// </summary>
        public static Rule Expr = Node((TertiaryExpr | AssignExpr | BinaryExpr | UnaryExpr) + WS);

        /// <summary>
        /// Rule for a block of code.
        /// </summary>
        public static Rule Block = Node(CharToken('{') + ZeroOrMore(RecStatement) + CharToken('}'));

        /// <summary>
        /// Rule for a variable declaration.
        /// </summary>
        public static Rule VarDecl = Node(Keyword("var") + Identifier + WS + Opt(Eq + Expr) + Eos);

        /// <summary>
        /// Rule for the while sequence.
        /// </summary>
        public static Rule While = Node(Keyword("while") + Parenthesize(Expr) + RecStatement);

        /// <summary>
        /// Rule for the for sequence.
        /// </summary>
        public static Rule For = Node(Keyword("for") + Parenthesize(VarDecl + Expr + WS + Eos + Expr + WS) + RecStatement);

        /// <summary>
        /// Rule for the foreach sequence.
        /// </summary>
        public static Rule Foreach = Node(Keyword("foreach") + Parenthesize(Identifier) + RecStatement);

        /// <summary>
        /// Rule for the else sequence.
        /// </summary>
        public static Rule Else = Node(Keyword("else") + RecStatement);

        /// <summary>
        /// Rule for the if expression.
        /// </summary>
        public static Rule If = Node(Keyword("if") + Parenthesize(Expr) + RecStatement + Opt(Else));

        /// <summary>
        /// Rule for an expression statement.
        /// </summary>
        public static Rule ExprStatement = Node(Expr + WS + Eos);

        /// <summary>
        /// Rule for the return.
        /// </summary>
        public static Rule Return = Node(Keyword("return") + Opt(Expr) + WS + Eos);

        /// <summary>
        /// Rule for an empty sentence.
        /// </summary>
        public static Rule Empty = Node(WS + Eos);

        /// <summary>
        /// Rule for an statement.
        /// </summary>
        public static Rule Statement = Block | For | Foreach | While | If | Return | VarDecl | ExprStatement | Empty;

        /// <summary>
        /// Rule for a script.
        /// </summary>
        public static Rule Script = Node(ZeroOrMore(Statement) + WS + End);

        #endregion
    }
}
