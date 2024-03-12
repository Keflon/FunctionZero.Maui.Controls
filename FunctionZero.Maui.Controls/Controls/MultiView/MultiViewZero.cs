using FunctionZero.ExpressionParserZero;
using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Evaluator;
using FunctionZero.ExpressionParserZero.Operands;
using FunctionZero.ExpressionParserZero.Parser;
using FunctionZero.Maui.zBind.z;
using Microsoft.Maui.Layouts;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace FunctionZero.Maui.Controls
{
    public class MultiViewZero : Layout
    {
        private readonly PocoBackingStore _backingStore;

        // Every expression acts on 'this' and has a 'View' property that refers to its target.
        // View will use *magic* to always have the right value for any Animation that is running.
        // E.g. 'View.Opacity = value' will set the Opacity of the target View to the animation value during the animation.
        public IView View { get; private set; }

        // A *magic* property that is set to the easing-function value for each Animation.
        // E.g. 'View.Opacity = value' will set the Opacity of the target View to the animation value during the animation.
        public double value { get; protected set; }

        public MultiViewZero()
        {
            InAnimations = new List<MultiViewAnimation>();
            OutAnimations = new List<MultiViewAnimation>();
            _backingStore = new PocoBackingStore(this);
            this.Loaded += MultiViewZero_Loaded;
        }

        private void MultiViewZero_Loaded(object sender, EventArgs e)
        {
            // Once only ever.
            this.Loaded -= MultiViewZero_Loaded;

            foreach (IView item in this)
                TryEvaluate(CreatedExpression, _backingStore, item);
        }

        protected override ILayoutManager CreateLayoutManager()
        {
            return new MultiViewZeroLayoutManager(this);
        }

        #region InDurationProperty

        public static readonly BindableProperty InDurationProperty = BindableProperty.Create(nameof(InDuration), typeof(uint), typeof(MultiViewZero), (uint)250, BindingMode.OneWay, null, InDurationChanged);

        public uint InDuration
        {
            get { return (uint)GetValue(InDurationProperty); }
            set { SetValue(InDurationProperty, value); }
        }

        private static void InDurationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewZero)bindable;
        }

        #endregion

        #region OutDurationProperty

        public static readonly BindableProperty OutDurationProperty = BindableProperty.Create(nameof(OutDuration), typeof(uint), typeof(MultiViewZero), (uint)250, BindingMode.OneWay, null, OutDurationChanged);

        public uint OutDuration
        {
            get { return (uint)GetValue(OutDurationProperty); }
            set { SetValue(OutDurationProperty, value); }
        }

        private static void OutDurationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewZero)bindable;
        }

        #endregion

        #region EaseInProperty

        public static readonly BindableProperty EaseInProperty = BindableProperty.Create(nameof(EaseIn), typeof(Easing), typeof(MultiViewZero), Easing.Linear, BindingMode.OneWay, null, EaseInChanged);

        private static void EaseInChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewZero)bindable;
        }

        public Easing EaseIn
        {
            get => (Easing)GetValue(EaseInProperty);
            set => SetValue(EaseInProperty, value);
        }

        #endregion

        #region EaseOutProperty

        public static readonly BindableProperty EaseOutProperty = BindableProperty.Create(nameof(EaseOut), typeof(Easing), typeof(MultiViewZero), Easing.Linear, BindingMode.OneWay, null, EaseOutChanged);

        private static void EaseOutChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewZero)bindable;
        }

        public Easing EaseOut
        {
            get => (Easing)GetValue(EaseOutProperty);
            set => SetValue(EaseInProperty, value);
        }
        #endregion

        #region TopViewNameProperty

        public static readonly BindableProperty TopViewNameProperty = BindableProperty.Create(nameof(TopViewName), typeof(string), typeof(MultiViewZero), string.Empty, BindingMode.OneWay, null, TopViewNameChanged);

        public string TopViewName
        {
            get { return (string)GetValue(TopViewNameProperty); }
            set { SetValue(TopViewNameProperty, value); }
        }

        private static void TopViewNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewZero)bindable;

            // TODO: Find the current view.
            bool flag = false;
            foreach (IView item in self)
            {
                if (item is View theChildView)
                {
                    var itemName = GetMultiName(theChildView);
                    if (itemName != null)
                    {
                        if (itemName == self.TopViewName)
                        {
                            //theChildView.IsVisible = true;
                            self.SetTopView(theChildView);
                            flag = true;
                        }
                        //else if (item != self.PreviousView && item != self.CurrentView)
                        //    theChildView.IsVisible = false;
                    }
                }
            }
            if (flag == false)
                self.SetTopView(null);
        }
        View _topView;

        private void SetTopView(View theChildView)
        {
            if (_topView == theChildView)
                return;

            // If there is something to animate out ...
            if (_topView != null)
            {
                if (_topView.IsLoaded)
                {
                    // If there's an old animation it'll be on its way in. Kill it.
                    if (_topView.AnimationIsRunning("TheAnimation"))
                    {
                        _topView.AbortAnimation("TheAnimation");

                        foreach (var anim in InAnimations)
                            TryEvaluate(anim.FinishedExpression, _backingStore, _topView);
                    }

                    var a = new Animation();

                    var localTopView = _topView;

                    // Animate the old topview out ...
                    foreach (var anim in OutAnimations)
                    {
                        TryEvaluate(anim.StartingExpression, _backingStore, _topView);
                        a.Add(0, 1, new Animation(val => { value = val; View = localTopView; TryEvaluate(anim.Expression, _backingStore, localTopView); }, EvaluateDouble(anim.From, _backingStore, localTopView), EvaluateDouble(anim.To, _backingStore, localTopView), anim.EasingFunc, () => TryEvaluate(anim.FinishedExpression, _backingStore, localTopView)));
                    }
                    a.Commit(_topView, "TheAnimation", 16, InDuration, null, null, () => false);
                }
                else
                {
                    value = 1.0;
                    foreach (var anim in OutAnimations)
                        TryEvaluate(anim.Expression, _backingStore, _topView);
                }

            }

            _topView = theChildView;

            // If there is something to animate in ...
            if (_topView != null)
            {
                if (_topView.IsLoaded)
                {
                    // If there's an old animation it'll be on its way out. Kill it.
                    if (_topView.AnimationIsRunning("TheAnimation"))
                    {
                        _topView.AbortAnimation("TheAnimation");

                        foreach (var anim in OutAnimations)
                            TryEvaluate(anim.FinishedExpression, _backingStore, _topView);
                    }

                    var a = new Animation();

                    var localTopView = _topView;

                    // Animate the old topview out ...
                    foreach (var anim in InAnimations)
                    {
                        TryEvaluate(anim.StartingExpression, _backingStore, localTopView);
                        a.Add(0, 1, new Animation(val => { value = val; View = localTopView; TryEvaluate(anim.Expression, _backingStore, localTopView); }, EvaluateDouble(anim.From, _backingStore, localTopView), EvaluateDouble(anim.To, _backingStore, localTopView), anim.EasingFunc, () => TryEvaluate(anim.FinishedExpression, _backingStore, localTopView)));
                    }
                    a.Commit(_topView, "TheAnimation", 16, InDuration, null, null, () => false);
                }
                else
                {
                    value = 1.0;
                    foreach (var anim in InAnimations)
                        TryEvaluate(anim.Expression, _backingStore, _topView);
                }
            }

            // Visibilty has changed, so arrange it, in case it wasn't visible for the initial arrange call.
            (this as IView).InvalidateArrange();
        }

        private double EvaluateDouble(ExpressionTree expression, PocoBackingStore backingStore, IView view)
        {
            View = view;
            try
            {
                OperandStack result = expression.Evaluate(backingStore);
                IOperand operand = OperatorActions.PopAndResolve(result, backingStore);
                double retval = Convert.ToDouble(operand.GetValue());
                return retval;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;
            }
        }

        private OperandStack TryEvaluate(ExpressionTree compiledExpression, PocoBackingStore backingStore, IView view)
        {
            //View = isInAnimation ? _currentView : _previousView;
            View = view;

            try
            {
                OperandStack retval = compiledExpression.Evaluate(backingStore);
                return retval;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        #endregion

        #region InAnimationsProperty

        public static readonly BindableProperty InAnimationsProperty = BindableProperty.Create(nameof(InAnimations), typeof(IList<MultiViewAnimation>), typeof(MultiViewZero), null, BindingMode.OneWay, null, InAnimationsChanged);

        public IList<MultiViewAnimation> InAnimations
        {
            get { return (IList<MultiViewAnimation>)GetValue(InAnimationsProperty); }
            set { SetValue(InAnimationsProperty, value); }
        }

        private static void InAnimationsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewZero)bindable;
        }

        #endregion

        #region OutAnimationsProperty

        public static readonly BindableProperty OutAnimationsProperty = BindableProperty.Create(nameof(OutAnimations), typeof(IList<MultiViewAnimation>), typeof(MultiViewZero), null, BindingMode.OneWay, null, OutAnimationsChanged);

        public IList<MultiViewAnimation> OutAnimations
        {
            get { return (IList<MultiViewAnimation>)GetValue(OutAnimationsProperty); }
            set { SetValue(OutAnimationsProperty, value); }
        }

        private static void OutAnimationsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewZero)bindable;
        }

        #endregion

        #region CreatedExpressionProperty

        public static readonly BindableProperty CreatedExpressionProperty = BindableProperty.Create(nameof(CreatedExpression), typeof(ExpressionTree), typeof(MultiViewZero), _dudExpression, BindingMode.OneWay, null, CreatedExpressionChanged, null, null, MakeDud);

        [TypeConverter(typeof(ExpressionTreeTypeConverter))]
        public ExpressionTree CreatedExpression
        {
            get { return (ExpressionTree)GetValue(CreatedExpressionProperty); }
            set { SetValue(CreatedExpressionProperty, value); }
        }

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

        private static void CreatedExpressionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (MultiViewZero)bindable;
        }

        #endregion

        #region AttachedProperties

        public static readonly BindableProperty MultiNameProperty =
    BindableProperty.CreateAttached("MultiName", typeof(string), typeof(MultiViewZero), "", BindingMode.OneWay, null, MultiNamePropertyChanged);

        private static void MultiNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // This is the name assigned to each child.
            Debug.WriteLine($"Object: {bindable}, oldValue: {oldValue}, newValue: {newValue}");
        }

        public static string GetMultiName(BindableObject view)
        {
            return (string)view.GetValue(MultiNameProperty);
        }

        public static void SetMultiName(BindableObject view, string value)
        {
            view.SetValue(MultiNameProperty, value);
        }
        #endregion
    }
}
