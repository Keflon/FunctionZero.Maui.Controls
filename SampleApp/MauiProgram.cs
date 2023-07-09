using FunctionZero.Maui.Controls;
using FunctionZero.Maui.MvvmZero;
using SampleApp.Mvvm.Pages;
using SampleApp.Mvvm.Pages.List;
using SampleApp.Mvvm.Pages.Tree;
using SampleApp.Mvvm.PageViewModels;
using SampleApp.Mvvm.PageViewModels.List;
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



                    .MapVmToView<LazyListPageVm, LazyListPage>()
                    .MapVmToView<TurbulentListPageVm, TurbulentListPage>()
                    .MapVmToView<WobblyListPageVm, WobblyListPage>()
                    .MapVmToView<BasicTreePageVm, BasicTreePage>()
                    .MapVmToView<TemplateSelectorTreePageVm, TemplateSelectorTreePage>()
                    .MapVmToView<SelfEnumerableTreePageVm, SelfEnumerableTreePage>()
                    .MapVmToView<TurbulentTreePageVm, TurbulentTreePage>()

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


                .AddSingleton<LazyListPageVm>()
                .AddSingleton<TurbulentListPageVm>()
                .AddSingleton<WobblyListPageVm>()

                .AddSingleton<BasicTreePageVm>()
                .AddSingleton<TemplateSelectorTreePageVm>()
                .AddSingleton<SelfEnumerableTreePageVm>()
                .AddSingleton<TurbulentTreePageVm>()


                .AddSingleton<LazyListPage>()
                .AddSingleton<TurbulentListPage>()
                .AddSingleton<WobblyListPage>()

                .AddSingleton<BasicTreePage>()
                .AddSingleton<TemplateSelectorTreePage>()
                .AddSingleton<SelfEnumerableTreePage>()
                .AddSingleton<TurbulentTreePage>()












               //.AddSingleton<MainPage>()
               //.AddSingleton<MainPageVm>()

               ;

            return builder.Build();
        }
    }
}