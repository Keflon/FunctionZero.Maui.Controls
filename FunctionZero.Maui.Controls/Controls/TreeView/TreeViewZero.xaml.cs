using FunctionZero.Maui.Controls;
using FunctionZero.Maui.Services.Cache;
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

        public IEnumerable TreeChildren => _rootContainer?.TreeNodeChildren;

        public static readonly BindableProperty ItemHeightProperty = BindableProperty.Create(nameof(ItemHeight), typeof(float), typeof(TreeViewZero), (float)30.0, BindingMode.OneWay, null);

        public float ItemHeight
        {
            get { return (float)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public static readonly BindableProperty ScrollOffsetProperty = BindableProperty.Create(nameof(ScrollOffset), typeof(float), typeof(TreeViewZero), (float)0.0, BindingMode.OneWay, null, null, ScrollOffsetChanged);

        public float ScrollOffset
        {
            get { return (float)GetValue(ScrollOffsetProperty); }
            set { SetValue(ScrollOffsetProperty, value); }
        }

        private static void ScrollOffsetChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (TreeViewZero)bindable;
        }

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(TreeViewZero), null, BindingMode.TwoWay, null, SelectedItemChanged);

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private static void SelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (TreeViewZero)bindable;
        }

        public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(nameof(SelectedItems), typeof(IList), typeof(TreeViewZero), null, BindingMode.TwoWay, null, SelectedItemsChanged);

        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }
        private static void SelectedItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (TreeViewZero)bindable;
        }

        public static readonly BindableProperty SelectionModeProperty = BindableProperty.Create(nameof(SelectionMode), typeof(SelectionMode), typeof(TreeViewZero), SelectionMode.None, BindingMode.TwoWay, null, SelectionModeChanged);

        public SelectionMode SelectionMode
        {
            get { return (SelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        private static void SelectionModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (TreeViewZero)bindable;
        }

        public TreeViewZero()
        {
            InitializeComponent();
            SelectedItems = new ObservableCollection<object>();
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
                self.TheListView.ItemsSource = null;
            }

            if (newValue != null)
            {
                var rootNode = newValue;       // Get your tree data from somewhere
                self._rootContainer = new TreeItemsSourceManager<object>(self.IsRootVisible, rootNode, self.GetCanHaveChildren, self.GetChildrenForNode);
                self._rootContainer.NodeChanged += self._rootContainer_NodeChanged;
                self.TheListView.ItemsSource = self._rootContainer.TreeNodeChildren;

                self.TryAttach(self._rootContainer);

                ((INotifyCollectionChanged)self._rootContainer.TreeNodeChildren).CollectionChanged += TreeViewZero_CollectionChanged;
                self.OnPropertyChanged(nameof(TreeChildren));
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

        public static readonly BindableProperty TreeItemControlTemplateProperty = BindableProperty.Create("TreeItemControlTemplate", typeof(ControlTemplate), typeof(TreeViewZero), null, propertyChanged: OnTreeItemControlTemplateChanged);

        public ControlTemplate TreeItemControlTemplate
        {
            get { return (ControlTemplate)GetValue(TreeItemControlTemplateProperty); }
            set { SetValue(TreeItemControlTemplateProperty, value); }
        }

        private static void OnTreeItemControlTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (TreeViewZero)bindable;
            //self.Resources["FunctionZero.Maui.Controls.TreeNodeZero.defaultControlTemplate"] = newValue;

            self.ListItemStyle.Setters[0].Value = newValue;

        }

        public static readonly BindableProperty IndentMultiplierProperty = BindableProperty.Create("IndentMultiplier", typeof(double), typeof(TreeViewZero), 15D);

        public double IndentMultiplier
        {
            get { return (double)GetValue(IndentMultiplierProperty); }
            set { SetValue(IndentMultiplierProperty, value); }
        }

        public static readonly BindableProperty ItemContainerStyleProperty = BindableProperty.Create(nameof(ItemContainerStyle), typeof(Style), typeof(TreeViewZero), null, BindingMode.OneWay, null, ItemContainerStyleChanged);

        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        private static void ItemContainerStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (TreeViewZero)bindable;
        }

        private static void TreeViewZero_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var thing = (ReadOnlyObservableCollection<TreeNodeContainer<object>>)sender;

            Debug.WriteLine($"Count:{thing.Count}");
        }

        private static int _diff = 0;

        private (bool setValue, bool attachedInpc) TryAttach(TreeNodeContainer<object> treeNodeContainer)
        {
            _diff++;

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
            _diff--;

            Debug.WriteLine($"Diff:{_diff}");
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
            // If no property is specified for the children, see if the node is IEnumerable.
            // TODO: Test this.
            else if (node is IEnumerable children)
                return children;
            
            return Enumerable.Empty<object>();
        }

        private TreeItemDataTemplate GetTemplateForNode(object node)
        {
            //return TreeItemTemplate.OnSelectTemplate(node);
            return (TreeItemDataTemplate)TreeItemTemplate.OnSelectTemplateProvider(node);
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