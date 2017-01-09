using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domo.Serialization;

namespace Domo.Testing
{
    [TestClass]
    public class SerializationTest
    {
        [TestMethod]
        public void Serialization_General()
        {
            var test = new TestObject()
            {
                name = "test",
                value = 313
            };
            
            string json = Serializer.instance.Serialize(test);
            Console.WriteLine(json);
            Assert.AreEqual(test, Serializer.instance.Deserialize<TestObject>(json));
        }

        public class TestObject
        {
            public string name;
            public int value;

            public override bool Equals(object obj)
            {
                TestObject o = obj as TestObject;
                return o != null && o.name == name && o.value == value;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}
