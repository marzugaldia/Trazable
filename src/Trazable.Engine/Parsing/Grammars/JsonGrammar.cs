using Trazable.Engine.Parsing.Rules;

namespace Trazable.Engine.Parsing.Grammars
{
    /// <summary>
    /// Grammar for a JSON message.
    /// </summary>
    public class JsonGrammar : CommonGrammar
    {
        #region - Static Constructors -

        /// <summary>
        /// Initializes the <see cref="JsonGrammar"/> class.
        /// </summary>
        static JsonGrammar()
        {
            InitGrammar(typeof(JsonGrammar));
        }

        #endregion

        #region - Rules -

        /// <summary>
        /// Rule for an integer.
        /// </summary>
        new public static Rule Integer = Node(CommonGrammar.Integer);

        /// <summary>
        /// Rule for a float.
        /// </summary>
        new public static Rule Float = Node(CommonGrammar.Float);

        /// <summary>
        /// Rule for true.
        /// </summary>
        public static Rule True = Node(MatchString("true"));

        /// <summary>
        /// Rule for false.
        /// </summary>
        public static Rule False = Node(MatchString("false"));

        /// <summary>
        /// Rule for null.
        /// </summary>
        public static Rule Null = Node(MatchString("null"));

        /// <summary>
        /// Rule for an unicode character.
        /// </summary>
        public static Rule UnicodeChar = MatchString("\\u") + HexDigit + HexDigit + HexDigit + HexDigit;

        /// <summary>
        /// Rule for a control character.
        /// </summary>
        public static Rule ControlChar = MatchChar('\\') + CharSet("\"\'\\/bfnt");

        /// <summary>
        /// Rule for a double quoted string.
        /// </summary>
        public static Rule DoubleQuotedString = Node(MatchChar('"') + ZeroOrMore(UnicodeChar | ControlChar | ExceptCharSet("\"\\")) + MatchChar('"'));

        /// <summary>
        /// Rule for a single quoted string.
        /// </summary>
        public static Rule SingleQuotedString = Node(MatchChar('\'') + ZeroOrMore(UnicodeChar | ControlChar | ExceptCharSet("'\\")) + MatchChar('\''));

        /// <summary>
        /// Rule for a string.
        /// </summary>
        public static Rule String = Node(DoubleQuotedString | SingleQuotedString);

        /// <summary>
        /// Rule for a number.
        /// </summary>
        public static Rule Number = Float | Integer;

        /// <summary>
        /// Rule for a value.
        /// </summary>
        public static Rule Value = Node(Recursive(() => String | Number | Object | Array | True | False | Null));

        /// <summary>
        /// Rule for a pair name: value.
        /// </summary>
        public static Rule Pair = Node(DoubleQuotedString + WS + CharToken(':') + Value + WS);

        /// <summary>
        /// Rule for an array.
        /// </summary>
        public static Rule Array = Node(CharToken('[') + CommaDelimited(Value) + WS + CharToken(']'));

        /// <summary>
        /// Rule for an object.
        /// </summary>
        public static Rule Object = Node(CharToken('{') + CommaDelimited(Pair) + WS + CharToken('}'));

        #endregion
    }
}
