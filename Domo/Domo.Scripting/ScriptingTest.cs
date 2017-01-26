using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Domo.Scripting
{
    public class ScriptingTest
    {
        public ScriptingTest()
        {
            var runtime = Python.CreateRuntime();

            Microsoft.Scripting.Hosting.ScriptEngine e = Python.CreateEngine();
            ScriptScope scope = e.CreateScope();
            var source = e.CreateScriptSourceFromString("");
            CompiledCode code = source.Compile();
        }
    }
}