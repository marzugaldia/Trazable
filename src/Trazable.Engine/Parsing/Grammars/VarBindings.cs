using System;
using System.Collections.Generic;
using System.Linq;

namespace Trazable.Engine.Parsing.Grammars
{
    /// <summary>
    /// Class for the variable environment bindings.
    /// </summary>
    public class VarBindings
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the tail.
        /// </summary>
        /// <value>
        /// The tail.
        /// </value>
        public VarBindings Tail { get; set; }

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public object this[string name]
        {
            get { return Find(name); }
        }

        /// <summary>
        /// Gets the bindings.
        /// </summary>
        /// <value>
        /// The bindings.
        /// </value>
        public IEnumerable<VarBindings> Bindings
        {
            get
            {
                for (var b = this; b != null; b = b.Tail)
                    yield return b;
            }
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds the binding.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public VarBindings AddBinding(string name, object value)
        {
            return new VarBindings { Name = name, Value = value, Tail = this };
        }

        /// <summary>
        /// Finds the binding or default.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public VarBindings FindBindingOrDefault(string name)
        {
            return Bindings.FirstOrDefault(c => c.Name == name);
        }

        /// <summary>
        /// Finds the binding.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Name does not exist in context:  + name</exception>
        public VarBindings FindBinding(string name)
        {
            var r = FindBindingOrDefault(name);
            if (r == null) throw new Exception("Name does not exist in context: " + name);
            return r;
        }

        /// <summary>
        /// Finds an object of the environment by its name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public object Find(string name)
        {
            return FindBinding(name).Value;
        }

        /// <summary>
        /// Binds a value to a name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public dynamic Bind(string name, dynamic value)
        {
            return FindBinding(name).Value = value;
        }

        /// <summary>
        /// Returns true if a binding with the given name exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool ExistsBinding(string name)
        {
            return FindBindingOrDefault(name) != null;
        }

        #endregion

    }
}
