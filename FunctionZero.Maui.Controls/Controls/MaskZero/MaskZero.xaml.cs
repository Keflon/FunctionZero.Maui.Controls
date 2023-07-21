using Microsoft.Maui.Controls.Shapes;
using System.Collections.Specialized;

namespace FunctionZero.Maui.Controls;

public partial class MaskZero : ContentView
{
    public MaskZero()
    {
        InitializeComponent();

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
            CenterXRequest = 300;
            CenterYRequest = 250;
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
        self.RequestMove();
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
        self.RequestMove();

    }
    private bool _moveRequested = false;
    private void RequestMove()
    {
        if(_moveRequested == false)
        {
            _moveRequested = true;
            this.Dispatcher.Dispatch(DoMove);
        }
    }

    private void DoMove()
    {
        Cutout.StartPoint = new Point(CenterX, CenterY);
        Arc.Point = new Point(CenterX+1, CenterY);
        this.InvalidateLayout();
        _moveRequested = false;
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

        double radius = newValue is double newRadius ? newRadius : 0;
        self.Arc.Size = new Size(radius, radius);
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
        animation.Commit(self, "XAnimation", 16, 800, Easing.CubicInOut, (v, c) => self.CenterX = endValue, () => false);
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
        animation.Commit(self, "YAnimation", 16, 800, Easing.CubicInOut, (v, c) => self.CenterY = endValue, () => false);
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
        animation.Commit(self, "RadiusAnimation", 16, 800, Easing.CubicInOut, (v, c) => self.Radius = endValue, () => false);
    }

    #endregion

    #endregion

    protected override async void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (Oblong != null)
            Oblong.Rect = new Rect(0, 0, width, height);
    }
}