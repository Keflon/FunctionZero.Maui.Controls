using FunctionZero.ExpressionParserZero.BackingStore;
using FunctionZero.ExpressionParserZero.Evaluator;
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
        private IView _currentView;
        private IView _previousView;
        public IView View { get; private set; }

        public MultiViewZero()
        {
            InAnimations = new List<MultiViewAnimation>();
            OutAnimations = new List<MultiViewAnimation>();
            _backingStore = new PocoBackingStore(this);
            this.Loaded += MultiViewZero_Loaded;
        }

        private void MultiViewZero_Loaded(object sender, EventArgs e)
        {
            var oldCurrentView = _currentView;

            if (oldCurrentView != null)
            {
                Debug.WriteLine("Got you!");
            }
            foreach (IView item in this)
            {
                // Temporarily set _currentView so expressions can reference 'View' e.g. View.IsVisible=...
                _currentView = item;
                TryEvaluate(CreatedExpression, _backingStore, true);
            }
            _currentView = oldCurrentView;
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

        private void SetTopView(View theChildView)
        {
            if (_previousView is View outgoingPreviousViewAsView)
            {
                if (outgoingPreviousViewAsView.AnimationIsRunning("PreviousAnimation"))
                {
                    outgoingPreviousViewAsView.AbortAnimation("PreviousAnimation");

                    foreach (var anim in OutAnimations)
                        TryEvaluate(anim.FinishedExpression, _backingStore, false);
                }
            }
            if (_currentView is View outgoingCurrentViewAsView)
            {
                if (outgoingCurrentViewAsView.AnimationIsRunning("CurrentAnimation"))
                {
                    outgoingCurrentViewAsView.AbortAnimation("CurrentAnimation");

                    foreach (var anim in InAnimations)
                        TryEvaluate(anim.FinishedExpression, _backingStore, true);
                }
            }

            _previousView = _currentView;
            _currentView = theChildView;

            if (_previousView is View previousViewAsView)
            {
                // Workaround for https://github.com/dotnet/maui/issues/18433
                if (previousViewAsView.IsLoaded)
                {
                    var a = new Animation();

                    foreach (MultiViewAnimation anim in OutAnimations)
                    {
                        TryEvaluate(anim.StartingExpression, _backingStore, false);

                        a.Add(0, 1, new Animation(val => { value = val; TryEvaluate(anim.Expression, _backingStore, false); }, anim.From, anim.To, anim.EasingFunc, () => TryEvaluate(anim.FinishedExpression, _backingStore, false)));
                    }
                    a.Commit(previousViewAsView, "PreviousAnimation", 16, OutDuration, null, null, () => false);
                }
                else
                {
                    value = 1.0;
                    foreach (var anim in OutAnimations)
                        TryEvaluate(anim.Expression, _backingStore, false);
                }
            }
            if (_currentView is View currentViewAsView)
            {
                if (currentViewAsView.IsLoaded)
                {
                    var a = new Animation();

                    foreach (var anim in InAnimations)
                    {
                        TryEvaluate(anim.StartingExpression, _backingStore, true);
                        a.Add(0, 1, new Animation(val => { value = val; TryEvaluate(anim.Expression, _backingStore, true); }, anim.From, anim.To, anim.EasingFunc, () => TryEvaluate(anim.FinishedExpression, _backingStore, true)));
                    }
                    a.Commit(currentViewAsView, "CurrentAnimation", 16, InDuration, null, null, () => false);
                }
                else
                {
                    value = 1.0;
                    foreach (var anim in InAnimations)
                        TryEvaluate(anim.Expression, _backingStore, true);
                }
            }

            // Visibilty has changed, so arrange it, in case it wasn't visible for the initial arrange call.
            (this as IView).InvalidateArrange();
        }

        private bool TryEvaluate(ExpressionTree compiledExpression, PocoBackingStore backingStore, bool isInAnimation)
        {
            View = isInAnimation ? _currentView : _previousView;
            try
            {
                compiledExpression.Evaluate(backingStore);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        #endregion

        public double value { get; protected set; }

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
