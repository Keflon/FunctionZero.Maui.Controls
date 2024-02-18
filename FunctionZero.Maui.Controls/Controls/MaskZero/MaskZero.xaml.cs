using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Specialized;
using System.Diagnostics;

namespace FunctionZero.Maui.Controls;

public partial class MaskZero : ContentView
{
    private Dictionary<string, BindableObject> _viewLookup;
    private GraphicsView _gv;
    private readonly MaskViewZero _mv;
    private bool _updateRequested = false;
    private View _actualTarget;
    private double _alphaMultiplier;
    private bool _loaded;

    public MaskZero()
    {
        _mv = new MaskViewZero();

        InitializeComponent();

        _viewLookup = new();

        MaskLeft = 0;
        MaskTop = 0;
        MaskRoundnessRequest = 1.0;

        DescendantAdded += MaskZero_DescendantAdded;
        DescendantRemoved += MaskZero_DescendantRemoved;
    }



    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        RequestUpdate();
    }

    private void MaskZero_DescendantRemoved(object sender, ElementEventArgs e)
    {
        var name = e.Element.GetValue(MaskNameProperty) as string;
        if (!string.IsNullOrEmpty(name))
        {
            _viewLookup.Remove(name);
        }

        if (e.Element is ScrollView scrollView)
        {
            scrollView.Scrolled -= ScrollView_Scrolled;
        }
    }

    private void MaskZero_DescendantAdded(object sender, ElementEventArgs e)
    {
        if (e.Element is ScrollView scrollView)
        {
            scrollView.Scrolled += ScrollView_Scrolled;
        }

        var name = e.Element.GetValue(MaskNameProperty) as string;
        if (!string.IsNullOrEmpty(name))
        {
            _viewLookup[name] = e.Element;
        }
    }

    private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
        RequestUpdate();
    }

    private void RequestUpdate()
    {
        if (_updateRequested == false)
        {
            _updateRequested = true;
            this.Dispatcher.Dispatch(DoUpdate);
        }
    }

    private void DoUpdate()
    {
        double delta = 0;
        if ((_actualTarget != null))
        {
            var point = GetScreenCoords(_actualTarget);

            MaskLeftRequest = point.X;
            MaskTopRequest = point.Y;
            MaskWidthRequest = _actualTarget.Width;
            MaskHeightRequest = _actualTarget.Height;
        }
        var radius = Math.Min(MaskWidth, MaskHeight) / 2.0 * MaskRoundness;

        delta = (1 - Math.Sin(Math.PI / 4)) * radius;

        _mv.Update(MaskLeft - delta, MaskTop - delta, MaskWidth + delta + delta, MaskHeight + delta + delta, MaskRoundness, BackgroundAlpha, MaskColor, MaskEdgeColor, MaskEdgeThickness, _alphaMultiplier);

        _gv?.Invalidate();
        _updateRequested = false;
    }


    /// <summary>
    /// A view's default X- and Y-coordinates are LOCAL with respect to the boundaries of its parent,
    /// and NOT with respect to the screen. This method calculates the SCREEN coordinates of a view.
    /// The coordinates returned refer to the top left corner of the view.
    /// </summary>
    public static Point GetScreenCoords(VisualElement element)
    {
        double x = 0; double y = 0;

        while (element != null)
        {
            x += element.Bounds.Left;
            y += element.Bounds.Top;

            if (element is IScrollView scrollView)
            {
                x -= scrollView.HorizontalOffset;
                y -= scrollView.VerticalOffset;
            }

            element = element.Parent as VisualElement;
        }
        return new Point(x, y);
    }

    private static T FindAncestor<T>(Element namedElement) where T : Element
    {
        if (namedElement is null)
            return null;

        if (namedElement is T result)
            return result;

        return FindAncestor<T>(namedElement.Parent);
    }

    private void AddDescendant(string name, BindableObject namedObject)
    {
        _viewLookup[name] = namedObject;
    }

    private void GraphicsView_ParentChanged(object sender, EventArgs e)
    {
        _gv = (GraphicsView)sender;
        _gv.Drawable = _mv;

    }

    private void AnimateColor(string name, Color startValue, Color endValue, Action<float, float, float> thing)
    {
        float r = 0, g = 0, b = 0;

        var a = new Animation
        {
            {0, 1, new Animation(val => {r = (float)val;thing(r, g, b);}, startValue.Red, endValue.Red, Easing.CubicInOut)},
            {0, 1, new Animation(val => {g = (float)val;thing(r, g, b);}, startValue.Green, endValue.Green, Easing.CubicInOut)},
            {0, 1, new Animation(val => {b = (float)val;thing(r, g, b); }, startValue.Blue, endValue.Blue, Easing.CubicInOut)}
        };
        a.Commit(this, name, 16, Duration, Easing.Linear, null, () => false);
    }

    #region bindable properties

    #region MaskLeftProperty

    public static readonly BindableProperty MaskLeftProperty = BindableProperty.Create(nameof(MaskLeft), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, MaskLeftChanged);

    public double MaskLeft
    {
        get { return (double)GetValue(MaskLeftProperty); }
        private set { SetValue(MaskLeftProperty, value); }
    }

    private static void MaskLeftChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
        self.RequestUpdate();
    }

    #endregion

    #region MaskTopProperty

    public static readonly BindableProperty MaskTopProperty = BindableProperty.Create(nameof(MaskTop), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, MaskTopChanged);

    public double MaskTop
    {
        get { return (double)GetValue(MaskTopProperty); }
        private set { SetValue(MaskTopProperty, value); }
    }

    private static void MaskTopChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
        self.RequestUpdate();

    }

    #endregion

    #region MaskWidthProperty

    public static readonly BindableProperty MaskWidthProperty = BindableProperty.Create(nameof(MaskWidth), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, MaskWidthChanged);

    public double MaskWidth
    {
        get { return (double)GetValue(MaskWidthProperty); }
        private set { SetValue(MaskWidthProperty, value); }
    }

    private static void MaskWidthChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
        self.RequestUpdate();

    }

    #endregion

    #region MaskHeightProperty

    public static readonly BindableProperty MaskHeightProperty = BindableProperty.Create(nameof(MaskHeight), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, MaskHeightChanged);

    public double MaskHeight
    {
        get { return (double)GetValue(MaskHeightProperty); }
        private set { SetValue(MaskHeightProperty, value); }
    }

    private static void MaskHeightChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
        self.RequestUpdate();

    }

    #endregion

    #region MaskRoundnessProperty

    public static readonly BindableProperty MaskRoundnessProperty = BindableProperty.Create(nameof(MaskRoundness), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, MaskRoundnessChanged);

    public double MaskRoundness
    {
        get { return (double)GetValue(MaskRoundnessProperty); }
        private set { SetValue(MaskRoundnessProperty, value); }
    }

    private static void MaskRoundnessChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
        self.RequestUpdate();
    }

    #endregion

    #region DurationProperty

    public static readonly BindableProperty DurationProperty = BindableProperty.Create(nameof(Duration), typeof(uint), typeof(MaskZero), (uint)1, BindingMode.OneWay, null, DurationChanged);

    public uint Duration
    {
        get { return (uint)GetValue(DurationProperty); }
        set { SetValue(DurationProperty, value); }
    }

    private static void DurationChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
        //self.RequestUpdate();
    }

    #endregion

    #region MaskColorProperty

    public static readonly BindableProperty MaskColorProperty = BindableProperty.Create(nameof(MaskColor), typeof(Color), typeof(MaskZero), Colors.Black, BindingMode.OneWay, null, MaskColorChanged);

    public Color MaskColor
    {
        get { return (Color)GetValue(MaskColorProperty); }
        set { SetValue(MaskColorProperty, value); }
    }

    private static void MaskColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
        self.RequestUpdate();
    }

    #endregion

    #region MaskEdgeColorProperty

    public static readonly BindableProperty MaskEdgeColorProperty = BindableProperty.Create(nameof(MaskEdgeColor), typeof(Color), typeof(MaskZero), Colors.Black, BindingMode.OneWay, null, MaskEdgeColorChanged);

    public Color MaskEdgeColor
    {
        get { return (Color)GetValue(MaskEdgeColorProperty); }
        set { SetValue(MaskEdgeColorProperty, value); }
    }

    private static void MaskEdgeColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
        self.RequestUpdate();
    }

    #endregion

    #region MaskEdgeThicknessProperty

    public static readonly BindableProperty MaskEdgeThicknessProperty = BindableProperty.Create(nameof(MaskEdgeThickness), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, MaskEdgeThicknessChanged);

    public double MaskEdgeThickness
    {
        get { return (double)GetValue(MaskEdgeThicknessProperty); }
        private set { SetValue(MaskEdgeThicknessProperty, value); }
    }

    private static void MaskEdgeThicknessChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
        self.RequestUpdate();
    }

    #endregion

    /// <summary>
    /// /////////
    /// </summary>
    #region MaskLeftRequestProperty

    public static readonly BindableProperty MaskLeftRequestProperty = BindableProperty.Create(nameof(MaskLeftRequest), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, MaskLeftRequestChanged);

    public double MaskLeftRequest
    {
        get { return (double)GetValue(MaskLeftRequestProperty); }
        set { SetValue(MaskLeftRequestProperty, value); }
    }

    private static void MaskLeftRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;

        self.AbortAnimation("MaskLeftAnimation");

        double startValue = oldValue is double ? (double)oldValue : 0;
        double endValue = newValue is double ? (double)newValue : 0;

        var animation = new Animation(v => self.MaskLeft = v, startValue, endValue);
        animation.Commit(self, "MaskLeftAnimation", 16, self.Duration, self.MovementEasing, (v, c) => self.MaskLeft = endValue, () => false);
    }

    #endregion

    #region MaskTopRequestProperty

    public static readonly BindableProperty MaskTopRequestProperty = BindableProperty.Create(nameof(MaskTopRequest), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, MaskTopRequestChanged);

    public double MaskTopRequest
    {
        get { return (double)GetValue(MaskTopRequestProperty); }
        set { SetValue(MaskTopRequestProperty, value); }
    }

    private static void MaskTopRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;

        self.AbortAnimation("MaskTopAnimation");

        double startValue = oldValue is double ? (double)oldValue : 0;
        double endValue = newValue is double ? (double)newValue : 0;

        var animation = new Animation(v => self.MaskTop = v, startValue, endValue);
        animation.Commit(self, "MaskTopAnimation", 16, self.Duration, self.MovementEasing, (v, c) => self.MaskTop = endValue, () => false);
    }

    #endregion

    #region MaskWidthRequestProperty

    public static readonly BindableProperty MaskWidthRequestProperty = BindableProperty.Create(nameof(MaskWidthRequest), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, MaskWidthRequestChanged);

    public double MaskWidthRequest
    {
        get { return (double)GetValue(MaskWidthRequestProperty); }
        set { SetValue(MaskWidthRequestProperty, value); }
    }

    private static void MaskWidthRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
        self.AbortAnimation("MaskWidthAnimation");

        double startValue = oldValue is double ? (double)oldValue : 0;
        double endValue = newValue is double ? (double)newValue : 0;

        var animation = new Animation(v => self.MaskWidth = v, startValue, endValue);
        animation.Commit(self, "MaskWidthAnimation", 16, self.Duration, self.MovementEasing, (v, c) => self.MaskWidth = endValue, () => false);

    }

    #endregion

    #region MaskHeightRequestProperty

    public static readonly BindableProperty MaskHeightRequestProperty = BindableProperty.Create(nameof(MaskHeightRequest), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, MaskHeightRequestChanged);

    public double MaskHeightRequest
    {
        get { return (double)GetValue(MaskHeightRequestProperty); }
        set { SetValue(MaskHeightRequestProperty, value); }
    }

    private static void MaskHeightRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
        self.AbortAnimation("MaskHeightAnimation");

        double startValue = oldValue is double ? (double)oldValue : 0;
        double endValue = newValue is double ? (double)newValue : 0;

        var animation = new Animation(v => self.MaskHeight = v, startValue, endValue);
        animation.Commit(self, "MaskHeightAnimation", 16, self.Duration, self.MovementEasing, (v, c) => self.MaskHeight = endValue, () => false);

    }

    #endregion

    #region MaskRoundnessRequestProperty

    public static readonly BindableProperty MaskRoundnessRequestProperty = BindableProperty.Create(nameof(MaskRoundnessRequest), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, MaskRoundnessRequestChanged);

    public double MaskRoundnessRequest
    {
        get { return (double)GetValue(MaskRoundnessRequestProperty); }
        set { SetValue(MaskRoundnessRequestProperty, value); }
    }

    private static void MaskRoundnessRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;

        self.AbortAnimation("MaskRoundnessAnimation");

        double startValue = oldValue is double ? (double)oldValue : 0;
        double endValue = newValue is double ? (double)newValue : 0;

        var animation = new Animation(v => self.MaskRoundness = v, startValue, endValue);
        animation.Commit(self, "MaskRoundnessAnimation", 16, self.Duration, self.MaskRoundnessEasing, (v, c) => self.MaskRoundness = endValue, () => false);
    }

    #endregion

    #region MovementEasingProperty

    public static readonly BindableProperty MovementEasingProperty = BindableProperty.Create(nameof(MovementEasing), typeof(Easing), typeof(MaskZero), Easing.CubicInOut, BindingMode.OneWay, null, MovementEasingChanged);

    public Easing MovementEasing
    {
        get { return (Easing)GetValue(MovementEasingProperty); }
        set { SetValue(MovementEasingProperty, value); }
    }

    private static void MovementEasingChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
    }

    #endregion

    #region MaskRoundnessEasingProperty

    public static readonly BindableProperty MaskRoundnessEasingProperty = BindableProperty.Create(nameof(MaskRoundnessEasing), typeof(Easing), typeof(MaskZero), Easing.CubicInOut, BindingMode.OneWay, null, MaskRoundnessEasingChanged);

    public Easing MaskRoundnessEasing
    {
        get { return (Easing)GetValue(MaskRoundnessEasingProperty); }
        set { SetValue(MaskRoundnessEasingProperty, value); }
    }

    private static void MaskRoundnessEasingChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
    }

    #endregion

    #region BackgroundAlphaProperty

    public static readonly BindableProperty BackgroundAlphaProperty = BindableProperty.Create(nameof(BackgroundAlpha), typeof(double), typeof(MaskZero), 0.7, BindingMode.OneWay, null, BackgroundAlphaChanged);

    public double BackgroundAlpha
    {
        get { return (double)GetValue(BackgroundAlphaProperty); }
        set { SetValue(BackgroundAlphaProperty, value); }
    }

    private static void BackgroundAlphaChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
        self.RequestUpdate();
    }

    #endregion


    #region BackgroundAlphaRequestProperty

    public static readonly BindableProperty BackgroundAlphaRequestProperty = BindableProperty.Create(nameof(BackgroundAlphaRequest), typeof(double), typeof(MaskZero), 0.7, BindingMode.OneWay, null, BackgroundAlphaRequestChanged);

    public double BackgroundAlphaRequest
    {
        get { return (double)GetValue(BackgroundAlphaRequestProperty); }
        set { SetValue(BackgroundAlphaRequestProperty, value); }
    }

    private static void BackgroundAlphaRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;

        self.AbortAnimation("BackgroundAlphaAnimation");

        double startValue = oldValue is double ? (double)oldValue : 0;
        double endValue = newValue is double ? (double)newValue : 0;

        var animation = new Animation(v => self.BackgroundAlpha = v, startValue, endValue);
        animation.Commit(self, "BackgroundAlphaAnimation", 16, self.Duration, self.MaskRoundnessEasing, (v, c) => self.BackgroundAlpha = endValue, () => false);
    }

    #endregion



    #region MaskColorRequestProperty

    public static readonly BindableProperty MaskColorRequestProperty = BindableProperty.Create(nameof(MaskColorRequest), typeof(Color), typeof(MaskZero), Colors.Black, BindingMode.OneWay, null, MaskColorRequestChanged);

    public Color MaskColorRequest
    {
        get { return (Color)GetValue(MaskColorRequestProperty); }
        set { SetValue(MaskColorRequestProperty, value); }
    }

    private static void MaskColorRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;

        self.AbortAnimation("MaskColorAnimation");

        Color startValue = self.MaskColor;
        Color endValue = newValue is Color ? (Color)newValue : Colors.Black;
        self.AnimateColor("MaskColorAnimation", startValue, endValue, (r, g, b) => { self.MaskColor = new Color(r, g, b); self.RequestUpdate(); });
    }

    #endregion

    #region MaskEdgeColorRequestProperty

    public static readonly BindableProperty MaskEdgeColorRequestProperty = BindableProperty.Create(nameof(MaskEdgeColorRequest), typeof(Color), typeof(MaskZero), Colors.Black, BindingMode.OneWay, null, MaskEdgeColorRequestChanged);

    public Color MaskEdgeColorRequest
    {
        get { return (Color)GetValue(MaskEdgeColorRequestProperty); }
        set { SetValue(MaskEdgeColorRequestProperty, value); }
    }

    private static void MaskEdgeColorRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
        self.AbortAnimation("MaskEdgeColorAnimation");

        Color startValue = self.MaskColor;
        Color endValue = newValue is Color ? (Color)newValue : Colors.Black;
        self.AnimateColor("MaskEdgeColorAnimation", startValue, endValue, (r, g, b) => { self.MaskEdgeColor = new Color(r, g, b); self.RequestUpdate(); });


    }

    #endregion

    #region MaskEdgeThicknessRequestProperty

    public static readonly BindableProperty MaskEdgeThicknessRequestProperty = BindableProperty.Create(nameof(MaskEdgeThicknessRequest), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, MaskEdgeThicknessRequestChanged);

    public double MaskEdgeThicknessRequest
    {
        get { return (double)GetValue(MaskEdgeThicknessRequestProperty); }
        private set { SetValue(MaskEdgeThicknessRequestProperty, value); }
    }

    private static void MaskEdgeThicknessRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;

        self.AbortAnimation("EdgeThicknessAnimation");

        double startValue = oldValue is double ? (double)oldValue : 0;
        double endValue = newValue is double ? (double)newValue : 0;

        var animation = new Animation(v => self.MaskEdgeThickness = v, startValue, endValue);
        animation.Commit(self, "EdgeThicknessAnimation", 16, self.Duration, self.MaskRoundnessEasing, (v, c) => self.MaskEdgeThickness = endValue, () => false);
    }

    #endregion

    #region MaskTargetNameProperty

    public static readonly BindableProperty MaskTargetNameProperty = BindableProperty.Create(nameof(MaskTargetName), typeof(string), typeof(MaskZero), "", BindingMode.OneWay, null, MaskTargetNameChanged);

    public string MaskTargetName
    {
        get { return (string)GetValue(MaskTargetNameProperty); }
        set { SetValue(MaskTargetNameProperty, value); }
    }

    private static void MaskTargetNameChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;

        // Get bounds of the control
        if (self._viewLookup.TryGetValue(newValue as string ?? "", out var target))
        {
            self._actualTarget = (View)target;

            self.AbortAnimation("_alphaMultiplierAnimation");

            var animation = new Animation(v => { self._alphaMultiplier = v; self.RequestUpdate(); }, self._alphaMultiplier, 1);
            animation.Commit(self, "_alphaMultiplierAnimation", 16, self.Duration, Easing.Linear, (v, c) => self._alphaMultiplier = 1, () => false);

            self.RequestUpdate();
        }
        else
        {
            self._actualTarget = null;

            var radius = 600;

            self.MaskLeftRequest = self.MaskLeft - radius;
            self.MaskTopRequest = self.MaskTop - radius;
            self.MaskWidthRequest = self.MaskWidth + radius + radius;
            self.MaskHeightRequest = self.MaskHeight + radius + radius;

            self.AbortAnimation("_alphaMultiplierAnimation");

            var animation = new Animation(v => { self._alphaMultiplier = v; self.RequestUpdate(); }, self._alphaMultiplier, 0);
            animation.Commit(self, "_alphaMultiplierAnimation", 16, self.Duration, Easing.Linear, (v, c) => self._alphaMultiplier = 0, () => false);
        }
    }

    #endregion

    #endregion

    #region AttachedProperties

    public static readonly BindableProperty MaskNameProperty =
BindableProperty.CreateAttached("MaskName", typeof(string), typeof(MaskZero), "", BindingMode.OneWay, null, MaskNamePropertyChanged);

    private static void MaskNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Element namedElement)
        {
            var parentMaskZero = FindAncestor<MaskZero>(namedElement);

            if (parentMaskZero != null)
                parentMaskZero.AddDescendant((string)newValue, bindable);
        }
        Debug.WriteLine($"Object: {bindable}, oldValue: {oldValue}, newValue: {newValue}");
    }

    public static string GetMaskName(BindableObject view)
    {
        return (string)view.GetValue(MaskNameProperty);
    }

    public static void SetMaskName(BindableObject view, string value)
    {
        view.SetValue(MaskNameProperty, value);
    }
    #endregion
}