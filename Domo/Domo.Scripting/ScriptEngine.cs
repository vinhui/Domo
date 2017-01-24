using Domo.Misc.Debug;
using IronPython.Hosting;
using IronPython.Modules;
using IronPython.Runtime;
using IronPython.Runtime.Operations;
using IronPython.Runtime.Types;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PythonEngine = Microsoft.Scripting.Hosting.ScriptEngine;

namespace Domo.Scripting
{
    public class ScriptEngine
    {
        public PythonEngine engine { get; private set; }
        private ScriptScope _scope;
        public ScriptScope scope { get { return _scope; } }

        public ScriptEngine()
        {
            engine = CreatePythonEngine(out _scope);
        }

        public void AddReference(ScriptEngine engine)
        {
            var vars = engine.GetVariables();
            foreach (var item in vars)
            {
                if (item.Value != null)
                    scope.SetVariable(item.Key, item.Value);
            }
        }

        public IEnumerable<KeyValuePair<string, dynamic>> GetVariables()
        {
            return scope.GetItems();
        }

        public IEnumerable<PythonType> GetTypes<T>()
        {
            return GetTypes(typeof(T));
        }

        public IEnumerable<PythonType> GetTypes(Type t)
        {
            List<PythonType> types = new List<PythonType>();

            foreach (var item in GetTypes())
            {
                if (PythonOps.IsSubClass(item, DynamicHelpers.GetPythonTypeFromType(t)))
                {
                    types.Add(item);
                }
            }

            return types;
        }

        public IEnumerable<PythonType> GetTypes()
        {
            List<PythonType> types = new List<PythonType>();

            foreach (var item in scope.GetItems())
            {
                if (item.Value is PythonType)
                {
                    PythonType type = item.Value;

                    Type t = ClrModule.GetClrType(type);

                    if (!t.IsAbstract)
                    {
                        types.Add(type);
                    }
                }
            }

            return types;
        }

        public void AddFile(string path)
        {
            ScriptSource source = engine.CreateScriptSourceFromFile(path, Encoding.Default, Microsoft.Scripting.SourceCodeKind.File);

            try
            {
                CompiledCode compiled = source.Compile();
                compiled.Execute(scope);
            }
            catch (SyntaxErrorException ex)
            {
                Log.Error("Failed to load " + Path.GetFileNameWithoutExtension(path) + ", error on line " + ex.Line + " and column " + ex.Column + ": " + ex.Message);
                Log.Error("Line: '" + source.GetCodeLine(ex.Line) + "'");
            }
        }

        public void AddFiles(params string[] paths)
        {
            foreach (string path in paths)
            {
                AddFile(path);
            }
        }

        private PythonEngine CreatePythonEngine(out ScriptScope globalScope)
        {
            PythonEngine e = Python.CreateEngine();
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                e.Runtime.LoadAssembly(item);
            }
            globalScope = e.CreateScope();
            globalScope.SetVariable("Print", new Action<string>(Log.Info));

            PythonType log = DynamicHelpers.GetPythonTypeFromType(typeof(Log));
            globalScope.SetVariable("log", log);
            globalScope.SetVariable("Log", log);

            ScriptScope builtinScope = Python.GetBuiltinModule(e);
            //builtinScope.SetVariable("__import__", new Func<CodeContext, string, PythonDictionary, PythonDictionary, PythonTuple, object>(ImportModule));

            return e;
        }

        private static object ImportModule(CodeContext context, string moduleName, PythonDictionary globals, PythonDictionary locals, PythonTuple tuple)
        {
            object o = Builtin.__import__(context, moduleName, globals, locals, tuple, -1);
            return o;
        }

        public void Unload()
        {

        }
    }
}
