using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;

namespace FunctionZero.Maui.Controls;

public partial class ExpanderZero : ContentView
{
    public ExpanderZero()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(ExpanderZero), false, BindingMode.OneWay, null, IsExpandedChanged);

    public bool IsExpanded
    {
        get { return (bool)GetValue(IsExpandedProperty); }
        set { SetValue(IsExpandedProperty, value); }
    }

    private static void IsExpandedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ExpanderZero)bindable;

        self.UpdateVisualState((bool)newValue);
    }


    public static readonly BindableProperty HeaderProperty = BindableProperty.Create(nameof(Header), typeof(View), typeof(ExpanderZero), null, BindingMode.OneWay, null, HeaderChanged);

    private static void HeaderChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ExpanderZero)bindable;
    }

    public View Header
    {
        get { return (View)GetValue(HeaderProperty); }
        set { SetValue(HeaderProperty, value); }
    }

    public static readonly BindableProperty EaseInProperty = BindableProperty.Create(nameof(EaseIn), typeof(Easing), typeof(ExpanderZero), Easing.Linear, BindingMode.OneWay, null, EaseInChanged);

    private static void EaseInChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ExpanderZero)bindable;
    }

    public Easing EaseIn
    {
        get => (Easing)GetValue(EaseInProperty);
        set => SetValue(HeaderProperty, value);
    }

    public static readonly BindableProperty EaseOutProperty = BindableProperty.Create(nameof(EaseOut), typeof(Easing), typeof(ExpanderZero), Easing.Linear, BindingMode.OneWay, null, EaseOutChanged);

    private static void EaseOutChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ExpanderZero)bindable;
    }

    public Easing EaseOut
    {
        get => (Easing)GetValue(EaseOutProperty);
        set => SetValue(HeaderProperty, value);
    }

    public static readonly BindableProperty DurationMillisecondsProperty = BindableProperty.Create(nameof(DurationMilliseconds), typeof(uint), typeof(ExpanderZero), (uint)500, BindingMode.OneWay, null, DurationMillisecondsChanged);

    private static void DurationMillisecondsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ExpanderZero)bindable;
    }

    public uint DurationMilliseconds
    {
        get { return (uint)GetValue(DurationMillisecondsProperty); }
        set { SetValue(DurationMillisecondsProperty, value); }
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        ContentPresenter container = (ContentPresenter)this.GetTemplateChild("DetailView");
        if (!IsExpanded)
            container.HeightRequest = 0;
    }
    private void UpdateVisualState(bool isExpanded)
    {
        ContentPresenter container = (ContentPresenter)this.GetTemplateChild("DetailView");
        var detail = container.Content;

        if (detail != null)
        {
            Animation animation;

            SizeRequest desiredSize = container.Content.Measure(container.Width, double.PositiveInfinity, MeasureFlags.None);

            this.AbortAnimation("SimpleAnimation");

            if ((isExpanded))
                animation = new Animation(h => container.HeightRequest = h, container.Height, desiredSize.Request.Height, EaseIn);
            else
                animation = new Animation(h => container.HeightRequest = h, container.Height, 0, EaseOut);

            animation.Commit(this, "SimpleAnimation", 16, DurationMilliseconds, Easing.Linear, (v, c) => { }, () => false);
        }
    }

    private void HeaderTapped(object sender, EventArgs e)
    {
        IsExpanded = !IsExpanded;
    }
}