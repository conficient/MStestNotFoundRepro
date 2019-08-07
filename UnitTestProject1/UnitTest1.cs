using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var actual = ClassLibrary1.Class1.GetTheAnswer();

            //HelperLib.HelperClass.AssertIs42(actual);
        }
    }
}
