using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Services
{
    /// <summary>
    /// Goal. To be decoupled enough to allow downloading and selection of new or updated language packs on the fly.
    /// </summary>
    public class TranslationService : INotifyPropertyChanged
    {
        private readonly string _resourceKey;
        private ResourceDictionary _resourceHost;
        private Dictionary<string, LanguageProvider> _languages;
        public event EventHandler<LanguageChangedEventArgs> LanguageChanged;

        // INPC raised by SetLanguage(..)
        public string CurrentLanguageId => _resourceHost[_resourceKey] as string;


        public event PropertyChangedEventHandler PropertyChanged;


        // TODO: Expose an ObsColl of languages, so the app can e.g. download new language packs and the UI can bind to it all.
        public TranslationService(string resourceKey = "LocalisedStrings")
        {
            _resourceKey = resourceKey;
            _languages = new();
        }

        public void Init(ResourceDictionary resourceHost)
        {
            _resourceHost = resourceHost;
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
                throw new InvalidOperationException("You must call Init on the TranslationService before you call SetLanguage(string id), e.g. Init(Application.Current.Resources);");

            if (_languages.TryGetValue(id, out var languageService))
                _resourceHost[_resourceKey] = languageService.GetLookup();
            else
                throw new Exception($"Register a language before trying to set it. Language: '{id}' ");

            LanguageChanged?.Invoke(this, new LanguageChangedEventArgs(id));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLanguageId)));
        }

        public string[] CurrentLookup => _resourceHost[_resourceKey] as string[];
        public class LanguageProvider
        {
            public LanguageProvider(Func<string[]> getLookup, string languageName)
            {
                GetLookup = getLookup;
                LanguageName = languageName;
            }
            public Func<string[]> GetLookup { get; }
            public string LanguageName { get; }
        }
    }
}
