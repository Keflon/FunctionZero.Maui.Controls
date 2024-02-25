using FunctionZero.ExpressionParserZero.Binding;
using FunctionZero.ExpressionParserZero.Evaluator;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.Maui.Controls;
using FunctionZero.Maui.MvvmZero;
using FunctionZero.Maui.Services;
using SampleApp.Mvvm.Pages;
using SampleApp.Mvvm.Pages.Expander;
using SampleApp.Mvvm.Pages.List;
using SampleApp.Mvvm.Pages.Mask;
using SampleApp.Mvvm.Pages.MultiView;
using SampleApp.Mvvm.Pages.Translations;
using SampleApp.Mvvm.Pages.Tree;
using SampleApp.Mvvm.PageViewModels;
using SampleApp.Mvvm.PageViewModels.Expander;
using SampleApp.Mvvm.PageViewModels.List;
using SampleApp.Mvvm.PageViewModels.Mask;
using SampleApp.Mvvm.PageViewModels.MultiView;
using SampleApp.Mvvm.PageViewModels.Translations;
using SampleApp.Mvvm.PageViewModels.Tree;
using SampleApp.Translations;

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
                        .MapVmToView<MultiViewModalPageVm, MultiViewModalPage>()
                        .MapVmToView<TranslationHomePageVm, TranslationHomePage>()

                        
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

                .AddSingleton<MultiViewModalPage>()
                .AddSingleton<MultiViewModalPageVm>()

                .AddSingleton<TranslationHomePage>()
                .AddSingleton<TranslationHomePageVm>()

                .AddSingleton<LangService>(GetConfiguredLanguageService)

               ;

            return builder.Build();
        }

        #region Language translation setup
        private static LangService GetConfiguredLanguageService(IServiceProvider provider)
        {
            var translationService = new LangService();
            translationService.RegisterLanguage("english", new LanguageProvider(GetEnglish, "English"));
            translationService.RegisterLanguage("german", new LanguageProvider(GetGerman, "Deutsch"));

            return translationService;
        }

        // Example
        private static string[] GetEnglish() => new string[] { "Hello", "World", "Welcome to the Moasure Playground!" };
        private static string[] GetGerman() => new string[] { "Hallo", "Welt", "Willkommen auf dem Moasure Spielplatz!" };

        //private static List<(ExpressionTree, string)> GetEnglish2()
        //{
        //    var retval = new List<(ExpressionTree, string)>();

        //    var parser = ExpressionParserFactory.GetExpressionParser();

        //    retval.Add((parser.Parse("NumBananas == 0"), "There are no bananas!"));

        //    retval.Add((parser.Parse("NumBananas == 1"), "There is one banana!"));
        //    retval.Add((parser.Parse("NumBananas == 2"), "There are two bananas!"));
        //    retval.Add((parser.Parse("NumBananas < 10"), "There are {NumBananas} bananas!"));       // TODO: {NumBananas}
        //    retval.Add((parser.Parse("true"), "There are loads of bananas!"));

        //    return retval;
        //}

        #endregion
    }
}