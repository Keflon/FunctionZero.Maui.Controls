using FunctionZero.TreeListItemsSourceZero;

namespace FunctionZero.Maui.Controls
{
    public class TreeNodeSpacer : TemplatedView
    {
        private TreeViewZero _treeView;

        public TreeNodeSpacer()
        {
            TemplateBinding f = new TemplateBinding();

            BackgroundColor = Colors.Transparent;
            //Margin = new Thickness(4);
            InputTransparent = true;
            SetBinding(BindingContextProperty, f);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _treeView = GetTreeViewForElement((Element)this);

            var listItem = (ListItemZero)BindingContext;
            // This instance belongs to a ListItemZero that draws it using a ControlTemplate that is tailored for a tree-node.
            // As the instance will ALWAYS have its BindingContext 
            listItem.BindingContextChanged += ListItem_BindingContextChanged;
        }

        private void ListItem_BindingContextChanged(object sender, EventArgs e)
        {
            var listItem = (ListItemZero)sender;

            var context = (TreeNodeContainer<object>)listItem.BindingContext;
            if (context != null)
                WidthRequest = _treeView.IndentMultiplier * (context.Indent - 1);
        }

        private TreeViewZero GetTreeViewForElement(Element parameter)
        {
            while (parameter != null)
                if (parameter is TreeViewZero treeView)
                    return treeView;
                else
                    parameter = parameter.Parent;

            return null;
        }
    }
}
