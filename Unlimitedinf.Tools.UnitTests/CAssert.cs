using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Unlimitedinf.Tools.UnitTests
{
    public static class CAssert
    {
        public static void AreEqual<T>(List<T> expected, List<T> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count, "Counts are not equal");
            for (int i = 0; i < expected.Count; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }
    }
}
