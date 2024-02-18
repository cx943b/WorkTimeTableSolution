using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeTable.Infrastructure;

namespace WorkTimeTable.Tests
{
    [TestClass]
    public class ValidationRuleTests
    {
        [TestMethod]
        public void BirthDateValidationRuleTest()
        {
            WorkerBirthDateValidationRule rule = new WorkerBirthDateValidationRule();
            var result = rule.Validate("860811", CultureInfo.CurrentCulture);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);

            result = rule.Validate("129933", CultureInfo.CurrentCulture);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
        }
    }
}
