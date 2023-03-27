namespace FunctionZero.Maui.Controls
{
    [ContentProperty("DataTemplateContent")]
    public class TreeItemDataTemplate : TemplateProvider
    {
        public string ChildrenPropertyName { get; set; }
        public string IsExpandedPropertyName { get; set; }
        public DataTemplate DataTemplateContent { get; set; }

        public Type TargetType { get; set; }

        public override TreeItemDataTemplate OnSelectTemplateProvider(object item)
        {
            return this;
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return DataTemplateContent;
        }
    }
}