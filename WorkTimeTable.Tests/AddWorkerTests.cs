using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WorkTimeTable.Tests
{
    [TestClass]
    public class AddWorkerTests
    {
        [TestMethod]
        public void GetWorkerColors()
        {
            PropertyInfo[] propInfos = typeof(Colors).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            var dicColors = propInfos.Select(pi => new KeyValuePair<string, Color>( pi.Name, (Color)ColorConverter.ConvertFromString(pi.Name))).ToDictionary();

            Debug.WriteLine(propInfos);
        }
    }
}
