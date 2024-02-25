using FunctionZero.Maui.Controls;
using FunctionZero.Maui.MvvmZero;
using Microsoft.Maui.Platform;
using SampleApp.Mvvm.PageViewModels;
using SampleApp.Translations;
using SampleApp.Widgets;
using System.Diagnostics;

namespace SampleApp
{
    public partial class App : Application
    {
        public App(IPageServiceZero pageService, LangService langService)
        {
            var temp = new WidgetContainer("", "", "");
            InitializeComponent();

            pageService.Init(this);
            langService.Init(this.Resources, "english");

            // Uses AdaptedFlyoutPage because https://github.com/dotnet/maui/issues/13496
            var flyoutPage = pageService.GetFlyoutPage<AppFlyoutPageVm>();
            MainPage = flyoutPage;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            window.Width = 1000;
            window.Height = 800;
            //window.SizeChanged += Window_SizeChanged;

            return window;
        }

        private void Window_SizeChanged(object? sender, EventArgs e)
        {
            // TODO: Implement this using platform handlers to do this properly and remove the flicker.
            var window = (Window)sender;
            window.Width = 1000;
            window.Height = 800;
        }
    }
}