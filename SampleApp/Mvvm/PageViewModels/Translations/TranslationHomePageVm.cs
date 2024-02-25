using FunctionZero.CommandZero;
using FunctionZero.Maui.MvvmZero;
using FunctionZero.Maui.Services;
using SampleApp.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SampleApp.Mvvm.PageViewModels.Translations
{
    public class TranslationHomePageVm : BasePageVm
    {
        private string _selectedLanguage;

        public IList<KeyValuePair<string, string>> AvailableLanguages { get; }

        public string SelectedLanguage { get => _selectedLanguage; set => SetProperty(ref _selectedLanguage, value); }

        public ICommand SetLanguageCommand { get; }
        public TranslationHomePageVm(TranslationService translationService)
        {
            AvailableLanguages = [new KeyValuePair<string, string>("English", "english"), new KeyValuePair<string, string>("Deutsch", "german")];

            SetLanguageCommand = new CommandBuilder()
                .SetText()=>translationService.
        }
    }
}
