using FunctionZero.TreeListItemsSourceZero;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;



namespace FunctionZero.Maui.Controls
{
    // https://stackoverflow.com/questions/68639893/maui-project-cannot-add-a-new-page

    public partial class TreeNodeZero
    {
        static int nextId = 0;

        int MyId { get; }
        static int DeadCount { get; set; }

        public TreeNodeZero()
        {
            InitializeComponent();

            MyId = nextId++;
        }
        ~TreeNodeZero()
        {
            DeadCount++;
        }

        public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(TreeNodeZero), false, BindingMode.TwoWay, null, IsExpandedChanged);

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        private static void IsExpandedChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            
        }

        protected override void OnParentChanged()
        {
            Debug.WriteLine($"TreeNodeZero::ID:{MyId}, Parent:{Parent}, DeadCount:{DeadCount}");
            if (Parent == null)
                BindingContext = null;
            base.OnParentChanged();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        { 
            base.OnPropertyChanged(propertyName);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }
    }
}