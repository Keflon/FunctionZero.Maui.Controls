using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System.ComponentModel;
using System.Diagnostics;

namespace FunctionZero.Maui.Controls;

public partial class ExpanderZero : ContentView
{
    private ContentPresenter _detailView;
    private bool _loaded;

    public ExpanderZero()
    {
        InitializeComponent();
        this.Loaded += ExpanderZero_Loaded;
    }

    

    private void ExpanderZero_Loaded(object sender, EventArgs e)
    {
        // This dodges something related to this: https://github.com/dotnet/maui/pull/6364
        // where
        // IsExpanded="{z:Bind '!Device.IsDeviceConnected'}"
        // can cause
        // System.ArgumentException: Unable to find IAnimationManager for ... (this control)
        // I guess because z:Bind flips the value when the binding context is set but before the control 
        // is in the visual tree, meaning there is no IAnimationManager available.
        // Note, my logic may be incorrect, but the problem we're dodging is specific to the '!' in the zBind expression.

        _loaded = true;
        UpdateVisualState(IsExpanded);
    }

    public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(ExpanderZero), StackOrientation.Vertical, BindingMode.OneWay, null, OrientationChanged);

    public StackOrientation Orientation
    {
        get { return (StackOrientation)GetValue(OrientationProperty); }
        set { SetValue(OrientationProperty, value); }
    }

    private static async void OrientationChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ExpanderZero)bindable;
        await Task.Yield();
        self.SetOrientation();
    }

    private void SetOrientation()
    {
        var root = (View)this.GetTemplateChild("RootStackLayout");

        ContentPresenter container = _detailView;

        if (Orientation == StackOrientation.Vertical)
        {
            root.HorizontalOptions = LayoutOptions.Fill;
            root.VerticalOptions = LayoutOptions.Fill;
            if (!IsExpanded)
                container.HeightRequest = 0;
            container.ClearValue(WidthRequestProperty);
        }
        else
        {
            root.HorizontalOptions = LayoutOptions.Fill;
            root.VerticalOptions = LayoutOptions.Fill;
            if (!IsExpanded)
                container.WidthRequest = 0;
            container.ClearValue(HeightRequestProperty);
        }
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
        set => SetValue(EaseInProperty, value);
    }

    public static readonly BindableProperty EaseOutProperty = BindableProperty.Create(nameof(EaseOut), typeof(Easing), typeof(ExpanderZero), Easing.Linear, BindingMode.OneWay, null, EaseOutChanged);

    private static void EaseOutChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ExpanderZero)bindable;
    }

    public Easing EaseOut
    {
        get => (Easing)GetValue(EaseOutProperty);
        set => SetValue(EaseInProperty, value);
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

    protected override async void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _detailView = (ContentPresenter)this.GetTemplateChild("DetailView");
        // Needed. Why?
        await Task.Yield();
        SetOrientation();
    }
    private void UpdateVisualState(bool isExpanded)
    {
        if(_loaded == false)
            return;

        ContentPresenter container = _detailView;
        _detailView.IsClippedToBounds = true;

        var detail = container.Content;

        if (detail != null)
        {
            Animation animation;

            this.AbortAnimation("SimpleAnimation");

            if (Orientation == StackOrientation.Horizontal)
            {
                SizeRequest desiredSize = container.Content.Measure(double.PositiveInfinity, container.Height, MeasureFlags.None);
                PaddingWidth = desiredSize.Request.Width - container.Width;
                if (isExpanded)
                    animation = new Animation(w => SetWidth(container, w, desiredSize.Request.Width),
                        container.Width, desiredSize.Request.Width, EaseIn);
                else
                    animation = new Animation(w => container.WidthRequest = w, container.Width, 0, EaseOut);
            }
            else
            {
                SizeRequest desiredSize = container.Content.Measure(container.Width, double.PositiveInfinity, MeasureFlags.None);
                if (isExpanded)
                    animation = new Animation(h => container.HeightRequest = h, container.Height, desiredSize.Request.Height, EaseIn);
                else
                    animation = new Animation(h => container.HeightRequest = h, container.Height, 0, EaseOut);
            }
            animation.Commit(this, "SimpleAnimation", 16, DurationMilliseconds/*5000*/, Easing.Linear, (v, c) => FinishedExpanding(container, isExpanded), () => false);
        }
    }

    private void FinishedExpanding(ContentPresenter container, bool isExpanded)
    {
        if (isExpanded)
        {
            if (Orientation == StackOrientation.Horizontal)
                container.ClearValue(VisualElement.WidthRequestProperty);
            else
                container.ClearValue(VisualElement.HeightRequestProperty);
        }
    }

    public double PaddingWidth { get; protected set; }
    private void SetWidth(ContentPresenter container, double currentWidth, double finalWidth)
    {
        container.WidthRequest = currentWidth;
        PaddingWidth = finalWidth - currentWidth;
    }

    private void HeaderTapped(object sender, TappedEventArgs e)
    {
        IsExpanded = !IsExpanded;
    }
}