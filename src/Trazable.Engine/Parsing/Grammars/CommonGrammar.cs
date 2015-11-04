using System;
using System.Linq;
using Trazable.Engine.Parsing.Rules;

namespace Trazable.Engine.Parsing.Grammars
{
    /// <summary>
    /// Common grammar rules.
    /// </summary>
    public class CommonGrammar : BaseGrammar
    {

        #region - Static Constructor -

        /// <summary>
        /// Initializes the <see cref="CommonGrammar"/> class.
        /// </summary>
        static CommonGrammar()
        {
            InitGrammar(typeof(CommonGrammar));
        }

        #endregion

        #region - Rules -

        /// <summary>
        /// Rule for matching any string of a given set of strings.
        /// </summary>
        /// <param name="xs">The xs.</param>
        /// <returns></returns>
        public static Rule MatchAnyString(params string[] xs)
        {
            return Condition(xs.Select(x => MatchString(x)).ToArray());
        }

        /// <summary>
        /// Rule for matching any string of a given set of strings separated by spaces.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static Rule MatchStringSet(string s)
        {
            return MatchAnyString(s.Split(' '));
        }

        /// <summary>
        /// Rule for a full comment.
        /// </summary>
        public static Rule FullComment = MatchString("/*") + AdvanceWhileNot(MatchString("*/"));

        /// <summary>
        /// Rule for a line comment.
        /// </summary>
        public static Rule LineComment = MatchString("//") + AdvanceWhileNot(MatchChar('\n'));

        /// <summary>
        /// Rule for the White Spaces pattern.
        /// </summary>
        public static Rule WS = Pattern(@"\s*");

        /// <summary>
        /// Rule for matching a digit
        /// </summary>
        public static Rule Digit = MatchChar(Char.IsDigit);

        /// <summary>
        /// Rule for matching a set of digits.
        /// </summary>
        public static Rule Digits = OneOrMore(Digit);

        /// <summary>
        /// Rule for matching a letter.
        /// </summary>
        public static Rule Letter = MatchChar(Char.IsLetter);

        /// <summary>
        /// Rule for matching a letter or digit.
        /// </summary>
        public static Rule LetterOrDigit = MatchChar(Char.IsLetterOrDigit);

        /// <summary>
        /// Rule for matching the exponent of a float number.
        /// </summary>
        public static Rule E = (MatchChar('e') | MatchChar('E')) + Opt(MatchChar('+') | MatchChar('-'));

        /// <summary>
        /// Rule for a letter or undescore 
        /// </summary>
        public static Rule IdentFirstChar = MatchChar(c => Char.IsLetter(c) || c == '_');

        /// <summary>
        /// Rule for letter, digit or undescore
        /// </summary>
        public static Rule IdentNextChar = MatchChar(c => Char.IsLetterOrDigit(c) || c == '_');

        /// <summary>
        /// Rule for an identifier name.
        /// </summary>
        public static Rule Identifier = IdentFirstChar + ZeroOrMore(IdentNextChar);

        /// <summary>
        /// Rule for the exponent part of a float, including the digits.
        /// </summary>
        public static Rule Exp = E + Digits;

        /// <summary>
        /// Rule for the fraction part of a number.
        /// </summary>
        public static Rule Frac = MatchChar('.') + Digits;

        /// <summary>
        /// Rule for an integer number.
        /// </summary>
        public static Rule Integer = Digits + Not(MatchChar('.'));

        /// <summary>
        /// Rule for a float number.
        /// </summary>
        public static Rule Float = Digits + ((Frac + Opt(Exp)) | Exp);

        /// <summary>
        /// Rule for an hexadecimal digit.
        /// </summary>
        public static Rule HexDigit = Digit | CharRange('a', 'f') | CharRange('A', 'F');

        /// <summary>
        /// Rule for a character and white spaces.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        public static Rule CharToken(char c) { return MatchChar(c) + WS; }

        /// <summary>
        /// Rule for an string and whitespaces.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static Rule StringToken(string s) { return MatchString(s) + WS; }

        /// <summary>
        /// Rule for a comma delimited set of tokens that match another rule.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        public static Rule CommaDelimited(Rule r)
        {
            return Opt(r + (ZeroOrMore(CharToken(',') + r) + Opt(CharToken(','))));
        }

        /// <summary>
        /// Rule for the comma;
        /// </summary>
        public static Rule Comma = CharToken(',');

        /// <summary>
        /// Rule for the End Of Sentence character.
        /// </summary>
        public static Rule Eos = CharToken(';');

        /// <summary>
        /// Rule for the equal character.
        /// </summary>
        public static Rule Eq = CharToken('=');

        /// <summary>
        /// Rule for the dot character.
        /// </summary>
        public static Rule Dot = CharToken('.');

        /// <summary>
        /// Rule for a keyword
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static Rule Keyword(string s)
        {
            return MatchString(s) + Not(LetterOrDigit) + WS;
        }

        /// <summary>
        /// Rule for a rule between parenthesis.
        /// </summary>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        public static Rule Parenthesize(Rule r)
        {
            return CharToken('(') + r + WS + CharToken(')');
        }

        #endregion
    }
}
