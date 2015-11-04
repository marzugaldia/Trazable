using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Trazable.Engine.Parsing.Rules;

namespace Trazable.Engine.Parsing.Grammars
{
    /// <summary>
    /// Base grammar, with the primitive rules.
    /// </summary>
    public class BaseGrammar
    {
        #region - Methods -

        /// <summary>
        /// Adds a node rule from a rule. A node rule is a rule that creates a node token.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public static Rule Node(Rule rule)
        {
            return new NodeRule(rule);
        }

        /// <summary>
        /// Adds a recursive rule from a function of rule.
        /// </summary>
        /// <param name="ruleFunction">The rule function.</param>
        /// <returns></returns>
        public static Rule Recursive(Func<Rule> ruleFunction)
        {
            return new RecursiveRule(ruleFunction);
        }

        /// <summary>
        /// Adds an At Rule from a given rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public static Rule At(Rule rule)
        {
            return new AtRule(rule);
        }

        /// <summary>
        /// Adds a sequence rule from a rule list.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns></returns>
        public static Rule Sequence(params Rule[] rules)
        {
            return new SequenceRule(rules);
        }

        /// <summary>
        /// Adds a conditional rule from a rule list.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns></returns>
        public static Rule Condition(params Rule[] rules)
        {
            return new IfRule(rules);
        }

        /// <summary>
        /// The end rule.
        /// </summary>
        public static Rule End = new EndRule();

        /// <summary>
        /// Adds a not rule from a rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public static Rule Not(Rule rule)
        {
            return new NotRule(rule);
        }

        /// <summary>
        /// Adds a zero or more rule from a rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public static Rule ZeroOrMore(Rule rule)
        {
            return new RepeatRule(rule);
        }

        /// <summary>
        /// Adds a one or more rule from a rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public static Rule OneOrMore(Rule rule)
        {
            return new WhileRule(rule);
        }

        /// <summary>
        /// Adds an optional rule from a rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public static Rule Opt(Rule rule)
        {
            return new OptRule(rule);
        }

        /// <summary>
        /// Adds a match string rule from a string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static Rule MatchString(string s)
        {
            return new StringRule(s);
        }

        /// <summary>
        /// Adds a match character rule for a predicate of char.
        /// </summary>
        /// <param name="charPredicate">The character predicate.</param>
        /// <returns></returns>
        public static Rule MatchChar(Predicate<char> charPredicate)
        {
            return new CharacterRule(charPredicate);
        }

        /// <summary>
        /// Adds a match character rule from a character.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        public static Rule MatchChar(char c)
        {
            return MatchChar(x => x == c).SetName(c.ToString());
        }

        /// <summary>
        /// Adds a match regular expression rule from a regular epxression.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <returns></returns>
        public static Rule MatchRegex(Regex regex)
        {
            return new RegexRule(regex);
        }

        /// <summary>
        /// Adds a match character rule from a string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">The charset cannot be empty</exception>
        public static Rule CharSet(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentException("The charset cannot be empty");
            }
            return MatchChar(c => s.Contains(c)).SetName(string.Format("[{0}]", s));
        }

        /// <summary>
        /// Adds a match character rule from a range of characters.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static Rule CharRange(char a, char b)
        {
            return MatchChar(c => (c >= a) && (c <= b)).SetName(string.Format("[{0}..{1}]", a, b));
        }

        /// <summary>
        /// Adds a match character rule for excluding the characters of a string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">The charset cannot be empty</exception>
        public static Rule ExceptCharSet(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentException("The charset cannot be empty");
            }
            return MatchChar(c => !s.Contains(c)).SetName(String.Format("[{0}]", s));
        }

        /// <summary>
        /// Adds a match character rule matching any character.
        /// </summary>
        public static Rule AnyChar = MatchChar(c => true).SetName(".");

        /// <summary>
        /// Adds new zero or more rule of a sequence for not matching a rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">The rule cannot be null</exception>
        public static Rule AdvanceWhileNot(Rule rule)
        {
            if (rule == null)
            {
                throw new ArgumentNullException("The rule cannot be null");
            }
            return ZeroOrMore(Sequence(Not(rule), AnyChar));
        }

        /// <summary>
        /// Adds a new match regular expression rule for a string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">The pattern cannot be empty</exception>
        public static Rule Pattern(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentException("The pattern cannot be empty");
            }
            return MatchRegex(new Regex(s));
        }

        /// <summary>
        /// Initializes the grammar.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <exception cref="System.Exception">Unexpected null rule</exception>
        public static void InitGrammar(Type type)
        {
            foreach (FieldInfo field in type.GetFields())
            {
                if (field.FieldType.Equals(typeof(Rule)))
                {
                    Rule rule = field.GetValue(null) as Rule;
                    if (rule == null)
                        throw new Exception("Unexpected null rule");
                    rule.Name = field.Name;
                }
            }
        }

        #endregion
    }
}
