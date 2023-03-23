using FunctionZero.Maui.Controls;
using FunctionZero.Maui.Services.Cache;
using Microsoft.Maui.Controls;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace FunctionZero.Maui.Controls;

public partial class ListViewZero : ContentView
{
    private BucketDictionary<DataTemplate, ListItemZero> _cache;
    private readonly List<ListItemZero> _killList;
    bool _pendingUpdate = false;
    bool _updatingContainers = false;

    #region bindable properties

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
            oldCollection.CollectionChanged -= self.ItemsSource_CollectionChanged;

        if (newValue is INotifyCollectionChanged newCollection)
            newCollection.CollectionChanged += self.ItemsSource_CollectionChanged;

        self.UpdateScrollBarContentHeight();
        self.UpdateItemContainers();
    }

    public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(nameof(SelectedItems), typeof(IList), typeof(ListViewZero), null, BindingMode.TwoWay, null, SelectedItemsChanged);

    public IList SelectedItems
    {
        get { return (IList)GetValue(SelectedItemsProperty); }
        set { SetValue(SelectedItemsProperty, value); }
    }
    private static void SelectedItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ListViewZero)bindable;

        if (oldValue is INotifyCollectionChanged oldCollection)
            oldCollection.CollectionChanged -= self.SelectedItems_CollectionChanged;

        if (newValue is INotifyCollectionChanged newCollection)
            newCollection.CollectionChanged += self.SelectedItems_CollectionChanged;

        self.UpdateItemContainers();
    }

    public static readonly BindableProperty SelectionModeProperty = BindableProperty.Create(nameof(SelectionMode), typeof(SelectionMode), typeof(ListViewZero), SelectionMode.None, BindingMode.OneWay, null, SelectionModeChanged);

    public SelectionMode SelectionMode
    {
        get { return (SelectionMode)GetValue(SelectionModeProperty); }
        set { SetValue(SelectionModeProperty, value); }
    }

    private static void SelectionModeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ListViewZero)bindable;

        self.DeferredFilterAndUpdate();
    }

    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(ListViewZero), null, BindingMode.TwoWay, null, SelectedItemChanged);

    public object SelectedItem
    {
        get { return (object)GetValue(SelectedItemProperty); }
        set { SetValue(SelectedItemProperty, value); }
    }

    private static void SelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ListViewZero)bindable;

        if (self.SelectionMode == SelectionMode.None)
        {
            self.SelectedItem = null;
        }
        else
        {
            var newContainer = self.GetViewForBindingContextFromCanvas(newValue);

            if (newContainer is ListItemZero listItem)
                listItem.IsSelected = true;
        }
    }

    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(ListViewZero), null, BindingMode.OneWay);

    public DataTemplate ItemTemplate
    {
        get { return (DataTemplate)GetValue(ItemTemplateProperty); }
        set { SetValue(ItemTemplateProperty, value); }
    }

    public static readonly BindableProperty ScrollOffsetProperty = BindableProperty.Create(nameof(ScrollOffset), typeof(double), typeof(ListViewZero), (double)0.0, BindingMode.OneWay, null, ScrollOffsetChanged,null , CoerceScrollOffsetValue);
    // ATTENTION: TwoWay Binding a double to a ScrollOffset on a ScrollView can lose precision by varying amounts on different platforms, causing an event storm!
    // Ignoring small changes prevents the storm.
    private static object CoerceScrollOffsetValue(BindableObject bindable, object value)
    {
        var self = (ListViewZero)bindable;

        if (Math.Abs(self.ScrollOffset - (double)value) < 1.0)
            return self.ScrollOffset;
        return value;
    }

    public double ScrollOffset
    {
        get { return (double)GetValue(ScrollOffsetProperty); }
        set { SetValue(ScrollOffsetProperty, value); }
    }

    private static void ScrollOffsetChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ListViewZero)bindable;

        // TODO: Is the second one quicker on a slow Droid phone?
        //self.UpdateItemContainers();
        self.DeferredFilterAndUpdate();
    }

    public static readonly BindableProperty ScrollVelocityProperty = BindableProperty.Create(nameof(ScrollVelocity), typeof(double), typeof(ListViewZero), (double)0.0, BindingMode.OneWay, null, ScrollVelocityChanged);

    public double ScrollVelocity
    {
        get { return (double)GetValue(ScrollVelocityProperty); }
        set { SetValue(ScrollVelocityProperty, value); }
    }

    private static void ScrollVelocityChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ListViewZero)bindable;
    }

    public static readonly BindableProperty ItemHeightProperty = BindableProperty.Create(nameof(ItemHeight), typeof(double), typeof(ListViewZero), (double)40.0, BindingMode.OneWay);

    public double ItemHeight
    {
        get { return (double)GetValue(ItemHeightProperty); }
        set { SetValue(ItemHeightProperty, value); }
    }

    public static readonly BindableProperty ItemContainerStyleProperty = BindableProperty.Create(nameof(ItemContainerStyle), typeof(Style), typeof(ListViewZero), null, BindingMode.OneWay, null, ItemContainerStyleChanged);

    public Style ItemContainerStyle
    {
        get { return (Style)GetValue(ItemContainerStyleProperty); }
        set { SetValue(ItemContainerStyleProperty, value); }
    }

    private static void ItemContainerStyleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ListViewZero)bindable;
    }

    #endregion

    private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateScrollBarContentHeight();
        DeferredFilterAndUpdate();
    }

    private void DeferredFilterAndUpdate()
    {
        if (_pendingUpdate == false)
        {
            _pendingUpdate = true;
            // The underlying collection can have items added / removed in a foreach,
            // and this buffers that down to 1 call to UpdateItemContainers.
            Dispatcher.Dispatch(() =>
            {
                // TODO: This switch statement could be better placed.
                switch (SelectionMode)
                {
                    case SelectionMode.None:
                        SelectedItems.Clear();
                        break;
                    case SelectionMode.Single:
                        if (SelectedItems.Count > 1)
                        {
                            var temp = SelectedItems[SelectedItems.Count - 1];
                            SelectedItems.Clear();
                            SelectedItems.Add(temp);
                        }
                        break;
                    case SelectionMode.Multiple:
                        break;
                }

                if (SelectedItems.Contains(SelectedItem) == false)
                {
                    if (SelectedItems.Count > 0)
                        SelectedItem = SelectedItems[SelectedItems.Count - 1];
                    else
                        SelectedItem = null;
                }

                UpdateItemContainers();
                _pendingUpdate = false;
            }
            );
        }
    }

    private void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        DeferredFilterAndUpdate();
    }
    public ListViewZero()
    {
        _cache = new();
        _killList = new(50);

        InitializeComponent();

        canvas.SizeChanged += Canvas_SizeChanged;

        var tgr = new TapGestureRecognizer();
        tgr.Tapped += Tgr_Tapped;
        this.GestureRecognizers.Add(tgr);

        SelectedItems = new ObservableCollection<object>();
    }

    private void Tgr_Tapped(object sender, EventArgs e)
    {
        var tea = (TappedEventArgs)e;
        Point? point = tea.GetPosition(this);
        
        // Get ListViewItem for point
        if (point.HasValue)
        {
            var listItem = GetListItemUnderPoint(point.Value.Y);
            if (listItem != null)
            {
                listItem.IsSelected = !listItem.IsSelected;
            }
        }
    }

    public ListItemZero GetListItemUnderPoint(double yOffset)
    {
        foreach (View item in this.canvas)
            if (item is ListItemZero listItem)
                if ((yOffset >= item.TranslationY) && (yOffset < (item.TranslationY+ItemHeight)))
                    return listItem;
        return null;
    }

    private void Canvas_SizeChanged(object sender, EventArgs e)
    {
        UpdateScrollBarLayoutBounds();

        UpdateItemContainers();
    }

    private void UpdateScrollBarLayoutBounds()
    {
        //canvas.SetLayoutBounds(sb, new Rect(canvas.Width - 50, 0, 50, canvas.Height));
        canvas.SetLayoutBounds(sb, new Rect(0, 0, canvas.Width, canvas.Height));
    }
    private void UpdateScrollBarContentHeight()
    {
        sb.ContentHeight = ItemHeight * ItemsSource.Count;
    }

    private void UpdateItemContainers()
    {
        if (canvas.Height <= 0)
            return;

        if (ItemsSource == null)
            return;

        if (_updatingContainers == true)
        {
            Debug.WriteLine("Gotcha!");
            //return;
        }

        _updatingContainers = true;

        // Find the first item that is to be in view
        int firstVisibleIndex = Math.Max(0, (int)(ScrollOffset / ItemHeight));

        // Maximum number of ListItem instances that can be at least partially seen.
        int maxVisibleContainers = (int)(canvas.Height / ItemHeight) + 1;

        int lastVisibleIndex = Math.Min(ItemsSource.Count - 1, firstVisibleIndex + maxVisibleContainers);

        // Foreach over each ListItemZero in this.Canvas and set ItemIndex to -1.
        // Then, after layout, kill any that still have ItemIndex of -1.
        // Mark everything in the canvas as a candidate for removal.
        foreach (View item in this.canvas)
            if (item is ListItemZero listItem)
                listItem.ItemIndex = -1;

        // For each item that should be visible,
        // get an existing ListViewItem from the canvas
        // if there isn't one, get one from the cache
        // if there isn't one, create one and set it up.
        for (int c = firstVisibleIndex; c <= lastVisibleIndex; c++)
        {
            ListItemZero itemContainer = GetViewForBindingContextFromCanvas(ItemsSource[c]);
            if (itemContainer == null)
            {
                itemContainer = GetView(ItemsSource[c]);
                itemContainer.BindingContext = null;
                // To prevent the property-changed callback calling this method, 
                // set IsSelected *before* adding to the canvas.
                itemContainer.IsSelected = (SelectedItem == ItemsSource[c]) || (SelectedItems.Contains(ItemsSource[c]));

                canvas.Add(itemContainer);
                itemContainer.BindingContext = ItemsSource[c];
            }
            itemContainer.ItemIndex = c;
        }

        _killList.Clear();

        foreach (View item in this.canvas)
            if (item is ListItemZero listItem)
                if (listItem.ItemIndex == -1)
                    _killList.Add(listItem);

        foreach (ListItemZero item in _killList)
        {
            //item.BindingContext = null;
            canvas.Remove(item);
            item.ClearValue(ListItemZero.BindingContextProperty);
            item.IsSelected = false;
            _cache.PushToBucket(item.ItemTemplate, item);
        }

        foreach (object item in canvas)
        {
            if (item is ListItemZero listItem)
            {
                // Determine offset for item.
                double itemOffset = listItem.ItemIndex * ItemHeight - ScrollOffset;
                listItem.BindingContext = ItemsSource[listItem.ItemIndex];
                listItem.TranslationY = itemOffset;
                listItem.WidthRequest = this.Width;

                listItem.IsSelected = SelectedItems.Contains(listItem.BindingContext);

                listItem.IsPrimary = listItem.BindingContext == SelectedItem;
            }
        }
        _updatingContainers = false;
    }

    private ListItemZero GetViewForBindingContextFromCanvas(object bindingContext)
    {
        // TODO: Once working, use a map.
        foreach (View item in this.canvas)
            if (item.BindingContext == bindingContext)
                return (ListItemZero)item;

        return null;
    }

    private ListItemZero GetView(object bindingContext)
    {
        ListItemZero retVal = null;

        DataTemplate template;

        if (ItemTemplate is DataTemplateSelector selector)
            template = selector.SelectTemplate(bindingContext, null);
        else
            template = ItemTemplate;

        if (_cache.TryPopFromBucket(template, out var cachedThing, bindingContext))
        {
            retVal = cachedThing;
        }

        if (retVal == null)
        {
            retVal = new ListItemZero();

            retVal.ItemTemplate = template;
            retVal.Content = (View)template.CreateContent();

            if (ItemContainerStyle != null)
                retVal.Style = ItemContainerStyle;

            retVal.PropertyChanged += ListItemZero_PropertyChanged;

            //retVal.BindingContext = null;       // Stop it inheriting an unsuitable value.

        }

        retVal.HeightRequest = ItemHeight;
        //retVal.WidthRequest = 200;
        //retVal.ItemIndex = itemIndex;
        //retVal.BindingContext = item;

        return retVal;
    }

    private void ListItemZero_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ListItemZero.IsSelected))
        {
            if (_updatingContainers == false)
            {
                var listItem = (ListItemZero)sender;

                if (listItem.BindingContext != null)
                {
                    if (SelectionMode != SelectionMode.None)
                    {
                        if (listItem.IsSelected)
                        {
                            // Adding to SelectedItems will cause a deferred update.
                            // SelectedItem must be set prior to that call.
                            SelectedItem = listItem.BindingContext;
                            SelectedItems.Add(listItem.BindingContext);

                        }
                        else
                        {
                            SelectedItems.Remove(listItem.BindingContext);
                            if (SelectedItem == listItem.BindingContext)
                            {
                                if (SelectedItems?.Count > 0)
                                    SelectedItem = SelectedItems[SelectedItems.Count - 1];
                                else
                                    SelectedItem = null;
                            }

                        }
                        //UpdateItemContainers();
                    }
                    else
                        listItem.IsSelected = false;
                }
            }
        }
    }
}
