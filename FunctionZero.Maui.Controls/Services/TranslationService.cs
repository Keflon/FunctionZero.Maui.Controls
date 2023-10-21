using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Services
{
    /// <summary>
    /// Goal. To be decoupled enough to allow downloading and selection of new or updated language packs on the fly.
    /// </summary>
    public class TranslationService
    {
        private Dictionary<string, LanguageProvider> _languages;
        // TODO: Expose an ObsColl of languages, so the app can e.g. download new language packs and the UI can bind to it all.
        public TranslationService()
        {
            _languages = new();
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
        public void SetLanguage(ResourceDictionary resourceHost, string id)
        {
            if (_languages.TryGetValue(id, out var languageService))
                resourceHost["LocalisedStrings"] = languageService.GetLookup();
            else
                throw new Exception($"Register a language before trying to set it. Language: '{id}' ");
        }
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
