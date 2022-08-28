namespace FunctionZero.Maui.Controls;

public partial class Chevron : ContentView
{
    public Chevron()
    {
        InitializeComponent();
            TheImage.Source = new FontImageSource() { Glyph = ">", Color = Colors.Gray, Size = 40 };
    }

    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(Chevron), false, BindingMode.TwoWay, null, IsExpandedChanged);

    public bool IsExpanded
    {
        get { return (bool)GetValue(IsExpandedProperty); }
        set { SetValue(IsExpandedProperty, value); }
    }

    private static void IsExpandedChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var self = (Chevron)bindable;

        //if (self.IsExpanded)
        //    self.RotateTo(90);
        //else
        //    self.RotateTo(0);

        self.Rotation = self.IsExpanded ? 90 : 0;
    }


    public static readonly BindableProperty ShowChevronProperty = BindableProperty.Create(nameof(ShowChevron), typeof(bool), typeof(Chevron), true, BindingMode.OneWay, null, ShowChevronChanged);

    public bool ShowChevron
    {
        get { return (bool)GetValue(ShowChevronProperty); }
        set { SetValue(ShowChevronProperty, value); }
    }

    private static void ShowChevronChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        var self = (Chevron)bindable;

        //if (self.ShowChevron)
        //    self.FadeTo(1);
        //else
        //    self.FadeTo(0);

        if (self.ShowChevron)
            self.Opacity = 1;
        else
            self.Opacity = 0;
    }


    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        IsExpanded = !IsExpanded;
    }
}