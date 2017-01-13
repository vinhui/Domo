﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Scripting.Hosting;
using PythonEngine = Microsoft.Scripting.Hosting.ScriptEngine;
using IronPython.Hosting;
using IronPython.Runtime;
using IronPython.Modules;
using Microsoft.Scripting;
using Domo.Misc.Debug;

namespace Domo.Scripting
{
    public class ScriptEngine
    {
        private PythonEngine engine;
        private ScriptScope scope;

        public ScriptEngine()
        {
            engine = CreatePythonEngine(out scope);
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
            globalScope = e.CreateScope();
            globalScope.SetVariable("Print", new Action<string>(Log.Info));

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
