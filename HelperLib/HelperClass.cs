using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLib
{
    public static class HelperClass
    {
        public static void AssertIs42(int value)
        {
            Assert.AreEqual(42, value);
        }
    }
}
