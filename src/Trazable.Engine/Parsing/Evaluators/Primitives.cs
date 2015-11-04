using System;
using System.Reflection;

namespace Trazable.Engine.Parsing.Evaluators
{
    /// <summary>
    /// Helper for primitives.
    /// </summary>
    public static class Primitives
    {
        #region - Methods -

        /// <summary>
        /// Diective for the noop (empty function for no operation).
        /// </summary>
        public static void noop() { }

        /// <summary>
        /// Add operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns>A+B</returns>
        public static dynamic add(dynamic a, dynamic b) { return a + b; }

        /// <summary>
        /// Subtract operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns>A-B</returns>
        public static dynamic subtract(dynamic a, dynamic b) { return a - b; }

        /// <summary>
        /// Multiply operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns>A-B</returns>
        public static dynamic multiply(dynamic a, dynamic b) { return a * b; }

        /// <summary>
        /// Divide operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns></returns>
        public static dynamic divide(dynamic a, dynamic b) { return a / b; }

        /// <summary>
        /// Modulo operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns></returns>
        public static dynamic modulo(dynamic a, dynamic b) { return a % b; }

        /// <summary>
        /// Negation operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <returns>-A</returns>
        public static dynamic negative(dynamic a) { return -a; }

        /// <summary>
        /// Shift Left operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns>A shift_left B</returns>
        public static dynamic shift_left(dynamic a, dynamic b) { return a << b; }

        /// <summary>
        /// Shift Right operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns>A shift_right B</returns>
        public static dynamic shift_right(dynamic a, dynamic b) { return a >> b; }

        /// <summary>
        /// Greater than operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns>A gt B</returns>
        public static dynamic gt(dynamic a, dynamic b) { return a > b; }

        /// <summary>
        /// Less than operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns>A lt B</returns>
        public static dynamic lt(dynamic a, dynamic b) { return a < b; }

        /// <summary>
        /// Gerater or equal operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns>A gteq B</returns>
        public static dynamic gteq(dynamic a, dynamic b) { return a >= b; }

        /// <summary>
        /// Less or equal operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns>A lteq B</returns>
        public static dynamic lteq(dynamic a, dynamic b) { return a <= b; }

        /// <summary>
        /// Equals operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns>A == B</returns>
        public static dynamic eq(dynamic a, dynamic b) { return a.Equals(b); }

        /// <summary>
        /// Hash operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <returns>Hash code of A</returns>
        public static dynamic hash(dynamic a) { return a.GetHashCode(); }

        /// <summary>
        /// Not equal operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns>A != B</returns>
        public static dynamic neq(dynamic a, dynamic b) { return a != b; }

        /// <summary>
        /// Conditional ternary operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <param name="c">Operand C.</param>
        /// <returns>A ? B : C</returns>
        public static dynamic cond(dynamic a, dynamic b, dynamic c) { return a ? b : c; }

        /// <summary>
        /// Not operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <returns>!A</returns>
        public static dynamic not(dynamic a) { return !a; }

        /// <summary>
        /// OR operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns>A OR B</returns>
        public static dynamic or(dynamic a, dynamic b) { return a || b; }

        /// <summary>
        /// AND operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns>A AND B</returns>
        public static dynamic and(dynamic a, dynamic b) { return a && b; }

        /// <summary>
        /// XOR operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns>A XOR B</returns>
        public static dynamic xor(dynamic a, dynamic b) { return a ^ b; }

        /// <summary>
        /// Bitwise OR operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operan B.</param>
        /// <returns>A | B</returns>
        public static dynamic bit_or(dynamic a, dynamic b) { return a | b; }

        /// <summary>
        /// Bitwise AND operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <param name="b">Operand B.</param>
        /// <returns>A % B</returns>
        public static dynamic bit_and(dynamic a, dynamic b) { return a & b; }

        /// <summary>
        /// Complement operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <returns>~a</returns>
        public static dynamic complement(dynamic a) { return ~a; }

        /// <summary>
        /// Invokes the specified function with the given arguments.
        /// </summary>
        /// <param name="f">The function.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>f(args)</returns>
        public static dynamic invoke(dynamic f, dynamic args) { return f(args); }

        /// <summary>
        /// Invoke the specified function with the given arguments, but returns void.
        /// </summary>
        /// <param name="f">The function.</param>
        /// <param name="args">The arguments.</param>
        public static void invoke_void(dynamic f, dynamic args) { f(args); }

        /// <summary>
        /// True expression.
        /// </summary>
        /// <returns>True</returns>
        public static dynamic @true() { return true; }

        /// <summary>
        /// False expression
        /// </summary>
        /// <returns>False</returns>
        public static dynamic @false() { return false; }

        /// <summary>
        /// Successor operator.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <returns>A + 1</returns>
        public static dynamic succ(dynamic a) { return a + 1; }

        /// <summary>
        /// Predecessor
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <returns>A - 1</returns>
        public static dynamic pred(dynamic a) { return a - 1; }

        /// <summary>
        /// Head operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <returns>Head of A</returns>
        public static dynamic head(dynamic a) { return a.Head; }

        /// <summary>
        /// Tail operation
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <returns>Tail of A</returns>
        public static dynamic tail(dynamic a) { return a.Tail; }

        /// <summary>
        /// Type operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <returns>Type of A</returns>
        public static dynamic type(dynamic a) { return a.GetType(); }

        /// <summary>
        /// Print operation. Write a term into console.
        /// </summary>
        /// <param name="a">Operand A.</param>
        public static void print(dynamic a) { Console.Write(a); }

        /// <summary>
        /// To String operation.
        /// </summary>
        /// <param name="a">Operand A.</param>
        /// <returns>A to string.</returns>
        public static dynamic tostring(dynamic a) { return a.ToString(); }

        /// <summary>
        /// Index item operation.
        /// </summary>
        /// <param name="obj">The array.</param>
        /// <param name="index">The index.</param>
        /// <returns>Item at position index of the array obj</returns>
        public static dynamic index(dynamic obj, dynamic index) { return obj[index]; }

        /// <summary>
        /// Dynamically invokes a function
        /// </summary>
        /// <param name="func">The function.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static dynamic dynamic_invoke(dynamic func, dynamic[] args) { return func.DynamicInvoke(args); }

        /// <summary>
        /// Gets the method by name.
        /// </summary>
        /// <param name="s">The name of the method.</param>
        /// <returns></returns>
        public static MethodInfo GetMethod(string s)
        {
            return typeof(Primitives).GetMethod(s);
        }

        /// <summary>
        /// Gets the method name from the unary operator.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Not a recognized unary operator  + s</exception>
        public static string UnaryOperatorToMethodName(string s)
        {
            switch (s)
            {
                case "-": return "negative";
                case "!": return "not";
                case "~": return "complement";
                default: throw new Exception("Not a recognized unary operator " + s);
            }
        }

        /// <summary>
        /// Gets the method name from binary operator.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Not a recognized operator</exception>
        public static string GetMethodNameFromBinaryOperator(string s)
        {
            switch (s)
            {
                case "+": return "add";
                case "-": return "subtract";
                case "*": return "multiply";
                case "/": return "divide";
                case "%": return "modulo";
                case ">>": return "shl";
                case "<<": return "shr";
                case ">": return "gt";
                case "<": return "lt";
                case ">=": return "gteq";
                case "<=": return "lteq";
                case "==": return "eq";
                case "!=": return "neq";
                case "||": return "or";
                case "&&": return "and";
                case "^": return "xor";
                case "|": return "bit_or";
                case "&": return "bit_nand";
                default: throw new Exception("Not a recognized operator");
            }
        }

        /// <summary>
        /// Evaluates the specified operator with the operands a0 and a1.
        /// </summary>
        /// <param name="op">The op.</param>
        /// <param name="a0">The a0.</param>
        /// <param name="a1">The a1.</param>
        /// <returns></returns>
        public static dynamic Eval(string op, dynamic a0, dynamic a1)
        {
            return GetMethodFromBinaryOperator(op).Invoke(null, new[] { a0, a1 });
        }

        /// <summary>
        /// Gets the method from binary operator.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static MethodInfo GetMethodFromBinaryOperator(string s)
        {
            return GetMethod(GetMethodNameFromBinaryOperator(s));
        }

        #endregion
    }
}
