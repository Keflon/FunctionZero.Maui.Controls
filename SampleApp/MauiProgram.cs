using FunctionZero.ExpressionParserZero.Binding;
using FunctionZero.ExpressionParserZero.Evaluator;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.Maui.Controls;
using FunctionZero.Maui.MvvmZero;
using FunctionZero.Maui.Services;
using FunctionZero.Maui.Services.Localisation;
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
        //private static string[] GetEnglish() => new string[] { "Hello", "World", "Welcome to the Moasure Playground!" };
        private static string[] GetGerman() => new string[] { "Bananas", "Hallo", "Welt", "Willkommen auf dem Moasure Spielplatz!" };

        private static List<List<(ExpressionTree, string)>> GetEnglish()
        {
            var parser = ExpressionParserFactory.GetExpressionParser();
            var trueExpression = parser.Parse("true");

            var retval = new List<List<(ExpressionTree, string)>>();

            {
                var item = new List<(ExpressionTree, string)>();

                item.Add((parser.Parse("NumBananas == 0"), "There are no bananas!"));
                item.Add((parser.Parse("NumBananas == 1"), "There is one banana!"));
                item.Add((parser.Parse("NumBananas == 2"), "There are two bananas!"));
                item.Add((parser.Parse("NumBananas < 10"), "There are {NumBananas} bananas!"));       // TODO: {NumBananas}
                item.Add((parser.Parse("true"), "There are loads of bananas!"));

                //HashSet<string> dependsOn = new HashSet<string>();

                //foreach(var tuple in item)
                //    foreach(var token in tuple.Item1.RpnTokens)
                //        if(token is Operand op)
                //            if(op.Type == OperandType.Variable)
                //                dependsOn.Add((string)op.GetValue());

                retval.Add(item);
            }
            {
                var item = new List<(ExpressionTree, string)>();

                item.Add((trueExpression, "Hello!"));

                retval.Add(item);
            }
            {
                var item = new List<(ExpressionTree, string)>();

                item.Add((trueExpression, "World"));

                retval.Add(item);
            }
            {
                var item = new List<(ExpressionTree, string)>();

                item.Add((trueExpression, "This is a demonstration of FunctionZero Translation Service"));

                retval.Add(item);
            }


            return retval;
        }

        //private static List<List<(ExpressionTree, string)>> GetGerman()
        //{
        //    var parser = ExpressionParserFactory.GetExpressionParser();
        //    var trueExpression = parser.Parse("true");

        //    var retval = new List<List<(ExpressionTree, string)>>();

        //    {
        //        var item = new List<(ExpressionTree, string)>();

        //        item.Add((parser.Parse("NumBananas == 0"), "Es gibt keine Bananen!"));
        //        item.Add((parser.Parse("NumBananas == 1"), "Es gibt eine Banane!"));
        //        item.Add((parser.Parse("NumBananas == 2"), "Es gibt zwei Bananen!"));
        //        item.Add((parser.Parse("NumBananas < 10"), "Es gibt {NumBananas} Bananen!"));       // TODO: {NumBananas}
        //        item.Add((parser.Parse("true"), "Es gibt jede Menge Bananen!"));

        //        retval.Add(item);
        //    }
        //    {
        //        var item = new List<(ExpressionTree, string)>();

        //        item.Add((trueExpression, "Hallo!"));

        //        retval.Add(item);
        //    }
        //    {
        //        var item = new List<(ExpressionTree, string)>();

        //        item.Add((trueExpression, "Welt"));

        //        retval.Add(item);
        //    }
        //    {
        //        var item = new List<(ExpressionTree, string)>();

        //        item.Add((trueExpression, "Dies ist eine Demonstration des FunctionZero Translation Service"));

        //        retval.Add(item);
        //    }


        //    return retval;
        //}

        #endregion
    }
}