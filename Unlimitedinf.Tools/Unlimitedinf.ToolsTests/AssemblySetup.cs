namespace Unlimitedinf.ToolsTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NuGet;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Test constants, usually string-based.
    /// </summary>
    public static class Constants
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
        public static class BinaryPackage
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

    /// <summary>
    /// Actions to be performed on assembly setup.
    /// </summary>
    [TestClass]
    public static class AssemblySetup
    {
        /// <summary>
        /// Runs on assembly initialization.
        /// </summary>
        /// <param name="testContext"></param>
        [AssemblyInitialize]
        public static void AssemblyInitialization(TestContext testContext)
        {
            if (Directory.Exists(Constants.BinaryPackage.Path))
                return;

            // Get the nuget package
            // http://blog.nuget.org/20130520/Play-with-packages.html

            try
            {
                // Connect to the official package repository
                IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
                // Initialize the package manager
                PackageManager packageManager = new PackageManager(repo, Constants.BasePath);
                // Download and unzip the package
                packageManager.InstallPackage(Constants.BinaryPackage.Id, SemanticVersion.Parse(Constants.BinaryPackage.Version), true, true);

                Constants.BinaryPackage.Acquired = true;
            }
            catch (Exception)
            { }
        }
    }
}
