using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Controls
{
    public class MultiViewAnimation : BindableObject
    {
        #region FromProperty

        public static readonly BindableProperty FromProperty = BindableProperty.Create(nameof(From), typeof(double), typeof(MultiViewAnimation), 0.0, BindingMode.OneWay, null, FromChanged);

        public double From
        {
            get { return (double)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        private static void FromChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewAnimation)bindable;

        }

        #endregion

        #region ToViewNameProperty

        public static readonly BindableProperty ToProperty = BindableProperty.Create(nameof(To), typeof(double), typeof(MultiViewAnimation), 0.0, BindingMode.OneWay, null, ToChanged);

        public double To
        {
            get { return (double)GetValue(ToProperty); }
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

        public static readonly BindableProperty ExpressionProperty = BindableProperty.Create(nameof(Expression), typeof(string), typeof(MultiViewAnimation), string.Empty, BindingMode.OneWay, null, ExpressionChanged);

        public string Expression
        {
            get { return (string)GetValue(ExpressionProperty); }
            set { SetValue(ExpressionProperty, value); }
        }

        private static void ExpressionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewAnimation)bindable;
        }

        #endregion

        #region StartingExpressionProperty

        public static readonly BindableProperty StartingExpressionProperty = BindableProperty.Create(nameof(StartingExpression), typeof(string), typeof(MultiViewAnimation), string.Empty, BindingMode.OneWay, null, StartingExpressionChanged);

        public string StartingExpression
        {
            get { return (string)GetValue(StartingExpressionProperty); }
            set { SetValue(StartingExpressionProperty, value); }
        }

        private static void StartingExpressionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewAnimation)bindable;
        }

        #endregion


        #region FinishedExpressionProperty

        public static readonly BindableProperty FinishedExpressionProperty = BindableProperty.Create(nameof(FinishedExpression), typeof(string), typeof(MultiViewAnimation), string.Empty, BindingMode.OneWay, null, FinishedExpressionChanged);

        public string FinishedExpression
        {
            get { return (string)GetValue(FinishedExpressionProperty); }
            set { SetValue(FinishedExpressionProperty, value); }
        }

        private static void FinishedExpressionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewAnimation)bindable;
        }

        #endregion

    }
}
