namespace FunctionZero.Maui.Services
{
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
