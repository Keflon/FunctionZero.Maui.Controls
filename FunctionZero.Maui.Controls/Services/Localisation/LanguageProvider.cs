using FunctionZero.ExpressionParserZero.Binding;
using FunctionZero.ExpressionParserZero.Evaluator;

namespace FunctionZero.Maui.Services.Localisation
{
    public class LanguageProvider
    {
        private readonly ExpressionTree _trueExpression;

        public LanguageProvider(Func<List<List<(ExpressionTree, string)>>> getLookup, string languageName)
        {
            GetLookup = getLookup;
            LanguageName = languageName;
        }
        //item.Add((parser.Parse("true"), "There are loads of bananas!"));
        public LanguageProvider(Func<IEnumerable<string>> getLookup, string languageName)
        {
            var parser = ExpressionParserFactory.GetExpressionParser();
            _trueExpression = parser.Parse("true");

            GetLookup = GetLookupFromStringList(getLookup);
            LanguageName = languageName;
        }

        private Func<List<List<(ExpressionTree, string)>>> GetLookupFromStringList(Func<IEnumerable<string>> getLookup)
        {
            List<List<(ExpressionTree, string)>> lookup = new List<List<(ExpressionTree, string)>>();
            foreach (var theString in getLookup())
            {
                var item = new List<(ExpressionTree, string)>();
                item.Add((_trueExpression, theString));
                lookup.Add(item);
            }
            return () => lookup;
        }


        //public Func<string[]> GetLookup { get; }
        public Func<List<List<(ExpressionTree, string)>>> GetLookup { get; }
        public string LanguageName { get; }
    }
}
