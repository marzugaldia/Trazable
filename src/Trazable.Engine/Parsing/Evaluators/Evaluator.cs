using System;
using System.Diagnostics;
using Trazable.Engine.Parsing.Grammars;

namespace Trazable.Engine.Parsing.Evaluators
{
    /// <summary>
    /// Class for a generic evaluator
    /// </summary>
    public class Evaluator
    {
        #region - Fields -

        /// <summary>
        /// The variable environment
        /// </summary>
        public VarBindings environment = new VarBindings();

        /// <summary>
        /// The template container
        /// </summary>
        public TemplateContainer container;

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="Evaluator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public Evaluator(TemplateContainer container)
        {
            if (container != null)
            {
                this.container = container;
            }
            else
            {
                this.container = new TemplateContainer();
            }
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds a variable to the environment.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public object AddVariable(string name, dynamic value)
        {
            this.environment = environment.AddBinding(name, value);
            return value;
        }

        /// <summary>
        /// Adds a global variable to the environment.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Variable already exists</exception>
        public object AddGlobal(string name, object value)
        {
            var e = environment; while (e.Tail != null) { Debug.Assert(e.Name != name); e = e.Tail; }
            e.Tail = new VarBindings { Name = name, Value = value }; ;
            return value;
        }

        /// <summary>
        /// Finds a variable and changes its value. If not found, the variable is created with global scope.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public object RebindGlobal(string name, object value)
        {
            var c = this.environment.FindBindingOrDefault(name);
            return c != null ? c.Value = value : AddGlobal(name, value);
        }


        /// <summary>
        /// Evaluates a function inside the scope.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        public object EvalScoped(Func<dynamic> func)
        {
            VarBindings node = this.environment;
            var result = func();
            this.environment = node;
            return result;
        }

        #endregion
    }
}
