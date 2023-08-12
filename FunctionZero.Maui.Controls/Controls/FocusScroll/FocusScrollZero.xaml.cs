namespace FunctionZero.Maui.Controls;

public partial class FocusScrollZero : ContentView
{
	public FocusScrollZero()
	{
		InitializeComponent();
	}

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _theScrollView = (ScrollView)this.GetTemplateChild("TheScrollView");

    }
    public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(FocusScrollZero), StackOrientation.Vertical, BindingMode.OneWay, null, OrientationChanged);
    private ScrollView _theScrollView;

    public StackOrientation Orientation
    {
        get { return (StackOrientation)GetValue(OrientationProperty); }
        set { SetValue(OrientationProperty, value); }
    }

    private static async void OrientationChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (FocusScrollZero)bindable;
        await Task.Yield();
        self.SetOrientation();
    }

    private void SetOrientation()
    {
        switch (Orientation)
        {
            case StackOrientation.Vertical:
                _theScrollView.Orientation = ScrollOrientation.Vertical;
                break;
            case StackOrientation.Horizontal:
                _theScrollView.Orientation = ScrollOrientation.Horizontal;
                break;
        }
    }


    private void TheScrollView_DescendantAdded(object sender, ElementEventArgs e)
    {
        if(e.Element is ExpanderZero expander)
        {
            expander.PropertyChanged += Expander_PropertyChanged;
        }
    }


    private void TheScrollView_DescendantRemoved(object sender, ElementEventArgs e)
    {
        if (sender is ExpanderZero expander)
        {
            expander.PropertyChanged -= Expander_PropertyChanged;
        }
    }


    private async void Expander_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if(e.PropertyName == nameof(ExpanderZero.IsExpanded))
        {
            var expander = (ExpanderZero)sender;

            await _theScrollView.ScrollToAsync(expander, 0, true);
            await Task.Delay((int)expander.DurationMilliseconds);

            await _theScrollView.ScrollToAsync(expander, 0, true);
            await _theScrollView.ScrollToAsync(expander.Header, 0, true);
        }
    }
}