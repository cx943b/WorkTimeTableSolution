using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Infrastructure.Converters;

namespace WorkTimeTable.Tests
{
    [TestClass]
    public class EnumToStringConverterTests
    {
        [TestMethod]
        public void ConvertTest()
        {
            EnumToStringConverter conv = new EnumToStringConverter();
            object weekDesc = conv.Convert(DayOfWeekFlag.Wednesday | DayOfWeekFlag.Monday, typeof(object) , typeof(DayOfWeekFlag), CultureInfo.CurrentCulture);

            string[] weekStrings = weekDesc.ToString()
                .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToArray();

            Assert.AreEqual(2, weekStrings.Length);
            Assert.IsTrue(Array.IndexOf(weekStrings, "Mon") >= 0);
            Assert.IsTrue(Array.IndexOf(weekStrings, "Wed") >= 0);
        }
    }
}
