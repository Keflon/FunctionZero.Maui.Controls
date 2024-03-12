using FunctionZero.ExpressionParserZero.Evaluator;
using FunctionZero.ExpressionParserZero.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Controls
{
    public class MultiViewAnimation : BindableObject
    {
        private static ExpressionTree _dudExpression;
        private static object MakeDud(BindableObject bindable)
        {
            if (_dudExpression == null)
            {
                var ep = ExpressionParserZero.Binding.ExpressionParserFactory.GetExpressionParser();
                _dudExpression = ep.Parse("");
            }
            return _dudExpression;
        }

        #region FromProperty

        public static readonly BindableProperty FromProperty = BindableProperty.Create(nameof(From), typeof(ExpressionTree), typeof(MultiViewAnimation), _dudExpression, BindingMode.OneWay, null, FromChanged, null, null, MakeDud);

        [TypeConverter(typeof(ExpressionTreeTypeConverter))]
        public ExpressionTree From
        {
            get { return (ExpressionTree)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        private static void FromChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewAnimation)bindable;

        }

        #endregion

        #region ToViewNameProperty

        public static readonly BindableProperty ToProperty = BindableProperty.Create(nameof(To), typeof(ExpressionTree), typeof(MultiViewAnimation), _dudExpression, BindingMode.OneWay, null, ToChanged, null, null, MakeDud);

        [TypeConverter(typeof(ExpressionTreeTypeConverter))]
        public ExpressionTree To
        {
            get { return (ExpressionTree)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        private static void ToChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewAnimation)bindable;

        }

        #endregion

        #region EasingFuncProperty

        public static readonly BindableProperty EasingFuncProperty = BindableProperty.Create(nameof(EasingFunc), typeof(Easing), typeof(MultiViewAnimation), Easing.Linear, BindingMode.OneWay, null, EasingFuncChanged);

        public Easing EasingFunc
        {
            get { return (Easing)GetValue(EasingFuncProperty); }
            set { SetValue(EasingFuncProperty, value); }
        }

        private static void EasingFuncChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewAnimation)bindable;

        }

        #endregion

        #region ExpressionProperty

        public static readonly BindableProperty ExpressionProperty = BindableProperty.Create(nameof(Expression), typeof(ExpressionTree), typeof(MultiViewAnimation), null, BindingMode.OneWay, null, ExpressionChanged,null,null, MakeDud);

        [TypeConverter(typeof(ExpressionTreeTypeConverter))]
        public ExpressionTree Expression
        {
            get { return (ExpressionTree)GetValue(ExpressionProperty); }
            set { SetValue(ExpressionProperty, value); }
        }

        private static void ExpressionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewAnimation)bindable;
        }

        #endregion

        #region StartingExpressionProperty

        public static readonly BindableProperty StartingExpressionProperty = BindableProperty.Create(nameof(StartingExpression), typeof(ExpressionTree), typeof(MultiViewAnimation), _dudExpression, BindingMode.OneWay, null, StartingExpressionChanged, null, null, MakeDud);

        [TypeConverter(typeof(ExpressionTreeTypeConverter))]
        public ExpressionTree StartingExpression
        {
            get { return (ExpressionTree)GetValue(StartingExpressionProperty); }
            set { SetValue(StartingExpressionProperty, value); }
        }

        private static void StartingExpressionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewAnimation)bindable;
        }

        #endregion


        #region FinishedExpressionProperty

        public static readonly BindableProperty FinishedExpressionProperty = BindableProperty.Create(nameof(FinishedExpression), typeof(ExpressionTree), typeof(MultiViewAnimation), _dudExpression, BindingMode.OneWay, null, FinishedExpressionChanged, null, null, MakeDud);

        [TypeConverter(typeof(ExpressionTreeTypeConverter))]
        public ExpressionTree FinishedExpression
        {
            get { return (ExpressionTree)GetValue(FinishedExpressionProperty); }
            set { SetValue(FinishedExpressionProperty, value); }
        }

        private static void FinishedExpressionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewAnimation)bindable;
        }

        #endregion

    }
}
