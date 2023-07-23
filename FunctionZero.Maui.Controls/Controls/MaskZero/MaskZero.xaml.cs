using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Specialized;
using System.Diagnostics;

namespace FunctionZero.Maui.Controls;

public partial class MaskZero : GraphicsView
{
    private readonly MaskViewZero _mvz;

    public MaskZero()
    {
        InitializeComponent();

        _mvz = new  MaskViewZero();

        _viewLookup = new();

        this.Drawable = _mvz;

        CenterX = 0;
        CenterY = 0;
        Radius = 50;
        _ = BadTestAsync();
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

    private void RequestUpdate()
    {
        if(_updateRequested == false)
        {
            _updateRequested = true;
            this.Dispatcher.Dispatch(DoUpdate);
        }
    }

    private void DoUpdate()
    {
        _mvz.Update(CenterX, CenterY, Radius, BackgroundAlpha, Colors.Green, Colors.Blue, 1 );
        this.Invalidate();
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


    #endregion

    #region AttachedProperties

    public static readonly BindableProperty MaskNameProperty =
    BindableProperty.CreateAttached("MaskName", typeof(string), typeof(MaskZero), "", BindingMode.OneWay, null, MaskNamePropertyChanged);

    private static void MaskNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if(bindable is Element namedElement)
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

    public static void SetHasShadow(BindableObject view, string value)
    {
        view.SetValue(MaskNameProperty, value);
    }
    #endregion


    private Dictionary<string, BindableObject> _viewLookup;

    private void AddDescendant(BindableObject namedObject, object oldValue, object newValue)
    {
        _viewLookup[(string)newValue] = namedObject;
    }
}