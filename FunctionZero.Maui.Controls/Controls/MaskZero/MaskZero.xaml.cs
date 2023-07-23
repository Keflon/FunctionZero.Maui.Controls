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
    public MaskZero()
    {
        _mv = new MaskViewZero();

        InitializeComponent();

        _viewLookup = new();

        CenterX = 0;
        CenterY = 0;
        Radius = 50;
        //_ = BadTestAsync();

        DescendantAdded += MaskZero_DescendantAdded;
        DescendantRemoved += MaskZero_DescendantRemoved;
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
        if(e.Element is ScrollView scrollView)
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

    private async Task BadTestAsync()
    {
        while (true)
        {
            await Task.Delay(2000);
            CenterXRequest = 200;
            await Task.Delay(2000);
            CenterYRequest = 200;
            await Task.Delay(2000);
            //CenterXRequest = 300;
            //CenterYRequest = 250;
            RadiusRequest = 100;
            await Task.Delay(2000);
            CenterXRequest = 30;
            CenterYRequest = 25;
            RadiusRequest = 10;
        }
    }

    #region bindable properties

    #region CenterXProperty

    public static readonly BindableProperty CenterXProperty = BindableProperty.Create(nameof(CenterX), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, CenterXChanged);

    public double CenterX
    {
        get { return (double)GetValue(CenterXProperty); }
        private set { SetValue(CenterXProperty, value); }
    }

    private static void CenterXChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
        self.RequestUpdate();
    }

    #endregion

    #region CenterYProperty

    public static readonly BindableProperty CenterYProperty = BindableProperty.Create(nameof(CenterY), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, CenterYChanged);

    public double CenterY
    {
        get { return (double)GetValue(CenterYProperty); }
        private set { SetValue(CenterYProperty, value); }
    }

    private static void CenterYChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
        self.RequestUpdate();

    }
    private bool _updateRequested = false;
    private View _actualTarget;

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
        if ((_actualTarget != null))
        {
            var point = GetScreenCoords(_actualTarget);


            CenterXRequest = point.X + _actualTarget.Width/2;
            CenterYRequest = point.Y + _actualTarget.Height/2;

            RadiusRequest = _actualTarget.Width/2;

        }

        _mv.Update(CenterX, CenterY, Radius, BackgroundAlpha, Colors.Green, Colors.Blue, 1);

        _gv?.Invalidate();
        _updateRequested = false;
    }

    #endregion

    #region RadiusProperty

    public static readonly BindableProperty RadiusProperty = BindableProperty.Create(nameof(Radius), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, RadiusChanged);

    public double Radius
    {
        get { return (double)GetValue(RadiusProperty); }
        private set { SetValue(RadiusProperty, value); }
    }

    private static void RadiusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;
        self.RequestUpdate();
    }

    #endregion

    /// <summary>
    /// /////////
    /// </summary>
        #region CenterXRequestProperty

    public static readonly BindableProperty CenterXRequestProperty = BindableProperty.Create(nameof(CenterXRequest), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, CenterXRequestChanged);

    public double CenterXRequest
    {
        get { return (double)GetValue(CenterXRequestProperty); }
        set { SetValue(CenterXRequestProperty, value); }
    }

    private static void CenterXRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;

        self.AbortAnimation("XAnimation");

        double startValue = oldValue is double ? (double)oldValue : 0;
        double endValue = newValue is double ? (double)newValue : 0;

        var animation = new Animation(v => self.CenterX = v, startValue, endValue);
        animation.Commit(self, "XAnimation", 16, 800, self.MovementEasing, (v, c) => self.CenterX = endValue, () => false);
    }

    #endregion

    #region CenterYRequestProperty

    public static readonly BindableProperty CenterYRequestProperty = BindableProperty.Create(nameof(CenterYRequest), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, CenterYRequestChanged);

    public double CenterYRequest
    {
        get { return (double)GetValue(CenterYRequestProperty); }
        set { SetValue(CenterYRequestProperty, value); }
    }

    private static void CenterYRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;

        self.AbortAnimation("YAnimation");

        double startValue = oldValue is double ? (double)oldValue : 0;
        double endValue = newValue is double ? (double)newValue : 0;

        var animation = new Animation(v => self.CenterY = v, startValue, endValue);
        animation.Commit(self, "YAnimation", 16, 800, self.MovementEasing, (v, c) => self.CenterY = endValue, () => false);
    }

    #endregion

    #region RadiusRequestProperty

    public static readonly BindableProperty RadiusRequestProperty = BindableProperty.Create(nameof(RadiusRequest), typeof(double), typeof(MaskZero), null, BindingMode.OneWay, null, RadiusRequestChanged);

    public double RadiusRequest
    {
        get { return (double)GetValue(RadiusRequestProperty); }
        set { SetValue(RadiusRequestProperty, value); }
    }

    private static void RadiusRequestChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (MaskZero)bindable;

        self.AbortAnimation("RadiusAnimation");

        double startValue = oldValue is double ? (double)oldValue : 0;
        double endValue = newValue is double ? (double)newValue : 0;

        var animation = new Animation(v => self.Radius = v, startValue, endValue);
        animation.Commit(self, "RadiusAnimation", 16, 800, self.RadiusEasing, (v, c) => self.Radius = endValue, () => false);
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

    #region RadiusEasingProperty

    public static readonly BindableProperty RadiusEasingProperty = BindableProperty.Create(nameof(RadiusEasing), typeof(Easing), typeof(MaskZero), Easing.CubicInOut, BindingMode.OneWay, null, RadiusEasingChanged);

    public Easing RadiusEasing
    {
        get { return (Easing)GetValue(RadiusEasingProperty); }
        set { SetValue(RadiusEasingProperty, value); }
    }

    private static void RadiusEasingChanged(BindableObject bindable, object oldValue, object newValue)
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

            self.RequestUpdate();
        }
        else
            self._actualTarget = null;
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

            if(element is IScrollView scrollView)
            {
                x -= scrollView.HorizontalOffset;
                y -= scrollView.VerticalOffset;
            }

            element = element.Parent as VisualElement;
        }
        return new Point(x, y);
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
            {
                parentMaskZero.AddDescendant(bindable, oldValue, newValue);
            }
        }
        Debug.WriteLine($"Object: {bindable}, oldValue: {oldValue}, newValue: {newValue}");
    }



    private static T FindAncestor<T>(Element namedElement) where T : Element
    {
        if (namedElement is null)
            return null;

        if (namedElement is T result)
            return result;

        return FindAncestor<T>(namedElement.Parent);
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


    private void AddDescendant(BindableObject namedObject, object oldValue, object newValue)
    {
        _viewLookup[(string)newValue] = namedObject;
    }


    private void GraphicsView_ParentChanged(object sender, EventArgs e)
    {
        _gv = (GraphicsView)sender;
        _gv.Drawable = _mv;

    }




}