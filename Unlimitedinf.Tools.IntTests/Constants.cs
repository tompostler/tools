using System;
using System.Collections.Generic;
using System.IO;

namespace Unlimitedinf.Tools.IntTests
{
    /// <summary>
    /// Test constants, usually string-based.
    /// </summary>
    internal static partial class C
    {
        /// <summary>
        /// Shorthand to be able to use Path, see <see cref="Path.Combine(string, string)"/>.
        /// </summary>
        private static Func<string, string, string> Combine = Path.Combine;

        /// <summary>
        /// This should be where the assembly is. Regardless, it's used for everything and will work as long as we've
        /// got permissions.
        /// </summary>
        public static string BasePath => Directory.GetCurrentDirectory();

        /// <summary>
        /// Constants for the binary test asset package.
        /// </summary>
        public static partial class BinaryPackage
        {
            public const string Id = "Unlimitedinf.Tools.Tests.Binary";
            public const string Version = "0.1.0";
            public static string Path => Combine(BasePath, $"{Id}.{Version}");

            /// <summary>
            /// True if the package was successfully acquired. Enables one simple error check.
            /// </summary>
            public static bool Acquired = false;

            public static class Content
            {
                private static string Path => Combine(BinaryPackage.Path, "content");

                private static IEnumerable<string> _Images = null;
                public static IEnumerable<string> Images => _Images ?? (_Images = Directory.EnumerateFiles(Combine(Path, "images")));
            }
        }
    }
}
