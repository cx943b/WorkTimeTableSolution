using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WorkTimeTable.Controls
{
    public class YearTextBox : ContentControl
    {
        public int Year
        {
            get { return (int)GetValue(YearProperty); }
            set { SetValue(YearProperty, value); }
        }

        public string YearLabel
        {
            get { return (string)GetValue(YearLabelProperty); }
            set { SetValue(YearLabelProperty, value); }
        }

        static YearTextBox() => DefaultStyleKeyProperty.OverrideMetadata(typeof(YearTextBox), new FrameworkPropertyMetadata(typeof(YearTextBox)));



        public static readonly DependencyProperty YearProperty = DependencyProperty.Register("Year", typeof(int), typeof(YearTextBox), new UIPropertyMetadata(DateTime.Now.Year));
        public static readonly DependencyProperty YearLabelProperty = DependencyProperty.Register("YearLabel", typeof(string), typeof(YearTextBox), new UIPropertyMetadata("Year"));
    }
}
