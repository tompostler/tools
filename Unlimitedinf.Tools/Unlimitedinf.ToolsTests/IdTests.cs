using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unlimitedinf.Tools;

namespace Unlimitedinf.ToolsTests
{
    [TestClass]
    public class IdTests
    {
        [TestMethod]
        public void IdDeserialization()
        {
            List<Id> ids = JsonConvert.DeserializeObject<List<Id>>(@"[ ""0000000A"" ]");
        }
    }
}
