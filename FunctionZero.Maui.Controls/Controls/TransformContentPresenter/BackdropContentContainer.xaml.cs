namespace FunctionZero.Maui.Controls;

public partial class BackdropContentContainer : ContentView
{
	public BackdropContentContainer()
	{
		InitializeComponent();
	}

    #region BackdropOpacityProperty

    public static readonly BindableProperty BackdropOpacityProperty = BindableProperty.Create(nameof(BackdropOpacity), typeof(double), typeof(BackdropContentContainer), 1.0, BindingMode.OneWay, null, BackdropOpacityChanged);

    public double BackdropOpacity
    {
        get { return (double)GetValue(BackdropOpacityProperty); }
        set { SetValue(BackdropOpacityProperty, value); }
    }

    private static void BackdropOpacityChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (BackdropContentContainer)bindable;
    }

    #endregion

    #region BackdropColorProperty

    public static readonly BindableProperty BackdropColorProperty = BindableProperty.Create(nameof(BackdropColor), typeof(Color), typeof(BackdropContentContainer), Colors.Black, BindingMode.OneWay, null, BackdropColorChanged);

    public Color BackdropColor
    {
        get { return (Color)GetValue(BackdropColorProperty); }
        set { SetValue(BackdropColorProperty, value); }
    }

    private static void BackdropColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (BackdropContentContainer)bindable;
    }

    #endregion
}