namespace FunctionZero.Maui.Controls
{
    public abstract class TemplateProvider : DataTemplateSelector
    {
        public abstract TreeItemDataTemplate OnSelectTemplateProvider(object item);
    }
}