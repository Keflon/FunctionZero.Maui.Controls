using FunctionZero.CommandZero;
using FunctionZero.Maui.MvvmZero;
using FunctionZero.Maui.Services;
using SampleApp.Mvvm.ViewModels;
using SampleApp.Translations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SampleApp.Mvvm.PageViewModels.Translations
{
    public class TranslationHomePageVm : BasePageVm
    {
        private readonly LangService _langService;

        public ICommand SetLanguageCommand { get; }
        public ICommand DoTheThingCommand { get; }
        public TranslationHomePageVm(LangService langService)
        {
            _langService = langService;

            SetLanguageCommand = new CommandBuilder()
                .AddGuard(this)
                .SetExecute(SetLanguageCommandExecute)
                .Build();

            DoTheThingCommand = new CommandBuilder()
                .AddGuard(this)
                .SetExecute(DoTheThingCommandExecute)
                .SetName(() => langService.GetText(LangStrings.E_World))
                .AddObservedProperty(langService, nameof(LangService.CurrentLanguageId))
                .Build();
        }

        private void DoTheThingCommandExecute()
        {
            Debug.WriteLine($"New language: {_langService.CurrentLanguageId}");
        }

        private void SetLanguageCommandExecute(object arg)
        {
            _langService.SetLanguage((string)arg);
        }
    }
}
