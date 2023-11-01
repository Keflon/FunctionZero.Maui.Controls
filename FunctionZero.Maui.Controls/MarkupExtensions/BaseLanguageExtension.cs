using System.ComponentModel;

namespace FunctionZero.Maui.MarkupExtensions
{
    public abstract class BaseLanguageExtension<TEnum> : IMarkupExtension<Binding>, INotifyPropertyChanged where TEnum : Enum
    {
        public BaseLanguageExtension()
        {
        }
        public TEnum TextId { get; set; }
        public bool ShowOff { get; set; }
        private string _text;


        public string Text
        {
            get => _text;
            set
            {
                if (value != _text)
                {
                    _text = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
                }
            }
        }

        #region INPC

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public Binding ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            var target = provideValueTarget.TargetObject as Element;
            var property = provideValueTarget.TargetProperty as BindableProperty;

            SetLangHost(target, this);
            target.SetDynamicResource(LookupProperty, "LocalisedStrings");

            var b = new Binding("Text", mode: BindingMode.OneWay, source: this);

            return b;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return (this as IMarkupExtension<Binding>).ProvideValue(serviceProvider);
        }

        public static readonly BindableProperty LangHostProperty =
    BindableProperty.CreateAttached("LangHost", typeof(BaseLanguageExtension<TEnum>), typeof(Element), null);


        public static readonly BindableProperty LookupProperty =
            BindableProperty.CreateAttached("Lookup", typeof(string[]), typeof(Element), new string[] { "Hello", "World" }, BindingMode.OneWay, null, LookupPropertyChanged);

        private static void LookupPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var langHost = GetLangHost(bindable);

            string[] lookup = GetLookup(bindable);
            if (lookup != null)
            {
                string to = "???";
                if (lookup.Length > (int)(object)langHost.TextId)
                {
                    to = lookup[(int)(object)langHost.TextId];
                }
                if (langHost.ShowOff)
                    CrossFadeText(langHost, to);
                else
                    langHost.Text = GetLookup(bindable)[(int)(object)langHost.TextId];

            }
        }

        private static async void CrossFadeText(BaseLanguageExtension<TEnum> langHost, string to)
        {
            string from = langHost.Text ?? string.Empty;

            int countToRemove = from.Length;
            int countToAdd = to.Length;

            int total = Math.Max(countToRemove, countToAdd);
            for (int c = 0; c < total; c++)
            {
                if (countToRemove-- > 0)
                    langHost.Text = langHost.Text.Remove(0, 1);

                if (countToAdd-- > 0)
                    langHost.Text += to[c];

                await Task.Delay(16);
            }
        }

        public static string[] GetLookup(BindableObject view)
        {
            var retval = (string[])view.GetValue(LookupProperty);
            return retval;
        }

        public static void SetLookup(BindableObject view, bool value)
        {
            view.SetValue(LookupProperty, value);
        }
        public static BaseLanguageExtension<TEnum> GetLangHost(BindableObject view)
        {
            return (BaseLanguageExtension<TEnum>)view.GetValue(LangHostProperty);
        }

        public static void SetLangHost(BindableObject view, BaseLanguageExtension<TEnum> value)
        {
            view.SetValue(LangHostProperty, value);
        }
    }
}
