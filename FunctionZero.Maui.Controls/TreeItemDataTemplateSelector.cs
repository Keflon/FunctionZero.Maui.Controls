using FunctionZero.TreeListItemsSourceZero;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Controls
{
    [ContentProperty("DataTemplateContent")]
    public class TreeItemDataTemplateSelector : TemplateProvider
    {
        public IList<TreeItemDataTemplate> DataTemplateContent { get; set; } = new List<TreeItemDataTemplate>();

        // Used by the underlying ListViewZero.
        //protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        //{
        //    var itemData = ((TreeNodeContainer<object>)item).Data;

        //    foreach (TreeItemDataTemplate template in Children)
        //        if (template.TargetType.IsAssignableFrom(itemData.GetType()))
        //            return template.OnSelectTemplateProvider(itemData).ItemTemplate;

        //    return null;
        //}

        //protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        //{
        //    var itemData = ((TreeNodeContainer<object>)item).Data;

        //    return OnSelectTemplateProvider(itemData).ItemTemplate;

        //    foreach (TreeItemDataTemplate template in Children)
        //        if (template.TargetType.IsAssignableFrom(itemData.GetType()))
        //            return template.OnSelectTemplateProvider(itemData).ItemTemplate;

        //    return null;
        //}

        // Used by the TreeViewZero.
        public override TreeItemDataTemplate OnSelectTemplateProvider(object item)
        {
            foreach (TreeItemDataTemplate template in DataTemplateContent)
                if (template.TargetType.IsAssignableFrom(item.GetType()))
                    return template.OnSelectTemplateProvider(item);

            return null;
        }

    }
}
