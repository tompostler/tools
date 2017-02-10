﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Unlimitedinf.Tools.IntTests
{
    [TestClass]
    public class IdTests
    {
        [TestMethod]
        public void IdDeserialization()
        {
            Id id = JsonConvert.DeserializeObject<Id>(@" ""0000000A"" ");
        }

        private class IdList
        {
            public List<Id> Ids { get; set; } = new List<Id>();
        }
        [TestMethod]
        public void IdListDeserialization()
        {
            const string json = @"
{
  ""Ids"": [
    ""00000000"", ""00000001"", ""00000002"", ""00000003"", ""00000004"", ""00000005"", ""00000007"", ""00000008"",
    ""00000009"", ""0000000A"", ""0000000B"", ""0000000C"", ""0000000D"", ""0000000E"", ""0000000F"", ""00000010"",
    ""00000011"", ""00000012"", ""00000014"", ""00000015"", ""00000016"", ""00000017"", ""00000018"", ""00000019"",
    ""0000001A"", ""0000001B"", ""0000001C"", ""0000001D"", ""0000001E"", ""0000001F"", ""00000020"", ""00000021"",
    ""00000022"", ""00000024"", ""00000025"", ""00000026"", ""00000027"", ""00000028"", ""00000029"", ""0000002A"",
    ""0000002B"", ""0000002C"", ""0000002D"", ""0000002E"", ""0000002F"", ""00000030"", ""00000031"", ""00000032"",
    ""00000033"", ""00000034"", ""00000035"", ""00000036"", ""00000037""
  ]
}";
            IdList ids = JsonConvert.DeserializeObject<IdList>(json);
        }
    }
}
