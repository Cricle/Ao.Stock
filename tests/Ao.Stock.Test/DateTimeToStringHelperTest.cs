using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ao.Stock.Test
{
    [TestClass]
    public class DateTimeToStringHelperTest
    {
        [TestMethod]
        public void ToStringTime()
        {
            var dt = DateTime.Parse("2023-2-22 12:01:01");
            var buffer=DateTimeToStringHelper.ToTimeString(dt);
            Assert.AreEqual("12:01:01", buffer);
        }
        [TestMethod]
        public void ToStringDate()
        {
            var dt = DateTime.Parse("2023-2-22 12:01:01");
            var buffer = DateTimeToStringHelper.ToDateString(dt);
            Assert.AreEqual("2023-02-22", buffer);
        }
        [TestMethod]
        public void ToStringFull()
        {
            var dt = DateTime.Parse("2023-2-22 12:01:01");
            var buffer = DateTimeToStringHelper.ToFullString(dt);
            Assert.AreEqual("2023-02-22 12:01:01", buffer);
        }
    }
}
