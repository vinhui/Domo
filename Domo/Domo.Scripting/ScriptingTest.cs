using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using IronPython.Runtime;

namespace Domo.Scripting
{
    public class ScriptingTest
    {
        public ScriptingTest()
        {
            var runtime  =Python.CreateRuntime();            
            
            Microsoft.Scripting.Hosting.ScriptEngine e = Python.CreateEngine();
            ScriptScope scope = e.CreateScope();
            var source = e.CreateScriptSourceFromString("");
            CompiledCode code = source.Compile();

        }
    }
}
