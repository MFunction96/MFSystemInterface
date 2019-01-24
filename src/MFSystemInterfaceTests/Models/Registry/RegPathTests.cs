using MFStandardUtil.Utils;
using MFSystemInterface.Models.Registry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MFSystemInterfaceTests.Models.Registry
{
    [TestClass()]
    public class RegPathTests
    {
        [TestMethod()]
        public void ToStringTest()
        {
            var tests = FileUtil.ImportJson<ICollection<RegPath>>("Test\\Models\\Registry\\RegPath\\ToString.json");
            foreach (var test in tests)
            {
                var result = new RegPath(test.ToString());
                Assert.AreEqual(test, result, $"Expected:\n{test}\nActual:\n{result}");
            }
        }
        
    }
}