using FunctionZero.Maui.Controls;
using FunctionZero.Maui.MvvmZero;
using SampleApp.Mvvm.Pages;
using SampleApp.Mvvm.Pages.Tree;
using SampleApp.Mvvm.PageViewModels;
using SampleApp.Mvvm.PageViewModels.Tree;

namespace SampleApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMvvmZero(config =>
                {
                    config.MapVmToView<AppFlyoutPageVm, AppFlyoutPage>()

                    .MapVmToView<BasicTreePageVm, BasicTreePage>()
                    .MapVmToView<TemplateSelectorTreePageVm, TemplateSelectorTreePage>()
                    .MapVmToView<SelfEnumerableTreePageVm, SelfEnumerableTreePage>()

                    
                    ;

                }

                )

                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            //#if DEBUG
            //            builder.Logging.AddDebug();
            //#endif

            builder.Services
                //.AddSingleton<FlyoutPage>()
                // because https://github.com/dotnet/maui/issues/14572
                //.AddSingleton<TabbedPage>()
                //.AddSingleton<MultiPage<Page>, AdaptedTabbedPage>()
                //.AddSingleton<FlyoutPage, AdaptedFlyoutPage>()
                .AddSingleton<FlyoutPage>()
                .AddSingleton<AppFlyoutPageVm>()
                .AddSingleton<AppFlyoutPage>()


                .AddSingleton<BasicTreePageVm>()
                .AddSingleton<TemplateSelectorTreePageVm>()
                .AddSingleton<SelfEnumerableTreePageVm>()

                .AddSingleton<BasicTreePage>()
                .AddSingleton<TemplateSelectorTreePage>()
                .AddSingleton<SelfEnumerableTreePage>()












               //.AddSingleton<MainPage>()
               //.AddSingleton<MainPageVm>()

               ;

            return builder.Build();
        }
    }
}