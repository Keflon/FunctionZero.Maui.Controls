using FunctionZero.Maui.Controls;
using FunctionZero.Maui.MvvmZero;
using Microsoft.Maui.Platform;
using SampleApp.Mvvm.PageViewModels;
using System.Diagnostics;

namespace SampleApp
{
    public partial class App : Application
    {
        public App(IPageServiceZero pageService)
        {
            InitializeComponent();

            pageService.Init(this);

            // Uses AdaptedFlyoutPage because https://github.com/dotnet/maui/issues/13496
            var flyoutPage = pageService.GetFlyoutPage<AppFlyoutPageVm>();
            MainPage = flyoutPage;
        }
    }
}