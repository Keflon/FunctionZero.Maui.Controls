using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Common;
using System.Windows.Input;

namespace FunctionZero.Maui.Controls;

public partial class GridViewZero : ContentView
{
    public GridViewZero()
    {
        InitializeComponent();

        GridColumns.CollectionChanged += GridColumns_CollectionChanged;
    }

    private void GridColumns_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                {
                    var column = (GridColumnZero)e.NewItems[0];
                    theGrid.ColumnDefinitions.Add(new ColumnDefinition(column.Width));

                    AddListView(column);
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                break;
            case NotifyCollectionChangedAction.Replace:
                break;
            case NotifyCollectionChangedAction.Move:
                break;
            case NotifyCollectionChangedAction.Reset:
                break;
        }

    }
    #region Proxy properties

    #region ItemsSourceProperty

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(GridViewZero), null, BindingMode.OneWay, null, test);

    private static void test(BindableObject bindable, object oldValue, object newValue)
    {

    }

    public IList ItemsSource
    {
        get { return (IList)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }

    #endregion

    #region RemainingItemsProperty

    public static readonly BindableProperty RemainingItemsProperty = BindableProperty.Create(nameof(RemainingItems), typeof(int), typeof(GridViewZero), -1, BindingMode.OneWay);

    public int RemainingItems
    {
        get { return (int)GetValue(RemainingItemsProperty); }
        set { SetValue(RemainingItemsProperty, value); }
    }

    #endregion

    #region RemainingItemsChangedCommand

    public static readonly BindableProperty RemainingItemsChangedCommandProperty = BindableProperty.Create(nameof(RemainingItemsChangedCommand), typeof(ICommand), typeof(GridViewZero), null, BindingMode.OneWay);

    public ICommand RemainingItemsChangedCommand
    {
        get { return (ICommand)GetValue(RemainingItemsChangedCommandProperty); }
        set { SetValue(RemainingItemsChangedCommandProperty, value); }
    }

    #endregion

    #region SelectedItemsProperty

    public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(nameof(SelectedItems), typeof(IList), typeof(GridViewZero), null, BindingMode.TwoWay);

    public IList SelectedItems
    {
        get { return (IList)GetValue(SelectedItemsProperty); }
        set { SetValue(SelectedItemsProperty, value); }
    }

    #endregion

    #region SelectionModeProperty

    public static readonly BindableProperty SelectionModeProperty = BindableProperty.Create(nameof(SelectionMode), typeof(SelectionMode), typeof(GridViewZero), SelectionMode.None, BindingMode.OneWay);

    public SelectionMode SelectionMode
    {
        get { return (SelectionMode)GetValue(SelectionModeProperty); }
        set { SetValue(SelectionModeProperty, value); }
    }

    #endregion

    #region SelectedItemProperty

    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(GridViewZero), null, BindingMode.TwoWay);

    public object SelectedItem
    {
        get { return (object)GetValue(SelectedItemProperty); }
        set { SetValue(SelectedItemProperty, value); }
    }

    #endregion

    #region ScrollOffsetProperty

    public static readonly BindableProperty ScrollOffsetProperty = BindableProperty.Create(nameof(ScrollOffset), typeof(double), typeof(GridViewZero), (double)0.0, BindingMode.TwoWay);
    public double ScrollOffset
    {
        get { return (double)GetValue(ScrollOffsetProperty); }
        set { SetValue(ScrollOffsetProperty, value); }
    }

    #endregion

    #region ItemHeightProperty

    public static readonly BindableProperty ItemHeightProperty = BindableProperty.Create(nameof(ItemHeight), typeof(double), typeof(GridViewZero), (double)40.0, BindingMode.OneWay);

    public double ItemHeight
    {
        get { return (double)GetValue(ItemHeightProperty); }
        set { SetValue(ItemHeightProperty, value); }
    }

    #endregion

    #endregion

    #region Specialised properties

    #region GridColumnsProperty

    public static readonly BindableProperty GridColumnsProperty = BindableProperty.Create(nameof(GridColumns), typeof(ObservableCollection<GridColumnZero>), typeof(GridViewZero), new ObservableCollection<GridColumnZero>(), BindingMode.OneWay, null, GridColumnsPropertyChanged);

    public ObservableCollection<GridColumnZero> GridColumns
    {
        get { return (ObservableCollection<GridColumnZero>)GetValue(GridColumnsProperty); }
        set { SetValue(GridColumnsProperty, value); }
    }

    private static void GridColumnsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (GridViewZero)bindable;

        // TODO: For each Grid column, make a ListViewZero.

        //self.theGrid.ColumnDefinitions.Clear();
        //self.theGrid.Children.Clear();

        //int columnIndex = 0;

        //if (self.GridColumns != null)
        //    foreach (var column in self.GridColumns)
        //    {
        //        self.theGrid.ColumnDefinitions.Add(new ColumnDefinition(column.Width));

        //        self.AddListView(columnIndex);
        //        columnIndex++;
        //        // TODO: Use an ObsColl and hook CollectionChanged.
        //    }

    }

    private void AddListView(GridColumnZero gcolumn)
    {
        var listView = new ListViewZero();

        Grid.SetColumn(listView, theGrid.ColumnDefinitions.Count - 1);
        listView.BindingContext = this;
        listView.SetBinding(ListViewZero.ItemsSourceProperty, nameof(GridViewZero.ItemsSource));
        //listView.SetBinding(ListViewZero.SelectedItemProperty, nameof(GridViewZero.SelectedItem));
        //listView.SetBinding(ListViewZero.RemainingItemsChangedCommandProperty, nameof(GridViewZero.RemainingItemsChangedCommand));
        //listView.SetBinding(ListViewZero.SelectedItemsProperty, nameof(GridViewZero.SelectedItems));
        //listView.SetBinding(ListViewZero.SelectionModeProperty, nameof(GridViewZero.SelectionMode));

        //listView.SetBinding(ListViewZero.ItemTemplateProperty, nameof(GridViewZero.SelectionMode));
        listView.ItemTemplate = gcolumn.ItemTemplate;

        //if (columnIndex == 0)
        {
            listView.SetBinding(ListViewZero.RemainingItemsProperty, nameof(GridViewZero.RemainingItems));
            listView.SetBinding(ListViewZero.ScrollOffsetProperty, nameof(GridViewZero.ScrollOffset));
            listView.SetBinding(ListViewZero.ItemHeightProperty, nameof(GridViewZero.ItemHeight));
        }

        this.theGrid.Children.Add(listView);

    }
    #endregion

    #endregion
}
