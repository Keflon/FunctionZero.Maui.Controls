using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.Maui.MarkupExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Services.Localisation
{
    /// <summary>
    /// Goal. To be decoupled enough to allow downloading and selection of new or updated language packs on the fly.
    /// </summary>
    public abstract partial class BaseLanguageService<TEnum> : INotifyPropertyChanged where TEnum : Enum
    {
        private readonly string _resourceKey;
        private ResourceDictionary _resourceHost;
        private Dictionary<string, LanguageProvider> _languages;
        public event EventHandler<LanguageChangedEventArgs> LanguageChanged;

        // INPC raised by SetLanguage(..)
        public string CurrentLanguageId { get; protected set; }


        public event PropertyChangedEventHandler PropertyChanged;


        // TODO: Expose an ObsColl of languages, so the app can e.g. download new language packs and the UI can bind to it all.
        public BaseLanguageService(string resourceKey = "languageResource")
        {
            _resourceKey = resourceKey;
            _languages = new();
        }

        public void Init(ResourceDictionary resourceHost, string initialLanguage)
        {
            _resourceHost = resourceHost;
            SetLanguage(initialLanguage);
        }

        public void RegisterLanguage(string id, LanguageProvider language)
        {
            _languages[id] = language;
        }

        /// <summary>
        /// You probably want resourceHost to be 'Application.Current.Resources'
        /// </summary>
        /// <param name="resourceHost"></param>
        /// <param name="id"></param>
        /// <exception cref="Exception"></exception>
        public void SetLanguage(string id)
        {
            if (_resourceHost == null)
                throw new InvalidOperationException("You must call Init on the LanguageService before you call SetLanguage(string id), e.g. Init(Application.Current.Resources);");

            if (_languages.TryGetValue(id, out var languageService))
                _resourceHost[_resourceKey] = languageService.GetLookup();
            else
                throw new Exception($"Register a language before trying to set it. Language: '{id}' ");

            CurrentLanguageId = id;

            LanguageChanged?.Invoke(this, new LanguageChangedEventArgs(id));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLanguageId)));
        }

        //public string[] CurrentLookup => _resourceHost[_resourceKey] as string[];

        public string GetText(TEnum textId, IBackingStore host)
        {
            var lookup = _languages[CurrentLanguageId].GetLookup()[(int)(object)textId];

            foreach (var item in lookup)
            {
                var result = item.Item1.Evaluate(host).Pop();
                if (result.Type == ExpressionParserZero.Operands.OperandType.Bool)
                    if ((bool)result.GetValue() == true)
                        return item.Item2;
            }
            return $"textId {textId} not found.";
        }
    }
}
