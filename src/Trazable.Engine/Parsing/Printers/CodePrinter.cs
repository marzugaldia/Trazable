using System;
using System.Collections.Generic;
using System.Text;

namespace Trazable.Engine.Parsing.Printers
{
    /// <summary>
    /// Class for a Code Printer. Transforms parser token expression tree into code.
    /// </summary>
    public abstract class CodePrinter
    {
        #region - Fields -

        /// <summary>
        /// The string builder.
        /// </summary>
        private StringBuilder sb = new StringBuilder();

        /// <summary>
        /// The indentation
        /// </summary>
        private string indent = "";

        #endregion

        #region - Methods -

        /// <summary>
        /// Prints the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public abstract CodePrinter Print(Token token);

        /// <summary>
        /// Prints the specified string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public CodePrinter Print(string s)
        {
            sb.Append(s);
            return this;
        }

        /// <summary>
        /// Prints the specified token and a line break.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public CodePrinter PrintLine(Token n)
        {
            Print(n);
            sb.AppendLine();
            sb.Append(this.indent);
            return this;
        }

        /// <summary>
        /// Prints a string and a line break.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public CodePrinter PrintLine(string s = "")
        {
            sb.AppendLine(s).Append(this.indent);
            return this;
        }

        /// <summary>
        /// Add indent to the code.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public CodePrinter Indent(string s = "")
        {
            this.indent = this.indent + "  ";
            return PrintLine(s);
        }

        /// <summary>
        /// Removes indent from the code.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public CodePrinter Unindent(string s = "")
        {
            if (this.indent.Length > 0)
            {
                this.indent = this.indent.Substring(0, indent.Length - 2);
            }
            return PrintLine(s);
        }

        /// <summary>
        /// Prints the specified tokens.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <param name="sep">The sep.</param>
        /// <returns></returns>
        public CodePrinter Print(IEnumerable<Token> tokens, string sep = "")
        {
            bool first = true;
            foreach (var n in tokens)
            {
                if (!first)
                    Print(sep);
                first = false;
                Print(n);
            }
            return this;
        }

        /// <summary>
        /// Prints the lines.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <returns></returns>
        public CodePrinter PrintLines(IEnumerable<Token> tokens)
        {
            foreach (var n in tokens)
                PrintLine(n);
            return this;
        }

        /// <summary>
        /// Indents the specified function.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns></returns>
        public CodePrinter Indent(Func<CodePrinter> f)
        {
            Indent();
            f();
            return Unindent();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.sb.ToString();
        }

        #endregion
    }
}
