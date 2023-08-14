using Microsoft.Maui.Controls.Shapes;
using System.ComponentModel;
using System.Diagnostics;

namespace FunctionZero.Maui.Controls;

public partial class FocusScrollZero : ContentView
{
    private readonly List<ExpanderZero> _expanderList;

    public FocusScrollZero()
    {
        _expanderList = new();

        InitializeComponent();
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _theScrollView = (ScrollView)this.GetTemplateChild("TheScrollView");
        _theSpacer = (Rectangle)this.GetTemplateChild("TheSpacer");

    }
    public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(FocusScrollZero), StackOrientation.Vertical, BindingMode.OneWay, null, OrientationChanged);
    private ScrollView _theScrollView;
    private Rectangle _theSpacer;

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
        if (e.Element is ExpanderZero expander)
        {
            expander.PropertyChanged += Expander_PropertyChanged;
            _expanderList.Add(expander);
        }
    }

    private void TheScrollView_DescendantRemoved(object sender, ElementEventArgs e)
    {
        if (sender is ExpanderZero expander)
        {
            _expanderList.Remove(expander);
            expander.PropertyChanged -= Expander_PropertyChanged;
        }
    }

    private async void Expander_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ExpanderZero.IsExpanded))
        {
            var expander = (ExpanderZero)sender;
            await Task.Yield();

            //var thing = MaskZero.GetScreenCoords(expander);
            var thing = expander.X + expander.Header.Width + expander.Header.Margin.HorizontalThickness;
            var minScrollOffset = thing + expander.PaddingWidth - _theScrollView.Width;

            if (_theScrollView.ScrollX < minScrollOffset)
            {
                this.AbortAnimation("SimpleAnimation");
                var animation = new Animation(offset => _theScrollView.ScrollToAsync(offset, 0, false), _theScrollView.ScrollX, minScrollOffset, expander.EaseIn);
                animation.Commit(this, "SimpleAnimation", 16, expander.DurationMilliseconds/*5000*/, Easing.Linear, (v, c) => { }, () => false);
            }
        }
        else if (e.PropertyName == nameof(ExpanderZero.Width))
        {
            double paddingWidth = 0;

            foreach (ExpanderZero item in _expanderList)
            {
                paddingWidth += item.PaddingWidth;
            }
            _theSpacer.WidthRequest = paddingWidth;
        }
    }
}