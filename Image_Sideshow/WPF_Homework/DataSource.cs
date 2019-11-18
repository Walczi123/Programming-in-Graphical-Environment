using System;
using System.Collections.Generic;
using System.Windows;

namespace WPF_Homework
{
    class DataSource : DependencyObject
    {
        public static readonly DataSource Instance = new DataSource();
        private DataSource() { }
        public bool IsSelected { get { return (bool)GetValue(IsSelectedProperty); } set { SetValue(IsSelectedProperty, value); } }
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(DataSource), new UIPropertyMetadata());
    }
}
