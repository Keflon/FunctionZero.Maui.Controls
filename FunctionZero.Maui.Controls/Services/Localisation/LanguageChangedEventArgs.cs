namespace FunctionZero.Maui.Services.Localisation
{
    public class LanguageChangedEventArgs : EventArgs
    {
        public LanguageChangedEventArgs(string languageId)
        {
            LanguageId = languageId;
        }

        public string LanguageId { get; }
    }
}