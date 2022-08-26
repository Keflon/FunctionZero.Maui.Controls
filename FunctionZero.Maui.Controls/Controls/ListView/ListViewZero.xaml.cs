using FunctionZero.Maui.Controls;
using System.Collections;
using System.Collections.ObjectModel;

namespace FunctionZero.Maui.Controls;

public partial class ListViewZero : ContentView
{
    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(ListViewZero), null, BindingMode.OneWay, null, ItemsSourceChanged);

    public IList ItemsSource
    {
        get { return (IList)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }

    private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ListViewZero)bindable;
        self.UpdateItemContainers();
    }

    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(ListViewZero), null, BindingMode.OneWay, null);

    public DataTemplate ItemTemplate
    {
        get { return (DataTemplate)GetValue(ItemTemplateProperty); }
        set { SetValue(ItemTemplateProperty, value); }
    }

    public static readonly BindableProperty ScrollOffsetProperty = BindableProperty.Create(nameof(ScrollOffset), typeof(float), typeof(ListViewZero), (float)0.0, BindingMode.OneWay, null, null, ScrollOffsetChanged);

    public float ScrollOffset
    {
        get { return (float)GetValue(ScrollOffsetProperty); }
        set { SetValue(ScrollOffsetProperty, value); }
    }

    private static void ScrollOffsetChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ListViewZero)bindable;
        self.UpdateItemContainers();
    }
    public static readonly BindableProperty ItemHeightProperty = BindableProperty.Create(nameof(ItemHeight), typeof(float), typeof(ListViewZero), (float)40.0, BindingMode.OneWay, null);

    public float ItemHeight
    {
        get { return (float)GetValue(ItemHeightProperty); }
        set { SetValue(ItemHeightProperty, value); }
    }

    private Dictionary<Type, Stack<ListItemZero>> _cache;

    public ListViewZero()
    {
        _cache = new();

        InitializeComponent();

        canvas.SizeChanged += Canvas_SizeChanged;

        var gr = new PanGestureRecognizer();

        gr.PanUpdated += Gr_PanUpdated;

        this.GestureRecognizers.Add(gr);
    }

    private void Canvas_SizeChanged(object sender, EventArgs e)
    {
        UpdateItemContainers();
    }

    float _anchor;
    int _firstVisibleItemIndex = int.MaxValue;
    int _lastVisibleItemIndex = -1;

    private void Gr_PanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _anchor = ScrollOffset;
                break;
            case GestureStatus.Running:
                ScrollOffset = _anchor - (float)(e.TotalY);
                break;
            case GestureStatus.Completed:
                break;
            case GestureStatus.Canceled:
                ScrollOffset = _anchor;
                break;
        }
    }


    private void UpdateItemContainers()
    {

        if (canvas.Height <= 0)
            return;
        // Generate item-containers ...

        // Step 1 -> Determine how many items are coming into view and create them.
        // Step 2 -> Update the Y offset of every container.
        // Step 3 -> Remove containers that are no longer visible and give them to the cache.

        // Find the first item that is to be in view
        int firstVisibleIndex = Math.Max(0, (int)(ScrollOffset / ItemHeight));

        // Find the last item that is be in view
        //int lastVisibleIndex = (int)(ScrollOffset / ItemHeight + (canvas.Height + ItemHeight - 1) / ItemHeight) - 1;

        // Maximum number of ListItem instances that can be at least partially seen.
        int maxVisibleContainers = (int)(canvas.Height / ItemHeight) + 1;


        int lastVisibleIndex = Math.Min(ItemsSource.Count-1, firstVisibleIndex + maxVisibleContainers);

        for (int c = firstVisibleIndex; c <= lastVisibleIndex; c++)
        {
            if ((c < _firstVisibleItemIndex) || (c > _lastVisibleItemIndex))
            {
                ListItemZero itemContainer = GetView(c);
                canvas.Add(itemContainer);
            }
        }
        _firstVisibleItemIndex = int.MaxValue;
        _lastVisibleItemIndex = -1;

        var killList = new List<View>();

        int numVisibleItems = (int)((canvas.Height + ItemHeight - 1) / ItemHeight);

        foreach (object obj in canvas)
        {
            if (obj is ListItemZero item)
            {
                // Determine offset for item.
                float itemOffset = item.ItemIndex * ItemHeight - ScrollOffset;

                if ((itemOffset < -ItemHeight) || (itemOffset > canvas.Height) || (item.ItemIndex >= ItemsSource.Count))
                {
                    killList.Add(item);
                }
                else
                {
                    item.TranslationY = itemOffset;

                    _firstVisibleItemIndex = Math.Min(_firstVisibleItemIndex, item.ItemIndex);
                    _lastVisibleItemIndex = Math.Max(_lastVisibleItemIndex, item.ItemIndex);

                }
            }
        }

        foreach (ListItemZero item in killList)
        {
            canvas.Remove(item);
            AddToCache(item.BindingContext.GetType(), item);
        }

        TestLabel.Text = $"Active: {canvas.Count}";
    }

    private void AddToCache(Type type, ListItemZero item)
    {
        if (_cache.TryGetValue(type, out var stack))
        {
            stack.Push(item);
        }
        else
        {
            var newStack = new Stack<ListItemZero>();
            newStack.Push(item);
            _cache.Add(type, newStack);
        }
    }

    private ListItemZero GetView(int itemIndex)
    {
        object item = ItemsSource[itemIndex];
        ListItemZero retVal = null;

        if (_cache.TryGetValue(item.GetType(), out var typeStack))
        {
            if (typeStack.TryPop(out var view))
                retVal = view;
        }

        if (retVal == null)
        {
            // Create one.
            DataTemplate template;

            if (ItemTemplate is DataTemplateSelector selector)
                template = selector.SelectTemplate(item, this);
            else
                template = ItemTemplate;

            retVal = new ListItemZero();
            retVal.Content = (View)template.CreateContent();
        }

        retVal.HeightRequest = ItemHeight;
        retVal.WidthRequest = 200;
        retVal.ItemIndex = itemIndex;
        retVal.BindingContext = item;

        return retVal;
    }
}
