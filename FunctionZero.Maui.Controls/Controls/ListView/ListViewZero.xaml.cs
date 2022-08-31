using FunctionZero.Maui.Controls;
using FunctionZero.Maui.Services.Cache;
using Microsoft.Maui.Controls;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;

namespace FunctionZero.Maui.Controls;

public partial class ListViewZero : ContentView
{
    float _anchor;
    int _firstVisibleItemIndex = int.MaxValue;
    int _lastVisibleItemIndex = -1;
    //private Dictionary<DataTemplate, Stack<ListItemZero>> _cache;

    internal BucketDictionary<DataTemplate, ListItemZero> _cache;

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(ListViewZero), null, BindingMode.OneWay, null, ItemsSourceChanged);

    public IList ItemsSource
    {
        get { return (IList)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }

    private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ListViewZero)bindable;

        if (oldValue is INotifyCollectionChanged oldCollection)
            oldCollection.CollectionChanged -= self.Collection_CollectionChanged;

        if (newValue is INotifyCollectionChanged newCollection)
            newCollection.CollectionChanged += self.Collection_CollectionChanged;

        self.UpdateItemContainers();
    }

    bool _pendingUpdate = false;

    private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (_pendingUpdate == false)
        {
            _pendingUpdate = true;
            // The underlying collection can have items added / removed in a foreach,
            // and this buffers that down to 1 call to UpdateItemContainers.
            Dispatcher.Dispatch(() =>
            {
                UpdateItemContainers();
                _pendingUpdate = false;
            }
            );
        }
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



    public static readonly BindableProperty ScrollVelocityProperty = BindableProperty.Create(nameof(ScrollVelocity), typeof(double), typeof(ListViewZero), (double)0.0, BindingMode.OneWay, null, null, ScrollVelocityChanged);

    public double ScrollVelocity
    {
        get { return (double)GetValue(ScrollVelocityProperty); }
        set { SetValue(ScrollVelocityProperty, value); }
    }

    private static void ScrollVelocityChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ListViewZero)bindable;

    }





    public static readonly BindableProperty ItemHeightProperty = BindableProperty.Create(nameof(ItemHeight), typeof(float), typeof(ListViewZero), (float)40.0, BindingMode.OneWay, null);

    public float ItemHeight
    {
        get { return (float)GetValue(ItemHeightProperty); }
        set { SetValue(ItemHeightProperty, value); }
    }

    public ListViewZero()
    {
        _cache = new();
        _sw = new();

        InitializeComponent();

        canvas.SizeChanged += Canvas_SizeChanged;

        var pgr = new PanGestureRecognizer();
        pgr.PanUpdated += Gr_PanUpdated;
        this.GestureRecognizers.Add(pgr);

        var tgr = new TapGestureRecognizer();
        tgr.Tapped += (s, e) => this.AbortAnimation("SimpleAnimation");
        this.GestureRecognizers.Add(tgr);

    }

    private void Canvas_SizeChanged(object sender, EventArgs e)
    {
        UpdateItemContainers();
    }
    Stopwatch _sw;

    double _totalY = 0;

    private void Gr_PanUpdated(object sender, PanUpdatedEventArgs e)
    {
        // TODO: Introduce inertia.
        switch (e.StatusType)
        {
            case GestureStatus.Started:
            this.AbortAnimation("SimpleAnimation");
                _anchor = ScrollOffset;
                _sw.Start();
                break;
            case GestureStatus.Running:
                ScrollOffset = _anchor - (float)(e.TotalY);
                _totalY = e.TotalY;
                break;
            case GestureStatus.Completed:

                double timeDelta = _sw.Elapsed.TotalMilliseconds;
                _sw.Reset();
                double motionDelta = _totalY;

                uint millisecondRate = 16;

                _animationDelta = -(motionDelta / timeDelta) * millisecondRate;


                var animation = new Animation(PanAnimate, 1, 0);

                animation.Commit(this, "SimpleAnimation", millisecondRate, 2000, Easing.Linear, (v, c) => { }, () => false);

                break;
            case GestureStatus.Canceled:
                ScrollOffset = _anchor;
                break;
        }
    }
    double _animationDelta;
    private void PanAnimate(double elapsed)
    {
        this.ScrollOffset += (float)(_animationDelta * elapsed);

        if (ScrollOffset < 0)
        {
            this.AbortAnimation("SimpleAnimation");
            ScrollOffset = 0;
            UpdateItemContainers();
            PanBounce();


        }
    }

    private void PanBounce()
    {
        double direction = 1;
        uint duration = 250;

        foreach (object obj in canvas)
        {
            if (obj is ListItemZero item)
            {
                direction = -direction;

                //item.TranslateTo(item.TranslationY * direction * _animationDelta / 120, item.TranslationY *= 1.15, duration, Easing.CubicOut);
                item.RotateTo(direction * 10, duration, Easing.SpringOut);
                //item.RotateXTo(direction * item.TranslationY / item.Height, duration, Easing.CubicOut);

                //item.TranslationY *= 1.02;
                //item.Rotation = direction * item.TranslationY / item.Height * 5;
            }
        }
        Task.Delay((int)duration).ContinueWith(async (a) =>
        {
            foreach (object obj in canvas)
            {
                if (obj is ListItemZero item)
                {
                    //item.TranslateTo(0, item.ItemIndex * ItemHeight - ScrollOffset, duration, Easing.BounceOut);
                    item.RotateTo(0, duration, Easing.BounceOut);
                    //item.RotateXTo(0, 1000, Easing.BounceOut);
                }
            }

        });
    }
    private void UpdateItemContainers()
    {

        if (canvas.Height <= 0)
            return;

        if (ItemsSource == null)
            return;
        // Generate item-containers ...

        // Step 1 -> Determine how many items are coming into view and create them.
        // Step 2 -> Update the Y offset of every container.
        // Step 3 -> Update the BindingContext of every container.
        //           Because items may be inserted or removed from the underlying list.
        //           Note setting the BC to the same value is fast because there will be no change event.
        // Step 4 -> Remove containers that are no longer visible and give them to the cache.

        // Find the first item that is to be in view
        int firstVisibleIndex = Math.Max(0, (int)(ScrollOffset / ItemHeight));

        // Find the last item that is be in view
        //int lastVisibleIndex = (int)(ScrollOffset / ItemHeight + (canvas.Height + ItemHeight - 1) / ItemHeight) - 1;

        // Maximum number of ListItem instances that can be at least partially seen.
        int maxVisibleContainers = (int)(canvas.Height / ItemHeight) + 1;


        int lastVisibleIndex = Math.Min(ItemsSource.Count - 1, firstVisibleIndex + maxVisibleContainers);

        for (int c = firstVisibleIndex; c <= lastVisibleIndex; c++)
        {
            if ((c < _firstVisibleItemIndex) || (c > _lastVisibleItemIndex))
            {
                ListItemZero itemContainer = GetView(c);
                //itemContainer.BindingContext = null;
                itemContainer.BindingContext = ItemsSource[c];

                canvas.Add(itemContainer);
                //var animation = new Animation(v => itemContainer.TranslationX = v, -100, 0);
                //animation.Commit(this, c.ToString(), 16, 200, Easing.Linear, (v, c) => itemContainer.TranslationX = 0, () => false);
            }
        }
        _firstVisibleItemIndex = int.MaxValue;
        _lastVisibleItemIndex = -1;

        var killList = new List<View>();

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
                    item.BindingContext = ItemsSource[item.ItemIndex];

                    item.TranslationY = itemOffset;

                    _firstVisibleItemIndex = Math.Min(_firstVisibleItemIndex, item.ItemIndex);
                    _lastVisibleItemIndex = Math.Max(_lastVisibleItemIndex, item.ItemIndex);
                }
            }
        }

        foreach (ListItemZero item in killList)
        {
            //item.BindingContext = null;
            canvas.Remove(item);
            item.ClearValue(ListItemZero.BindingContextProperty);
            _cache.PushToBucket(item.ItemTemplate, item);
        }

        TestLabel.Text = $"Active: {canvas.Count}";
    }

    //private void AddToCache(DataTemplate template, ListItemZero item)
    //{
    //    //GC.Collect();
    //    //GC.WaitForPendingFinalizers();
    //    //GC.Collect();


    //    if (_cache.TryGetValue(template, out var stack))
    //    {
    //        stack.Push(item);
    //    }
    //    else
    //    {
    //        var newStack = new Stack<ListItemZero>();
    //        newStack.Push(item);
    //        _cache.Add(template, newStack);
    //    }
    //}

    private ListItemZero GetView(int itemIndex)
    {
        object item = ItemsSource[itemIndex];
        ListItemZero retVal = null;

        DataTemplate template;

        if (ItemTemplate is DataTemplateSelector selector)
            template = selector.SelectTemplate(item, this);
        else
            template = ItemTemplate;

        //if (_cache.TryGetValue(template, out var typeStack))
        //{
        //    if (typeStack.TryPop(out var view))
        //        retVal = view;
        //}

        if (_cache.TryPopFromBucket(template, out var cachedThing, ItemsSource[itemIndex]))
        {
            retVal = cachedThing;
        }

        if (retVal == null)
        {
            retVal = new ListItemZero();

            retVal.ItemTemplate = template;
            retVal.Content = (View)template.CreateContent();
            //retVal.BindingContext = null;       // Stop it inheriting an unsuitable value.

        }

        retVal.HeightRequest = ItemHeight;
        retVal.WidthRequest = 200;
        retVal.ItemIndex = itemIndex;
        //retVal.BindingContext = item;

        return retVal;
    }
}
