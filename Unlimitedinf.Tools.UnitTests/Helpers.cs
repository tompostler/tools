/// <summary>
/// Used to extend classes (such as constants), as well as provide generic unit testing helpers.
/// </summary>
namespace Unlimitedinf.Tools.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static partial class Constants
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
