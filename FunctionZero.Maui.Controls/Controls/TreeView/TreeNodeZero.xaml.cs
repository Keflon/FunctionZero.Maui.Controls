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
        private TreeViewZero _ownerTree;
        private TreeNodeContainer<object> _oldNodeData;

        int MyId { get; }
        static int DeadCount { get; set; }

        public TreeNodeZero()
        {
            InitializeComponent();
            MyId = nextId++;
            ParentChanged += TreeNodeZero_ParentChanged;
        }

        ~TreeNodeZero()
        {
            DeadCount++;

        }

        private void TreeNodeZero_ParentChanged(object sender, EventArgs e)
        {
            var node = (Element)sender;

            node.ParentChanged -= TreeNodeZero_ParentChanged;

            while (node.Parent != null)
            {
                if (node.Parent is TreeViewZero treeView)
                {
                    _ownerTree = treeView;

                    var thing = (TreeNodeContainer<object>)BindingContext;
                    DoTheThing(thing);

                    return;
                }
                node = node.Parent;
            }
            node.ParentChanged += TreeNodeZero_ParentChanged;
        }

        public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(TreeNodeZero), false, BindingMode.OneWay);

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

        Element _oldParent;
        protected override void OnParentChanged()
        {
            base.OnParentChanged();
            return;

            _ownerTree = GetTreeView(this);

            if (Parent == null)
            {
                Debug.WriteLine("Parent is null in TreeNodeZero::OnParentChanged");
                return;
            }

            if ((Parent != null) && (_oldParent != null))
            {
                Debug.WriteLine("Parent recycled is null in TreeNodeZero::OnParentChanged");
            }

            if (_ownerTree == null)
            {
                Debug.WriteLine("_ownerTree is null in TreeNodeZero::OnParentChanged");
            }
            if (BindingContext == null)
            {
                Debug.WriteLine("BindingContext is null in TreeNodeZero::OnParentChanged");
            }
            try
            {
                var thing = (TreeNodeContainer<object>)BindingContext;
                DoTheThing(thing);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TreeNodeZero::OnBindingContextChanged exception: {ex}");
            }

            return;
            try
            {
                Debug.WriteLine($"TreeNodeZero::ID:{MyId}, Parent:{Parent}, DeadCount:{DeadCount}");
                base.OnParentChanged();
                if (Parent.Parent == null)
                {
                    _ownerTree = null;
                }
                else
                {
                    _ownerTree = (TreeViewZero)Parent.Parent.Parent.Parent;

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
        public DataTemplate _contentTemplate;

        //private void DoTheThing(TreeNodeContainer<object> nodeData)
        //{
        //    try
        //    {
        //        if (_oldNodeData != nodeData)
        //        {
        //            ActualIndent = (nodeData.Indent - 1) * (float)_ownerTree.IndentMultiplier;
        //            var template = _ownerTree.TreeItemTemplate.OnSelectTemplate(nodeData.Data).ItemTemplate;

        //            if (_contentTemplate != template)
        //            {
        //                _contentTemplate = template;
        //                var content = (View)template.CreateContent();
        //                content.BindingContext = null;
        //                this.Content = content;
        //            }
        //            this.Content.BindingContext = nodeData.Data;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //    _oldNodeData = nodeData;
        //}

        private void DoTheThing(TreeNodeContainer<object> nodeData)
        {
            try
            {
                if (_oldNodeData != nodeData)
                {
                    ActualIndent = (nodeData.Indent - 1) * (float)_ownerTree.IndentMultiplier;
                    var template = _ownerTree.TreeItemTemplate.OnSelectTemplate(nodeData.Data).ItemTemplate;

                    if (_contentTemplate != template)
                    {
                        _contentTemplate = template;
                        var content = (View)template.CreateContent();
                        if (Content != null)
                            Content.BindingContext = null;
                        //this.UnapplyBindings();
                        this.Content = null;
                        this.Content = content;
                    }
                    this.Content.BindingContext = nodeData.Data;
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

            if (BindingContext == null)
            {
                Debug.WriteLine("Null BindingContext");
                //if (Content != null)
                //    Content.BindingContext = null;
            }
            else
            {
                //_ownerTree = GetTreeView(this);

            }

            if (_ownerTree != null)
            {
                Debug.WriteLine("_ownerTree isn't null in TreeNodeZero::OnBindingContextChanged");
            }

            if ((BindingContext != null) && (_ownerTree != null))
            {
                try
                {
                    var thing = (TreeNodeContainer<object>)BindingContext;
                    DoTheThing(thing);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"TreeNodeZero::OnBindingContextChanged exception: {ex}");
                }
            }
        }

        private TreeViewZero GetTreeView(Element current)
        {
            while (current != null)
            {
                if (current is TreeViewZero treeView)
                    return treeView;

                current = current.Parent;
            }
            return null;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}