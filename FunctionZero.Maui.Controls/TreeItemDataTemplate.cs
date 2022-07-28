namespace FunctionZero.Maui.Controls
{
    [ContentProperty("ItemTemplate")]
    public abstract class TreeItemDataTemplate : TemplateProvider
    {
        public string ChildrenPropertyName { get; set; }
        public string IsExpandedPropertyName { get; set; }
        public DataTemplate ItemTemplate { get; set; }

        public Type TargetType { get; set; }

        public override TreeItemDataTemplate OnSelectTemplate(object item)
        {
            return this;
        }
    }
}