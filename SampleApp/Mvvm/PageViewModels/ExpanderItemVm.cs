using FunctionZero.TreeZero;
using Microsoft.Maui.Graphics;
using SampleApp.Mvvm.ViewModels;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SampleApp.Mvvm.PageViewModels
{
    public class ExpanderItemVm : Node
    {
        public ExpanderItemVm(string title, Action<object> action)
        {
            Title = title;
            Action = action ?? ((arg) => { });
        }

        public string Title { get; }
        public Action<object> Action { get; }
    }

    public class Node : BaseVm
    {
        private Node _parent;
        private bool _hasChildren;

        private int Depth
        {
            get
            {
                if (Parent == null) return 0;
                return Parent.Depth + 1;
            }
        }

        public ObservableCollection<Node> Children { get; }

        public Node Parent { get => _parent; set => SetProperty(ref _parent, value); }
        public bool HasChildren { get => _hasChildren; set => SetProperty(ref _hasChildren, value); }
        public Node()
        {
            Children = new();
            Children.CollectionChanged += Children_CollectionChanged;
        }

        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Node item in e.NewItems)
                    {
                        if (item.Parent != null)
                            item.Parent.Children.Remove(item);

                        // item.Parent should now be null.
                        item.Parent = this;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (Node item in e.OldItems)
                    {
                        if (item.Parent == null)
                            throw new InvalidOperationException("Node.Children_CollectionChanged NotifyCollectionChangedAction.Remove");
                        item.Parent = null;
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Reset:
                    throw new NotImplementedException();
            }
            HasChildren = Children.Count > 0;
        }
    }
}
