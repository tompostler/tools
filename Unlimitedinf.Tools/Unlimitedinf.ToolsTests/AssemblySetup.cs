namespace Unlimitedinf.ToolsTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NuGet;
    using System;
    using System.IO;

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
            {
                Constants.BinaryPackage.Acquired = true;
                return;
            }

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
