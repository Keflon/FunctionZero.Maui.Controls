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

            // TODO: When Parent changes, manage TreeViewZero and update Actual Indent = indent * multiplier
            // TODO: When BindingContext changes, manage Indent and update Actual Indent = indent * multiplier
            // TODO: 
            // TODO: 
        }
        ~TreeNodeZero()
        {
            DeadCount++;
        }

        public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(TreeNodeZero), false, BindingMode.TwoWay);

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public static readonly BindableProperty ActualIndentProperty = BindableProperty.Create(nameof(ActualIndent), typeof(float), typeof(TreeNodeZero), (float)0.0, BindingMode.OneWay);

        public float ActualIndent
        {
            get { return (float)GetValue(ActualIndentProperty); }
            set { SetValue(ActualIndentProperty, value); }
        }



        private TreeViewZero _ownerTree;
        protected async override void OnParentChanged()
        {
            Debug.WriteLine($"TreeNodeZero::ID:{MyId}, Parent:{Parent}, DeadCount:{DeadCount}");
            base.OnParentChanged();
            if (Parent == null)
            {
                // TODO: Why was I doing this? If it is necessary, clear it rather than null it.
                BindingContext = null;

                _ownerTree = null;
            }
            else
            {
                _ownerTree = (TreeViewZero)Parent.Parent;

                if ((BindingContext != null) && (_ownerTree != null))
                {
                    var thing = (TreeNodeContainer<object>)BindingContext;
                    ActualIndent = (thing.Indent - 1) * (float)_ownerTree.IndentMultiplier;
                }
            }
        }


        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext == null)
            {
                Debug.WriteLine("Null BindingContext");
            }

            if ((BindingContext != null) && (_ownerTree != null))
            {
                var thing = (TreeNodeContainer<object>)BindingContext;
                ActualIndent = (thing.Indent - 1) * (float)_ownerTree.IndentMultiplier;
            }
        }
    }
}