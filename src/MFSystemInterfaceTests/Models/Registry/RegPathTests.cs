using System;
using System.Collections.Generic;
using MFSystemInterface.Models.Registry;
using MFSystemInterface.Services.PInvoke.Enums;
using MFSystemInterface.Services.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace MFSystemInterfaceTests.Models.Registry
{
    [TestClass()]
    public class RegPathTests
    {
        [TestMethod()]
        public void ToStringTest()
        {
            var tests = FileUtil.ImportJson<ICollection<RegPath>>("Test\\Models\\Registry\\RegPath.json");
            foreach (var test in tests)
            {
                var result = new RegPath(test.ToString());
                Assert.AreEqual(test, result);
            }
        }
        
    }
}