using Microsoft.Maui.Controls.Shapes;
using System.ComponentModel;
using System.Diagnostics;

namespace FunctionZero.Maui.Controls;

public partial class FocusScrollZero : ContentView
{
    private readonly List<ExpanderZero> _expanderList;
    private ScrollView _theScrollView;
    private Rectangle _theSpacer;

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

    public static Point GetScreenCoords2(VisualElement element)
    {
        double x = element.Bounds.Left;
        double y = element.Bounds.Top;

        element = element.Parent?.Parent as ExpanderZero;

        while (element != null)
        {
            x += element.Bounds.Left;
            y += element.Bounds.Top;
            if (element is ExpanderZero expander)
            {
                x += expander.Header.Width + expander.Header.Margin.HorizontalThickness;
            }
            element = element.Parent?.Parent as ExpanderZero;

        }
        return new Point(x, y);
    }
    public Point GetScreenCoords(VisualElement element)
    {
        double x = 0;
        double y = 0;

        while (element != this)
        {
            x += element.Bounds.Left;
            y += element.Bounds.Top;

            if (element is ExpanderZero expander)
            {
                x += expander.Header.Width + expander.Header.Margin.HorizontalThickness;
                Debug.WriteLine($"Error: {expander.Header.Width + expander.Header.Margin.HorizontalThickness - expander.Header.Bounds.Width}");
            }
            element = element.Parent as VisualElement;
        }
        return new Point(x, y);
    }

    /*

    [HCC][HCC][H[HCC][HCC]
    */

    private async void Expander_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ExpanderZero.IsExpanded))
        {
            var expander = (ExpanderZero)sender;
            await Task.Yield();

            var x = GetScreenCoords(expander).X;

            var thing = x;// + expander.Header.Width + expander.Header.Margin.HorizontalThickness;
            //var thing = x + expander.Header.Bounds.Width;
            var minScrollOffset = thing + expander.PaddingWidth - _theScrollView.Width;

            //await _theScrollView.ScrollToAsync(x, 0, true);
            //await Task.Delay(3000);

            if (_theScrollView.ScrollX < minScrollOffset)
            {
                this.AbortAnimation("SimpleAnimation");
                var animation = new Animation(offset => _theScrollView.ScrollToAsync(offset, 0, false), _theScrollView.ScrollX, minScrollOffset, expander.EaseIn);
                animation.Commit(this, "SimpleAnimation", 16, expander.DurationMilliseconds/*5000*/, Easing.Linear,
                    (v, c) => Finished(x, expander), () => false);


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

    private void Finished(double x, ExpanderZero expander)
    {
        var targetX = x - expander.Header.Width - expander.Header.Margin.HorizontalThickness;
        if (targetX < _theScrollView.ScrollX)
            _theScrollView.ScrollToAsync(targetX, 0, true);
    }
}