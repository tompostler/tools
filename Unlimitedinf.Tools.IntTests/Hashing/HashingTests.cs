using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unlimitedinf.Tools.Hashing;

namespace Unlimitedinf.Tools.IntTests.Hashing
{
    [TestClass]
    public class HashingTests
    {
        private static IEnumerable<FileInfo> Images = null;

        [ClassInitialize]
        public static void Initialize(TestContext tc)
        {
            List<FileInfo> images = new List<FileInfo>();
            foreach (string filename in C.BinaryPackage.Content.Images)
                images.Add(new FileInfo(filename));
            Images = images;
        }

        [TestMethod]
        public void BlockhashTest()
        {
            C.BinaryPackage.AssertExists();

            Dictionary<string, string> hashesExpected = new Dictionary<string, string>
            {
                { "astronomy-ngc-3766.jpg",     "ecc00fd13f8077f01fe027e01ff01fe04fe98fc00fc0cfe0e7f444fc28f003f8" },
                { "burst.jpg",                  "06f007f00ff81ff807c007e00ff81ffe1ffe1fe00fe007c03ff83ff00fe001e0" },
                { "halo-5-cover-small.png",     "c7e3c7e383c383c08fe30ff007e00ff00ff00fe04fe247e27fe6ffee03c60000" },
                { "halo-5-cover.jpg",           "c7e3c7e383c383c08fe30ff007e00ff00ff00fe04fe247e27fe6ffee03c60000" },
                { "halo-master-chief.jpg",      "00000000ffffffff373c77067706478677cd160c174cbfc0fffffff902000001" },
                { "landscape-mountains.jpeg",   "000f003f01ff1fff0004003307ffffff1fff0bff00c300781fff01f701e70185" },
                { "milky-way-laser.jpg",        "83f807f807f80ff01fe01fe01fe03fc07fc03fc07fc07c40fefaeefc4c700100" },
                { "skyrim.jpg",                 "ffe0ffc0e080fe00ffb8df7888f0e000affc1ff40fc40700c7fc03fc03ec01f8" },
                { "stargate-atlantis.jpg",      "00008bc19be9fffd87e0366f0db84fe00da01d943ffc2fc4fffd3ffc01a30000" },
                { "unlimitedinf-0100.png",      "07e00ff01ff01f780c300ff013cc3ffe6ff2cfb187e003c01ffe3ffe1e380000" },
                { "unlimitedinf-1024.png",      "07e00ff01ff01f780c300ff013cc3ffe6ff2cfb187e003c01ffe3ffe1e380000" },
                { "unlimitedinf.ico",           "07e00ff01ff01f780c300ff013cc3ffe6ff2cfb187e003c01ffe3ffe1e380000" },
                { "unlimitedinf_green.jpg",     "07e10fe00fe00ff00e600fe01ff03ff83ff837ec27c023c03ff83ff81ff00001" },
                { "wood-dark.jpg",              "d380ff80ff80df80fc92dd00fc11fc16f01ae616ec16ee16663e03300f33cf3d" },
            };

            Hasher hasher = new Hasher(Hasher.Algorithm.Blockhash);

            foreach (FileInfo fileInfo in Images)
                Assert.AreEqual(hashesExpected[fileInfo.Name],
                    hasher.ComputeHashS(fileInfo.OpenRead()).ToLowerInvariant(),
                    fileInfo.FullName);
        }

        [TestMethod]
        public void BlockhashHammingDistanceTest()
        {
            List<string> hashes = new List<string>
            {
                "deadbeef",
                "f00fba11",
                "42133742",
                "beefca47"
            };

            int[][] expectedHammingDistances = new int[][]
            {
                new int[]{ 0, 15, 18, 11 },
                new int[]{ 0, 15, 14 },
                new int[]{ 0, 21 },
                new int[]{ 0 }
            };

            for (int i = 0; i < hashes.Count; i++)
                for (int j = i; j < hashes.Count; j++)
                {
                    string left = hashes[i];
                    string right = hashes[j];
                    Assert.AreEqual(
                        expectedHammingDistances[i][j - i],
                        Blockhash.HammingDistance(
                            Enumerable.Range(0, left.Length / 2).Select(x => Convert.ToByte(left.Substring(x * 2, 2), 16)).ToArray(),
                            Enumerable.Range(0, right.Length / 2).Select(x => Convert.ToByte(right.Substring(x * 2, 2), 16)).ToArray()),
                        $"{left}<->{right}");
                }
        }

        [TestMethod]
        public void Crc32Test()
        {
            C.BinaryPackage.AssertExists();

            Dictionary<string, string> hashesExpected = new Dictionary<string, string>
            {
                { "astronomy-ngc-3766.jpg",     "781338ad" },
                { "burst.jpg",                  "e80c7502" },
                { "halo-5-cover.jpg",           "d6365e1c" },
                { "halo-5-cover-small.png",     "0f4ac35b" },
                { "halo-master-chief.jpg",      "46af6859" },
                { "landscape-mountains.jpeg",   "ca8da683" },
                { "milky-way-laser.jpg",        "97c04d9f" },
                { "skyrim.jpg",                 "1bc99a9b" },
                { "stargate-atlantis.jpg",      "623c2396" },
                { "unlimitedinf.ico",           "9539bf95" },
                { "unlimitedinf_green.jpg",     "5d9358de" },
                { "unlimitedinf-0100.png",      "d42a5773" },
                { "unlimitedinf-1024.png",      "05ca948f" },
                { "wood-dark.jpg",              "454fc5d0" },
            };

            Hasher hasher = new Hasher(Hasher.Algorithm.Crc32);

            foreach (FileInfo fileInfo in Images)
                Assert.AreEqual(hashesExpected[fileInfo.Name],
                    hasher.ComputeHashS(fileInfo.OpenRead()).ToLowerInvariant(),
                    fileInfo.FullName);
        }

        [TestMethod]
        public void MD5Test()
        {
            C.BinaryPackage.AssertExists();

            Dictionary<string, string> hashesExpected = new Dictionary<string, string>
            {
                { "astronomy-ngc-3766.jpg",     "87f6fa2640c450ca4f7b00c29debb63f" },
                { "burst.jpg",                  "b54189f2c337b46a01df94a0e016baf2" },
                { "halo-5-cover-small.png",     "bd07d878604c8a3fcf77b7dee0442d83" },
                { "halo-5-cover.jpg",           "37fbf02b8b058b50d0fdaddafb330eaf" },
                { "halo-master-chief.jpg",      "cd51e48f4c2079ea2fdddb996401a5f1" },
                { "landscape-mountains.jpeg",   "aef496b5177fa0f453c03a130a690fe6" },
                { "milky-way-laser.jpg",        "1e3401245ab7431ec2fa0d8523486b18" },
                { "skyrim.jpg",                 "500a420558aefeb4dbd784f8f1979e56" },
                { "stargate-atlantis.jpg",      "8ba2c41fcb0602069afb0fe5f60e399b" },
                { "unlimitedinf-0100.png",      "f4360e2bea3d84a11d64032da6222c96" },
                { "unlimitedinf-1024.png",      "af322cb0fd63094a6a1f13ad189cec5b" },
                { "unlimitedinf.ico",           "e0ee4792be023100e0e43b30c574fec3" },
                { "unlimitedinf_green.jpg",     "00f60bc1ec9bfb42aec40ae6be132f78" },
                { "wood-dark.jpg",              "49249fa4b3eaa8572272c2378cba8f11" },
            };

            Hasher hasher = new Hasher(Hasher.Algorithm.MD5);

            foreach (FileInfo fileInfo in Images)
                Assert.AreEqual(hashesExpected[fileInfo.Name],
                    hasher.ComputeHashS(fileInfo.OpenRead()).ToLowerInvariant(),
                    fileInfo.FullName);
        }

        [TestMethod]
        public void SHA1Test()
        {
            C.BinaryPackage.AssertExists();

            Dictionary<string, string> hashesExpected = new Dictionary<string, string>
            {
                { "astronomy-ngc-3766.jpg",     "28e6647943cb2c6a229a0e1f8fdde38cb5de5da4" },
                { "burst.jpg",                  "0919af827acb560be827c0cdd89d575a90e8a948" },
                { "halo-5-cover-small.png",     "147ef0aea39cb64b5f08d668a2690b29d35bd4a0" },
                { "halo-5-cover.jpg",           "dcf5f83fde3704ae6e995ab00afe3e40b1a8adc1" },
                { "halo-master-chief.jpg",      "a906539a9836b38f93fbf5ef761c4cb9c669f95c" },
                { "landscape-mountains.jpeg",   "a3f59acc29d5564e22bcb6ea8c31b93d3c95811e" },
                { "milky-way-laser.jpg",        "665e74d89448433530fd84080d61fb0fc501e6c9" },
                { "skyrim.jpg",                 "8245490b3c5348a69dfc4258ac5ca5c3077d3581" },
                { "stargate-atlantis.jpg",      "99abd49b7b6a8b33dcbc804c39c88a456527aed8" },
                { "unlimitedinf-0100.png",      "bb7287b225788cb7b00035f87a0e32d74ac0628e" },
                { "unlimitedinf-1024.png",      "af248b0f53cfd194dcb8e03d36d98a3dcb9d9beb" },
                { "unlimitedinf.ico",           "336e0ac17f7c5aeada007bb62deef3566c45ff79" },
                { "unlimitedinf_green.jpg",     "9b9b86eab6930da59d48dd710b4742b387663fb7" },
                { "wood-dark.jpg",              "b9458b212d11f1c9380fe22f48cc42b1306b05d1" },
            };

            Hasher hasher = new Hasher(Hasher.Algorithm.SHA1);

            foreach (FileInfo fileInfo in Images)
                Assert.AreEqual(hashesExpected[fileInfo.Name],
                    hasher.ComputeHashS(fileInfo.OpenRead()).ToLowerInvariant(),
                    fileInfo.FullName);
        }

        [TestMethod]
        public void SHA256Test()
        {
            C.BinaryPackage.AssertExists();

            Dictionary<string, string> hashesExpected = new Dictionary<string, string>
            {
                { "astronomy-ngc-3766.jpg",     "aab2e06ad4dfeff2cbca5e35cee92a15ba19952770266aadbf2f1507f95ceb55" },
                { "burst.jpg",                  "dfe6e1f7dad7de0403a4a5be27b5763f6ef7acbe42eeb82835aa32fbf36e36b2" },
                { "halo-5-cover-small.png",     "aaead67d1bb10140d70a68a599fae2d783ad29d283088e240b5c6f79874a2c18" },
                { "halo-5-cover.jpg",           "cae1fa9b51f152cc76667fe0f447069a913cc52e3f526c4067e99ec74e54bba2" },
                { "halo-master-chief.jpg",      "2be5abf0982b5bd6d35cd8003c01571f608a18afd738c0356f5ff6e9cc2a20c1" },
                { "landscape-mountains.jpeg",   "d0f20f155eb56e7aa7dd7b6f8d794cac0ef18474ce5f3c3884bbea26d035aad7" },
                { "milky-way-laser.jpg",        "501d4baab4451f7eced3bf9712775a94feadf0e17f72fe6202862090a25b9b2a" },
                { "skyrim.jpg",                 "ddab25934aae2f716220467453cc164e5aa4dc92450ce7d335cefd84e67338e3" },
                { "stargate-atlantis.jpg",      "2cb14dce52b067ab446bcd960772bbe4af72123aecfeabd44a2eb757494940fa" },
                { "unlimitedinf-0100.png",      "22964487106827d3c9e51c969039ef261b0634c30a90bf07400215d41c59f7bd" },
                { "unlimitedinf-1024.png",      "be4bfaff3b9f3fa9a766ebb819e5f7eb4b13681e12d5a056938641abcf2ffc8f" },
                { "unlimitedinf.ico",           "7908d9176f1ba0df8034cd3b3b0aadf7aaeb51c73e93a7b69b37df9b894d6a9d" },
                { "unlimitedinf_green.jpg",     "4405d45cec0c622a26bc9b0a61e0a72b8b442522033c4821cd57a6424780594b" },
                { "wood-dark.jpg",              "28f9483502c3dadc13f2f223578b87274c3d73e405dc6e1243b184cffe6ab1ff" },
            };

            Hasher hasher = new Hasher(Hasher.Algorithm.SHA256);

            foreach (FileInfo fileInfo in Images)
                Assert.AreEqual(hashesExpected[fileInfo.Name],
                    hasher.ComputeHashS(fileInfo.OpenRead()).ToLowerInvariant(),
                    fileInfo.FullName);
        }

        [TestMethod]
        public void SHA512Test()
        {
            C.BinaryPackage.AssertExists();

            Dictionary<string, string> hashesExpected = new Dictionary<string, string>
            {
                { "astronomy-ngc-3766.jpg",     "f4d9a14116532a931a08713aeda8a65fa16c79061661b3dfcfe7646f1bad5856abdfd3891a783246f871e2591ba8bd883313030b0fe620a229b958fb21d59dfa" },
                { "burst.jpg",                  "753129369b09883f29b00925126478a4a119e20535b436636b4478573431777ad6590259f72beb4e6cc1b69555a721cce2a4704079bee10add0af7037c173efd" },
                { "halo-5-cover-small.png",     "e33f55645c3e94a08081883255f5e9655a2064f64712065d9448958ee5b8b232e027efc0f15ae445bac530d7e06cf40c0581ec44ab97e42043d66df8dc942ee7" },
                { "halo-5-cover.jpg",           "e8c56cb2bd3655593e3a48cbf5914672888174627e8f5e8a1a47cc561d0228e4610206e53c810c44af2204ab2415357b68904b7ad0ab1bcf3a9db6d5c764fe7f" },
                { "halo-master-chief.jpg",      "981ba09d3d6bfe8b08090610fb77c92db1be60913f3d23299a08c03a56614d51f8fbc9b3f47cd502cee57e33c5879632bf0b03706dffac47b94f218a38704727" },
                { "landscape-mountains.jpeg",   "de4dfc5fdd971b36eb477c5c4c77a9e4b873fe5cd483a05e15fc304aff93eb3b264bca3dc4f3e61e34b01698340adb62ac0f558105b99b41f8e855cd845f9a70" },
                { "milky-way-laser.jpg",        "e3eb7a95f67a0aba552fa8755b4803e2548c9d680ed76cdffd5e8bac481d8c4f42e36929b877baed0706ddfb65e7b6ea91909c6190831bf44c8187d47a4d4111" },
                { "skyrim.jpg",                 "4bc2fdd19f6b67d531cfbcaaa415806b170a0ad0a059bc7c0f020c1cfe24e08639ed15690a79e90c89c295e5aea91df067a97302adbdc41177056fd853258893" },
                { "stargate-atlantis.jpg",      "22c34b47b0b073d3e13afb78799045a37a2655da8c7007ac3fde6ef0ac5f4fe7c3622ae460383e469ff03e25daa1d90af02487251c0fc6d13b8a69b6f0e8fac7" },
                { "unlimitedinf-0100.png",      "42c660c1ebbcf6358194927c4f57de0b4dbbb4e05039897e2191b9c5fdfeeac336b1ecc102ab9fbc16fb5155988343921f7cceee95c32386c14980ec8c3d2663" },
                { "unlimitedinf-1024.png",      "d212bb63228ffdff7e3d7416cb1340bdcedc22772f3d0491e8391ef8bbec48ff184cad86ddf071a46adaf837f11214c6272ff136e12967eaba3b899131acb85f" },
                { "unlimitedinf.ico",           "8e097d2cf0aaaab0d580360f090cdc63e047a51cbe2aaef87fb0feb9748836dd6b788de15ca84107b78f6f49dd2acf5ffcbfa031b6d1b3c5fd488f52d54922f2" },
                { "unlimitedinf_green.jpg",     "cff134cdd4fc9746c24077093b8c4c1e631d826dfa15f5f9dce2866f7b018c21d91af0c58ea7752a62b9c5f6c3a334616722e4134913b74f06912a81b5e051d4" },
                { "wood-dark.jpg",              "6456f2c2c22c0a9ad58196a7db62155997bb76664b164990215b2073f83931a5afa1644305a5141806a1d0c9ad08c2d36e6ab7769b7bbb52beb99b8591d5bd2c" },
            };

            Hasher hasher = new Hasher(Hasher.Algorithm.SHA512);

            foreach (FileInfo fileInfo in Images)
                Assert.AreEqual(hashesExpected[fileInfo.Name],
                    hasher.ComputeHashS(fileInfo.OpenRead()).ToLowerInvariant(),
                    fileInfo.FullName);
        }
    }
}
