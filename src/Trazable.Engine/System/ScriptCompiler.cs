using Microsoft.CSharp;
using Microsoft.JScript;
using Microsoft.VisualBasic;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Trazable.Engine.System
{
    /// <summary>
    /// Compiler for scripts (C#, VB, Javascript)
    /// </summary>
    public class ScriptCompiler
    {
        #region - Fields -

        /// <summary>
        /// The compiler provider for C#
        /// </summary>
        private CodeDomProvider compiler;

        /// <summary>
        /// The compiler options.
        /// </summary>
        private CompilerParameters compilerOptions;

        /// <summary>
        /// The compilation errors.
        /// </summary>
        private CompilerErrorCollection compilerErrors;

        /// <summary>
        /// The compiled script.
        /// </summary>
        private Assembly compiledScript;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the script code.
        /// </summary>
        /// <value>
        /// The script code.
        /// </value>
        public string ScriptCode { get; set; }

        /// <summary>
        /// Gets the compiler options.
        /// </summary>
        /// <value>
        /// The compiler options.
        /// </value>
        public CompilerParameters CompilerOptions
        {
            get { return this.compilerOptions; }
        }

        /// <summary>
        /// Gets the compilation errors.
        /// </summary>
        /// <value>
        /// The compilation errors.
        /// </value>
        public CompilerErrorCollection CompilerErrors
        {
            get { return this.compilerErrors; }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="ScripterRunner"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ScriptCompiler(string name, ScriptType scriptType)
        {
            this.Name = name;
            InitCompiler(scriptType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScripterRunner"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ScriptCompiler(string name)
            : this(name, ScriptType.CSharp)
        {
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Initialization of the compiler, adding compiler options.
        /// </summary>
        private void InitCompiler(ScriptType scriptType)
        {
            Dictionary<string, string> compilerInfo = new Dictionary<string, string>();
            compilerInfo.Add("CompilerVersion", "v4.0");
            switch (scriptType)
            {
                case ScriptType.VBasic:
                    compiler = new VBCodeProvider(compilerInfo);
                    break;
                case ScriptType.JScript:
                    compiler = new JScriptCodeProvider();
                    break;
                default:
                    compiler = new CSharpCodeProvider(compilerInfo);
                    break;
            }
            this.compilerOptions = new CompilerParameters();
            this.compilerOptions.GenerateExecutable = false;
            this.compilerOptions.GenerateInMemory = true;
        }

        /// <summary>
        /// Adds an assembly to the referenced assemblies.
        /// </summary>
        /// <param name="name">The name.</param>
        public void AddAssembly(string name)
        {
            name = name.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                if (!this.compilerOptions.ReferencedAssemblies.Contains(name))
                {
                    this.compilerOptions.ReferencedAssemblies.Add(name);
                }
            }
        }

        /// <summary>
        /// Adds an assembly list to the referenced assemblies.
        /// </summary>
        /// <param name="names">The names.</param>
        public void AddAssemblies(string[] names)
        {
            foreach (string name in names)
            {
                this.AddAssembly(name);
            }
        }

        /// <summary>
        /// Adds an assembly list to the referenced assemblies.
        /// </summary>
        /// <param name="names">The names.</param>
        public void AddAssemblies(IList<string> names)
        {
            this.AddAssemblies(names.ToArray());
        }

        /// <summary>
        /// Adds the default assemblies to the referenced assemblies.
        /// </summary>
        public void AddDefaultAssemblies()
        {
            this.compilerOptions.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
            this.AddAssembly("System.dll");
            this.AddAssembly("System.Core.dll");
            this.AddAssembly("System.Xml.dll");
            this.AddAssembly("System.Data.dll");
        }

        /// <summary>
        /// Compiles the script.
        /// </summary>
        /// <returns></returns>
        public bool Compile()
        {
            CompilerResults result = this.compiler.CompileAssemblyFromSource(this.compilerOptions, this.ScriptCode);
            this.compilerErrors = result.Errors;
            if (result.Errors.HasErrors)
            {
                this.compiledScript = null;
                return false;
            }
            else
            {
                this.compiledScript = result.CompiledAssembly;
                return true;
            }
        }

        /// <summary>
        /// Compiles the specified script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        public bool Compile(string script)
        {
            this.ScriptCode = script;
            return this.Compile();
        }

        /// <summary>
        /// Loads the script from file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void LoadFromFile(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            this.ScriptCode = reader.ReadToEnd();
            reader.Close();
        }

        /// <summary>
        /// Gets an interface from the compiled script.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetInterface<T>()
        {
            foreach (Type type in compiledScript.GetExportedTypes())
            {
                foreach (Type iface in type.GetInterfaces())
                {
                    if (iface == typeof(T))
                    {
                        ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                        if ((constructor != null) && (constructor.IsPublic))
                        {
                            T scriptObject = (T)constructor.Invoke(null);
                            return scriptObject;
                        }
                    }
                }
            }
            return default(T);
        }

        /// <summary>
        /// Gets a list of interfaces of the given type from the compiled script.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetInterfaces<T>()
        {
            List<T> list = new List<T>();
            foreach (Type type in compiledScript.GetExportedTypes())
            {
                foreach (Type iface in type.GetInterfaces())
                {
                    if (iface == typeof(T))
                    {
                        ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                        if ((constructor != null) && (constructor.IsPublic))
                        {
                            T scriptObject = (T)constructor.Invoke(null);
                            list.Add(scriptObject);
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Gets a type by its name in the compiled assembly
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Type GetTypeByName(string name)
        {
            name = name.ToLower();
            foreach (Type subtype in this.compiledScript.DefinedTypes)
            {
                if (subtype.Name.ToLower().Equals(name))
                {
                    return subtype;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets a type by its name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Type GetType(string name)
        {
            Type result = this.compiledScript.GetType(name);
            if (result == null)
            {
                result = GetTypeByName(name);
            }
            return result;
        }


        /// <summary>
        /// Gets a method from the name of the type and name of the method.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        public MethodInfo GetMethod(string typeName, string methodName)
        {
            Type type = this.GetType(typeName);
            if (type == null)
            {
                type = this.GetTypeByName(typeName);
            }
            MethodInfo method = type.GetMethod(methodName);
            if (method == null)
            {
                method = type.GetMethod(type.FullName + "." + methodName);
            }
            return method;
        }

        /// <summary>
        /// Runs the main.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void RunMain(params string[] args)
        {
            if (this.compiledScript == null)
            {
                this.Compile();
            }
            MethodInfo main = null;
            foreach (var t in this.compiledScript.GetTypes())
                foreach (var mi in t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                    if (mi.Name == "Main")
                        main = mi;
            main.Invoke(null, new object[] { args });
        }

        /// <summary>
        /// Runs the specified method.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// The type is null
        /// or
        /// The method is null
        /// </exception>
        public object Run(string typeName, string methodName, params object[] args)
        {
            if (this.compiledScript == null)
            {
                this.Compile();
            }
            Type type = this.GetType(typeName);
            if (type == null)
            {
                throw new Exception("The type is null");
            }
            MethodInfo method = type.GetMethod(methodName);
            if (method == null)
            {
                throw new Exception("The method is null");
            }
            object obj = Activator.CreateInstance(type);
            return method.Invoke(obj, args);
        }

        /// <summary>
        /// Compiles and run main.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="args">The arguments.</param>
        public void CompileAndRunMain(string script, params string[] args)
        {
            this.ScriptCode = script;
            this.RunMain(args);
        }

        /// <summary>
        /// Compiles and run method.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public object CompileAndRunMethod(string script, string typeName, string methodName, params object[] args)
        {
            this.ScriptCode = script;
            return this.Run(typeName, methodName, args);
        }

        #endregion
    }
}
