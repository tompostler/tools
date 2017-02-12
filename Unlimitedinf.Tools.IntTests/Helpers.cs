using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Used to extend classes (such as constants), as well as provide generic unit testing helpers.
/// </summary>
namespace Unlimitedinf.Tools.IntTests
{
    internal static partial class C
    {
        public static partial class BinaryPackage
        {
            /// <summary>
            /// Causes an Assert.Inconclusive if the package was not successfully acquired.
            /// </summary>
            /// <exception cref="AssertInconclusiveException"></exception>
            public static void AssertExists()
            {
                if (Acquired == false)
                    Assert.Inconclusive($"Package {Id} failed to acquire.");
            }
        }
    }
}
