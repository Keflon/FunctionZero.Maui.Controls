using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Evaluator;

namespace FunctionZero.Maui.Services.Localisation
{
    public class ExpressionString
    {
        private ExpressionTree _expression;
        private readonly string _resultString;

        public ExpressionString(ExpressionTree expression, string resultString)
        {
            _expression = expression;
            _resultString = resultString;
        }

        public bool GetString(object backingStore, ref string result)
        {
            result = _resultString;

            try
            {
                if (backingStore is IBackingStore pocoBackingStore)
                    return (bool)_expression.Evaluate(pocoBackingStore).Pop().GetValue();
                else
                    return (bool)_expression.Evaluate(new PocoBackingStore(backingStore)).Pop().GetValue();
            }
            catch(Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }
    }
}