using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Unlimitedinf.Tools.IntTests
{
    [TestClass]
    public class GenerateRandomTests
    {
        const int Loops = 1000;

        [TestMethod]
        public void BytesTest()
        {
            byte[] bytes = GenerateRandom.Bytes();

            for (int i = 0; i < Loops; i++)
            {
                byte[] oldBytes = bytes;
                bytes = GenerateRandom.Bytes();
                Assert.AreNotEqual(oldBytes, bytes);
            }

            // Nondefault param
            for (int i = 4; i < Loops; i++)
            {
                byte[] oldBytes = new byte[i];
                Array.Copy(bytes, oldBytes, i);
                bytes = GenerateRandom.Bytes(i + 1);

                Assert.AreEqual(i + 1, bytes.Length);

                bool same = true;
                for (int j = 0; j < i; j++)
                    if (oldBytes[j] != bytes[j])
                    {
                        same = false;
                        break;
                    }
                Assert.IsFalse(same);
            }
        }

        [TestMethod]
        public void TokenTest()
        {
            string token = GenerateRandom.Token();

            for (int i = 0; i < Loops; i++)
            {
                string oldToken = token;
                token = GenerateRandom.Token();
                Assert.AreNotEqual(oldToken, token);
            }

            // Nondefault param
            for (int i = 4; i < Loops; i++)
            {
                string oldToken = token;
                token = GenerateRandom.Token(i + 1);

                Assert.AreEqual(i + 1, token.Length);
                Assert.AreNotEqual(oldToken, token.Substring(0, i));
            }
        }

        [TestMethod]
        public void HexTokenTest()
        {
            string hexToken = GenerateRandom.HexToken();

            for (int i = 0; i < Loops; i++)
            {
                string oldHexToken = hexToken;
                hexToken = GenerateRandom.HexToken();
                Assert.AreNotEqual(oldHexToken, hexToken);
            }

            // Nondefault param
            for (int i = 4; i < Loops; i++)
            {
                string oldHexToken = hexToken;
                hexToken = GenerateRandom.HexToken(i + 1);

                Assert.AreEqual(i + 1, hexToken.Length);
                Assert.AreNotEqual(oldHexToken, hexToken.Substring(0, i));
            }
        }
    }
}