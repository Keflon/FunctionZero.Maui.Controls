using FunctionZero.Maui.Controls;
using FunctionZero.TreeListItemsSourceZero;
using FunctionZero.TreeZero;
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
using System.Text;
using System.Threading.Tasks;



namespace FunctionZero.Maui.Controls
{
    // https://stackoverflow.com/questions/68639893/maui-project-cannot-add-a-new-page

    public partial class TreeViewZero
    {
        private static char[] _dot = new[] { '.' };
        private TreeItemsSourceManager<object> _rootContainer;

        public TreeViewZero()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TreeItemTemplateProperty = BindableProperty.Create("TreeItemTemplate", typeof(TemplateProvider), typeof(TreeViewZero), null, propertyChanged: OnItemTemplateChanged);

        public TemplateProvider TreeItemTemplate
        {
            get { return (TemplateProvider)GetValue(TreeItemTemplateProperty); }
            set { SetValue(TreeItemTemplateProperty, value); }
        }

        private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = bindable as TreeViewZero;
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(object), typeof(TreeViewZero), null, propertyChanged: OnItemsSourceChanged);

        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (TreeViewZero)bindable;

            if (oldValue != null)
            {
                self.TryDetach(self._rootContainer);
                self._rootContainer.NodeChanged -= self._rootContainer_NodeChanged;
                self.TheListView.ItemsSource = Enumerable.Empty<object>();
            }

            if (newValue != null)
            {
                var rootNode = newValue;       // Get your tree data from somewhere
                self._rootContainer = new TreeItemsSourceManager<object>(self.IsRootVisible, rootNode, self.GetCanHaveChildren, self.GetChildrenForNode);
                self._rootContainer.NodeChanged += self._rootContainer_NodeChanged;
                self.TheListView.ItemsSource = self._rootContainer.TreeNodeChildren;

                self.TryAttach(self._rootContainer);

                ((INotifyCollectionChanged)self._rootContainer.TreeNodeChildren).CollectionChanged += TreeViewZero_CollectionChanged;
            }
        }

        public static readonly BindableProperty IsRootVisibleProperty = BindableProperty.Create(nameof(IsRootVisible), typeof(bool), typeof(TreeViewZero), true, propertyChanged: OnIsRootVisibleChanged);

        public bool IsRootVisible
        {
            get { return (bool)GetValue(IsRootVisibleProperty); }
            set { SetValue(IsRootVisibleProperty, value); }
        }

        private static void OnIsRootVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (TreeViewZero)bindable;
            if (self._rootContainer != null)
            {
                self._rootContainer.IsTreeRootShown = (bool)newValue;
                if (self._rootContainer.IsExpanded)
                {
                    // If the root visibility changes, re-align any children if they are visible.
                    self._rootContainer.IsExpanded = false;
                    self._rootContainer.IsExpanded = true;
                }
            }
        }

        public static readonly BindableProperty TreeItemContainerStyleProperty = BindableProperty.Create("TreeItemContainerStyle", typeof(Style), typeof(TreeViewZero), null, propertyChanged: OnTreeItemContainerStyleChanged);

        public Style TreeItemContainerStyle
        {
            get { return (Style)GetValue(TreeItemContainerStyleProperty); }
            set { SetValue(TreeItemContainerStyleProperty, value); }
        }

        private static void OnTreeItemContainerStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (TreeViewZero)bindable;
            self.Resources["FunctionZero.Maui.Controls.TreeNodeZero"] = newValue;
        }



        public static readonly BindableProperty IndentMultiplierProperty = BindableProperty.Create("IndentMultiplier", typeof(double), typeof(TreeViewZero), 15D, propertyChanged: OnIndentMultiplierChanged);

        public double IndentMultiplier
        {
            get { return (double)GetValue(IndentMultiplierProperty); }
            set { SetValue(IndentMultiplierProperty, value); }
        }

        private static void OnIndentMultiplierChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (TreeViewZero)bindable;
        }

        private static void TreeViewZero_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var thing = (ReadOnlyObservableCollection<TreeNodeContainer<object>>)sender;

            Debug.WriteLine($"Count:{thing.Count}");
        }

        private (bool setValue, bool attachedInpc) TryAttach(TreeNodeContainer<object> treeNodeContainer)
        {
            bool didSetValue = false;
            bool didattachInpc = false;

            var qualifiedIsExpandedPropertyName = GetTemplateForNode(treeNodeContainer.Data).IsExpandedPropertyName;

            if (TryGetPropertyValue<bool>(treeNodeContainer.Data, qualifiedIsExpandedPropertyName, out var isDataExpanded))
            {
                didSetValue = true;
                treeNodeContainer.IsExpanded = isDataExpanded;

                if (treeNodeContainer.Data is INotifyPropertyChanged inpc)
                {
                    didattachInpc = true;
                    _lookup.Add(inpc, new Target(treeNodeContainer, qualifiedIsExpandedPropertyName));
                    inpc.PropertyChanged += Inpc_PropertyChanged;
                }
            }
            return (didSetValue, didattachInpc);
        }

        private bool TryDetach(TreeNodeContainer<object> treeNodeContainer)
        {
            if (treeNodeContainer.Data is INotifyPropertyChanged inpc)
            {
                _lookup.Remove(inpc);
                inpc.PropertyChanged -= Inpc_PropertyChanged;
                return true;
            }
            return false;
        }

        //bool _isBusy = false;
        private void _rootContainer_NodeChanged(object sender, TreeNodeContainerEventArgs<object> e)
        {
            var treeNodeContainer = e.Node;


            Debug.WriteLine($"Node Changed: {e.Node.Data}, {e.Action}");

            switch (e.Action)
            {
                case NodeAction.Added:
                    {
                        TryAttach(treeNodeContainer);
                    }
                    break;
                case NodeAction.Removed:
                    {
                        TryDetach(treeNodeContainer);
                    }
                    break;
                case NodeAction.IsExpandedChanged:
                    {
                        var qualifiedIsExpandedPropertyName = GetTemplateForNode(treeNodeContainer.Data).IsExpandedPropertyName;
                        TrySetPropertyValue(treeNodeContainer.Data, qualifiedIsExpandedPropertyName, treeNodeContainer.IsExpanded);
                    }
                    break;
                case NodeAction.IsVisibleChanged:
                    break;
            }

            #region 

            //if(_isBusy == false)
            //{
            //    TheListView.BatchBegin();
            //    _isBusy = true;
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        TheListView.BatchCommit();
            //        _isBusy = false;
            //    });

            //}

            #endregion
        }

        record Target(TreeNodeContainer<object> container, string qualifiedIsExpandedPropertyName);

        Dictionary<object, Target> _lookup = new Dictionary<object, Target>();
        private void Inpc_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_lookup.TryGetValue(sender, out Target target))
            {
                if (e.PropertyName == target.qualifiedIsExpandedPropertyName)
                {
                    if (TryGetPropertyValue<bool>(target.container.Data, target.qualifiedIsExpandedPropertyName, out var newDataValue))
                        target.container.IsExpanded = newDataValue;
                }
            }
        }

        //private void InpcChanged(TreeNodeContainer<object> container, PropertyChangedEventArgs e, string qualifiedIsExpandedPropertyName)
        //{
        //    if (e.PropertyName == qualifiedIsExpandedPropertyName)
        //    {
        //        if (TryGetPropertyValue<bool>(container.Data, qualifiedIsExpandedPropertyName, out var newDataValue))
        //            container.IsExpanded = newDataValue;
        //    }
        //}

        private bool GetCanHaveChildren(object node)
        {
            foreach (var item in GetChildrenForNode(node))
                return true;

            return false;
        }

        private IEnumerable GetChildrenForNode(object node)
        {
            var qualifiedChildrenPropertyName = GetTemplateForNode(node).ChildrenPropertyName;

            if (!string.IsNullOrEmpty(qualifiedChildrenPropertyName))
            {
                // (node) => node.Children
                var hostInfo = GetPropertyInfo(node, qualifiedChildrenPropertyName);
                if (hostInfo != (null, null))
                {
                    var value = hostInfo.info.GetValue(hostInfo.host);
                    return value as IEnumerable;
                }
            }
            return Enumerable.Empty<object>();
        }

        private TreeItemDataTemplate GetTemplateForNode(object node)
        {
            return TreeItemTemplate.OnSelectTemplate(node);
        }

        protected (object host, PropertyInfo info) GetPropertyInfo(object host, string qualifiedName)
        {
            var bits = qualifiedName.Split(_dot);

            PropertyInfo pi = null;
            object nextHost = host;
            foreach (var bit in bits)
            {
                host = nextHost;

                // Get info for the property
                pi = host.GetType().GetProperty(bit, BindingFlags.Public | BindingFlags.Instance);
                if (pi == null || pi.CanRead == false)
                    return (null, null);

                nextHost = pi.GetValue(host);
            }
            return (host, pi);
        }

        protected bool TrySetPropertyValue(object host, string qualifiedName, object newValue)
        {
            var info = GetPropertyInfo(host, qualifiedName);
            if (info != (null, null))
            {
                info.info.SetValue(info.host, newValue);
                return true;
            }
            return false;
        }

        protected bool TryGetPropertyValue<T>(object host, string qualifiedName, out T result)
        {
            var info = GetPropertyInfo(host, qualifiedName);
            if (info != (null, null))
            {
                result = (T)info.info.GetValue(info.host);
                return true;
            }
            result = default(T);
            return false;
        }
    }
}