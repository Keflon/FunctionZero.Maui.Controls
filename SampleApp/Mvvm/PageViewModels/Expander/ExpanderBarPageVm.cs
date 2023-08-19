using FunctionZero.CommandZero;
using SampleApp.Mvvm.ViewModels;
using SampleApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SampleApp.Mvvm.PageViewModels.Expander
{
    public class ExpanderBarPageVm : BasePageVm
    {
        public ExpanderBarPageVm()
        {
            ThemeButtonCommand = new CommandBuilder().SetExecuteAsync(ThemeButtonCommandExecuteAsync).Build();
        }

        private async Task ThemeButtonCommandExecuteAsync(object arg)
        {
            if(Application.Current.UserAppTheme == AppTheme.Dark)
                Application.Current.UserAppTheme = AppTheme.Light; 
            else
                Application.Current.UserAppTheme = AppTheme.Dark;
        }

        public ICommand ThemeButtonCommand { get; }

        public SampleAppThemeService ThemeService { get; }

    }
}
