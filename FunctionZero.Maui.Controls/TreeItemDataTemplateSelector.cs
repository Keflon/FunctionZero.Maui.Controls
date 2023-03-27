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
