using FunctionZero.TreeListItemsSourceZero;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Controls
{
    [ContentProperty("Children")]
    public class TreeItemDataTemplateSelector : TemplateProvider
    {
        public IList<TreeItemDataTemplate> Children { get; set; } = new List<TreeItemDataTemplate>();

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var itemData = ((TreeNodeContainer<object>)item).Data;

            foreach (var template in Children)
                if (template.TargetType.IsAssignableFrom(itemData.GetType()))
                    return template.OnSelectTemplateProvider(itemData).ItemTemplate;

            return null;
        }

        public override TreeItemDataTemplate OnSelectTemplateProvider(object item)
        {
            foreach (var template in Children)
                if (template.TargetType.IsAssignableFrom(item.GetType()))
                    return template.OnSelectTemplateProvider(item);

            return null;
        }

    }
}
