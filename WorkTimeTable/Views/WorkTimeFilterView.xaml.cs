﻿using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorkTimeTable.ViewModels;

namespace WorkTimeTable.Views
{
    /// <summary>
    /// WorkTimeFilterView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WorkTimeFilterView : UserControl
    {
        static readonly int[] _targetMonts = Enumerable.Range(1, 12).ToArray();
        public static readonly DependencyPropertyKey TargetMonthsProperty = DependencyProperty.RegisterReadOnly(nameof(TargetMonths), typeof(IEnumerable<int>), typeof(WorkTimeFilterView), new PropertyMetadata(_targetMonts));

        public IEnumerable<int> TargetMonths => (IEnumerable<int>)GetValue(TargetMonthsProperty.DependencyProperty);
        public WorkTimeFilterView()
        {
            InitializeComponent();

            if(!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                DataContext = Ioc.Default.GetService<WorkTimeFilterViewModel>();
        }
    }
}