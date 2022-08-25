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
        protected override void OnParentChanged()
        {
            try
            {
                Debug.WriteLine($"TreeNodeZero::ID:{MyId}, Parent:{Parent}, DeadCount:{DeadCount}");
                base.OnParentChanged();
                if (Parent == null)
                {
                    _ownerTree = null;
                }
                else
                {
                    _ownerTree = (TreeViewZero)Parent.Parent;

                    if ((BindingContext != null) && (_ownerTree != null))
                    {
                        var nodeContainer = (TreeNodeContainer<object>)BindingContext;
                        DoTheThing(nodeContainer);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private TreeNodeContainer<object> _oldNodeData;

        private void DoTheThing(TreeNodeContainer<object> nodeData)
        {
            try
            {
                if (_oldNodeData != nodeData)
                {
                    ActualIndent = (nodeData.Indent - 1) * (float)_ownerTree.IndentMultiplier;
                    var template = _ownerTree.TreeItemTemplate.OnSelectTemplate(nodeData.Data);
                    var content = (View)template.ItemTemplate.CreateContent();
                    this.Content = content;
                    content.BindingContext = nodeData.Data;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            _oldNodeData = nodeData;
        }

        private TreeNodeContainer<object> _oldBindingContext;

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (_oldBindingContext != (TreeNodeContainer<object>)BindingContext)
            {
                if (_oldBindingContext != null)
                {
                    if (BindingContext != null)
                    {
                        Debug.WriteLine("Swapped BindingContext");
                    }
                }
                _oldBindingContext = (TreeNodeContainer<object>)BindingContext;
            }
            if (BindingContext == null)
            {
                Debug.WriteLine("Null BindingContext");
                //if (Content != null)
                //    Content.BindingContext = null;
            }

            if ((BindingContext != null) && (_ownerTree != null))
            {
                var thing = (TreeNodeContainer<object>)BindingContext;
                DoTheThing(thing);
            }
        }
    }
}