using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;

namespace Unlimitedinf.Tools.IntTests
{
    [TestClass]
    public class SemVerTests
    {
        [TestMethod]
        public void SV_Deserialization()
        {
            SemVer deserialized = JsonConvert.DeserializeObject<SemVer>(@" ""1.2.3-4567+890"" ");
            SemVer version = SemVer.Parse("1.2.3-4567+890");
            Assert.IsTrue(deserialized == version);
        }

        [TestMethod]
        public void SV_Serialization()
        {
            SemVer version = SemVer.Parse("1.2.3-4567+890");
            string serialized = JsonConvert.SerializeObject(version);
            Assert.AreEqual(@"""1.2.3-4567+890""", serialized);
        }

        [TestMethod]
        public void SV_IncrementMajorTest()
        {
            SemVer version = new SemVer(1, 2, 3);
            version = version.IncrementMajor();
            Assert.IsTrue(version == new SemVer(2, 0, 0));
        }

        [TestMethod]
        public void SV_IncrementMinorTest()
        {
            SemVer version = new SemVer(1, 2, 3);
            version = version.IncrementMinor();
            Assert.IsTrue(version == new SemVer(1, 3, 0));
        }

        [TestMethod]
        public void SV_IncrementPatchTest()
        {
            SemVer version = new SemVer(1, 2, 3);
            version = version.IncrementPatch();
            Assert.IsTrue(version == new SemVer(1, 2, 4));
        }

        [TestMethod]
        public void SV_SetPrereleaseTest()
        {
            SemVer version = SemVer.Parse("1.2.3-4567+890");
            version = version.SetPrerelease("5678");
            Assert.AreEqual("5678", version.Prerelease);
            Assert.IsNull(version.Build);
        }

        [TestMethod]
        public void SV_SetBuildTest()
        {
            SemVer version = SemVer.Parse("1.2.3-4567+890");
            version = version.SetBuild("5678");
            Assert.AreEqual("4567", version.Prerelease);
            Assert.AreEqual("5678", version.Build);
        }

        [TestMethod]
        public void SV_GetHashCodeTest()
        {
            Assert.AreNotEqual(SemVer.Parse("1.2.3-4+5"), SemVer.Parse("1.2.3-4+6"));
        }

        [TestMethod]
        public void SV_ToStringTest()
        {
            string ver = "56548.1861.1863457-asd873-ASD+8500-asd-ASDU-8";
            Assert.AreEqual(ver, SemVer.Parse(ver).ToString());
        }

        #region Tests from blog, adapted to fit my specifics

        [TestMethod]
        public void SV_CompareToComparesTwoSemVerObjects()
        {
            var version1 = new SemVer(1, 0, 0);
            var version2 = new SemVer(1, 0, 0);
            Assert.AreEqual(0, version1.CompareTo(version2));
        }

        [TestMethod]
        public void SV_MajorVersionIsLessThanOther()
        {
            var version1 = new SemVer(1, 2, 3);
            var version2 = new SemVer(2, 0, 0);
            Assert.IsTrue(version1 < version2);
        }

        [TestMethod]
        public void SV_MinorVersionIsGreaterThanOther()
        {
            var version1 = new SemVer(1, 2, 0);
            var version2 = new SemVer(1, 1, 0);
            Assert.IsTrue(version1 > version2);
        }

        [TestMethod]
        public void SV_PatchVersionIsLessThanOther()
        {
            var version1 = new SemVer(1, 1, 3);
            var version2 = new SemVer(1, 1, 4);
            Assert.IsTrue(version1 < version2);
        }

        [TestMethod]
        public void SV_ReleaseVersionIsGreaterThanPrereleaseVersion()
        {
            var version1 = SemVer.Parse("1.0.0-alpha");
            var version2 = new SemVer(1, 0, 0);
            Assert.IsTrue(version1 < version2);
            Assert.IsTrue(version2 > version1);
        }

        [TestMethod]
        public void SV_VersionIsEqualToItself()
        {
            var version = new SemVer(1, 0, 0);
            Assert.IsTrue(version.Equals(version));
        }

        [TestMethod]
        public void SV_VersionIsNotEqualToNull()
        {
            var version = new SemVer(1, 0, 0);
            Assert.IsFalse(version == null);
            Assert.IsFalse(null == version);
            Assert.IsTrue(null != version);
            Assert.IsTrue(version != null);
            object other = null;
            Assert.IsFalse(version.Equals(other));
        }

        [TestMethod]
        public void SV_VersionIsTheSameAsItself()
        {
            var version = new SemVer(1, 0, 0);
            Assert.AreEqual(0, version.CompareTo(version));
            Assert.IsTrue(version.Equals(version));
        }

        [TestMethod]
        public void SV_VersionsAreComparedCorrectly()
        {
            var version1 = SemVer.Parse("1.0.0-alpha");
            var version2 = SemVer.Parse("1.0.0-alpha.1");
            var version3 = SemVer.Parse("1.0.0-beta.2");
            var version4 = SemVer.Parse("1.0.0-beta.11");
            var version5 = SemVer.Parse("1.0.0-rc.1");
            var version6 = SemVer.Parse("1.0.0-rc.1+build.1");
            var version7 = SemVer.Parse("1.0.0");
            var version8 = SemVer.Parse("1.0.0+0.3.7");
            var version9 = SemVer.Parse("1.3.7+build");
            var version10 = SemVer.Parse("1.3.7+build.2.b8f12d7");
            var version11 = SemVer.Parse("1.3.7+build.11.e0f985a");
            var version12 = SemVer.Parse("1.0.0-beta");
            var version13 = SemVer.Parse("1.0.0+0.3");
            Assert.IsTrue(version1 < version2);
            Assert.IsTrue(version2 < version3);
            Assert.IsTrue(version3 < version4);
            Assert.IsTrue(version4 < version5);
            Assert.IsTrue(version5 == version6);
            Assert.IsTrue(version6 < version7);
            Assert.IsTrue(version7 == version8);
            Assert.IsTrue(version8 < version9);
            Assert.IsTrue(version9 == version10);
            Assert.IsTrue(version10 == version11);
            Assert.IsTrue(version4 > version12);
            Assert.IsTrue(version8 == version7);
            Assert.IsTrue(version8 == version13);
        }

        [TestMethod]
        public void SV_VersionsAreEqual()
        {
            var version1 = SemVer.Parse("1.0.0-alpha+build.1");
            var version2 = SemVer.Parse("1.0.0-alpha+build.2");
            var version3 = version2;
            var version4 = version1;
            Assert.IsTrue(version1 == version2);
            Assert.IsFalse(version1.Equals(version3));
            Assert.IsTrue(version1.Equals(version4));
        }

        [TestMethod]
        public void SV_VersionsAreNotEqual()
        {
            var version1 = SemVer.Parse("1.0.0");
            var version2 = SemVer.Parse("1.0.0-alpha+build.1");
            object version3 = version2;
            Assert.IsTrue(version1 != version2);
            Assert.IsFalse(version1.Equals(version3));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SV_VersionCannotBeComparedToNull()
        {
            var version1 = new SemVer(1, 0, 0);
            SemVer version2 = null;
            version1.CompareTo(version2);
        }
        #endregion
    }
}