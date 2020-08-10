using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibrary1.Test
{
    [TestClass]
    public class UnitTest1
    {
        Class1 testObj;

        [TestInitialize]
        public void setup()
        {
            testObj = new Class1();
        }

        [DataRow(1, 2, 3)]
        [DataRow(0, 3, 3)]
        [TestMethod]
        public void Add(int a, int b, int expected)
        {
            var actual = testObj.Add(a, b);

            Assert.AreEqual(actual, expected);

        }
    }
}
