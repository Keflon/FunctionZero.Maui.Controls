//using Microsoft.Maui.Controls;

//namespace FunctionZero.Maui.Controls
//{
//    public class ContentControl : ContentView
//    {
//        protected override void OnBindingContextChanged()
//        {
//            base.OnBindingContextChanged();

//            UpdateTemplate();

//            // Necessary for Android. Stops truncation of text in itemtemplate for the node viewmodel
//            //if (BindingContext != null)
//            //    this.Dispatcher.Dispatch(() =>
//            //    InvalidateLayout() 
//            //    );
//        }

//        public static readonly BindableProperty ContentTemplateProviderProperty = 
//            BindableProperty.Create(
//                nameof(ContentTemplateProvider), 
//                typeof(TemplateProvider), 
//                typeof(ContentControl), 
//                null, 
//                BindingMode.OneWay, 
//                null, 
//                TemplateProviderChanged
//            );

//        public TemplateProvider ContentTemplateProvider
//        {
//            get { return (TemplateProvider)GetValue(ContentTemplateProviderProperty); }
//            set { SetValue(ContentTemplateProviderProperty, value); }
//        }

//        private static void TemplateProviderChanged(BindableObject bindable, object oldvalue, object newvalue)
//        {
//            var cp = (ContentControl)bindable;
//            cp.UpdateTemplate();
//        }

//        private void UpdateTemplate()
//        {
//            if (BindingContext != null)
//            {
//                if (ContentTemplateProvider != null)
//                {
//                    var template = ContentTemplateProvider.OnSelectTemplate(BindingContext);
//                    Content = (View)template.ItemTemplate.CreateContent();
//                }
//            }
//        }
//    }
//}
