using Domo.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Domo.Testing
{
    [TestClass]
    public class ConfigTest
    {
        [TestMethod]
        public void Config_SetValue()
        {
            TestUtils.SetEntryAssembly();

            Config.SetValue(true, false, "test", "test");
            Assert.IsTrue(Config.GetValue<bool>("test", "test"));
        }
    }
}