namespace FunctionZero.Maui.Services
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