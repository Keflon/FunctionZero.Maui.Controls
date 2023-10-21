using FunctionZero.Maui.Controls;
using FunctionZero.Maui.MvvmZero;
using SampleApp.Mvvm.Pages;
using SampleApp.Mvvm.Pages.Expander;
using SampleApp.Mvvm.Pages.List;
using SampleApp.Mvvm.Pages.Mask;
using SampleApp.Mvvm.Pages.Tree;
using SampleApp.Mvvm.PageViewModels;
using SampleApp.Mvvm.PageViewModels.Expander;
using SampleApp.Mvvm.PageViewModels.List;
using SampleApp.Mvvm.PageViewModels.Mask;
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
                        // Root controller
                        config.MapVmToView<AppFlyoutPageVm, AppFlyoutPage>()

                        // 'Pages' that can be shown.
                        .MapVmToView<LazyListPageVm, LazyListPage>()
                        .MapVmToView<TurbulentListPageVm, TurbulentListPage>()
                        .MapVmToView<WobblyListPageVm, WobblyListPage>()
                        .MapVmToView<BasicTreePageVm, BasicTreePage>()
                        .MapVmToView<TemplateSelectorTreePageVm, TemplateSelectorTreePage>()
                        .MapVmToView<SelfEnumerableTreePageVm, SelfEnumerableTreePage>()
                        .MapVmToView<TurbulentTreePageVm, TurbulentTreePage>()
                        .MapVmToView<CircleMaskPageVm, CircleMaskPage>()
                        .MapVmToView<JayBirthdayPageVm, JayBirthdayPage>()
                        .MapVmToView<ExpanderBarPageVm, ExpanderBarPage>()
                        .MapVmToView<ExpanderTestPageVm, ExpanderTestPage>()
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
                .AddSingleton<CircleMaskPageVm>()
                .AddSingleton<JayBirthdayPageVm>()
                .AddSingleton<ExpanderBarPageVm>()

                .AddSingleton<LazyListPage>()
                .AddSingleton<TurbulentListPage>()
                .AddSingleton<WobblyListPage>()

                .AddSingleton<BasicTreePage>()
                .AddSingleton<TemplateSelectorTreePage>()
                .AddSingleton<SelfEnumerableTreePage>()
                .AddSingleton<TurbulentTreePage>()
                .AddSingleton<CircleMaskPage>()
                .AddSingleton<JayBirthdayPage>()
                .AddSingleton<ExpanderBarPage>()

                .AddSingleton<ExpanderTestPage>()
                .AddSingleton<ExpanderTestPageVm>()



               ;

            return builder.Build();
        }
    }
}