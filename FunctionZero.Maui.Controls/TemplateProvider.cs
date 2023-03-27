using FunctionZero.TreeListItemsSourceZero;

namespace FunctionZero.Maui.Controls
{
    public abstract class TemplateProvider : DataTemplateSelector
    {
        public abstract TreeItemDataTemplate OnSelectTemplateProvider(object item);

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var itemData = ((TreeNodeContainer<object>)item).Data;
            return OnSelectTemplateProvider(itemData).DataTemplateContent;
        }
    }
}