using FunctionZero.TreeListItemsSourceZero;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Controls
{
    public class TreeNodeSpacer : TemplatedView
    {
        private TreeViewZero _treeView;

        public TreeNodeSpacer()
        {
            TemplateBinding f = new TemplateBinding();

            //BackgroundColor = Colors.Purple;

            //Margin = new Thickness(4);

            SetBinding(BindingContextProperty, f);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _treeView = GetTreeViewForElement((Element)this);

            var listItem = (ListItemZero)BindingContext;
            listItem.BindingContextChanged += ListItem_BindingContextChanged;
        }

        private void ListItem_BindingContextChanged(object sender, EventArgs e)
        {
            var listItem = (ListItemZero)sender;

            var context = (TreeNodeContainer<object>)listItem.BindingContext;
            if (context != null)
                WidthRequest = _treeView.IndentMultiplier * (context.Indent - 1);
        }



        //public static readonly BindableProperty WidthProperty = BindableProperty.Create(nameof(Width), typeof(double), typeof(TreeNodeSpacer), (double)0, BindingMode.OneWayToSource);

        //public double Width
        //{
        //    get { return (double)GetValue(WidthProperty); }
        //    set { SetValue(WidthProperty, value); }
        //}






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
