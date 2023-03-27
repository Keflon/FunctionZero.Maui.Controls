using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace FunctionZero.Maui.Controls;

public partial class ListItemZero : ContentView
{
    public ListItemZero()
    {
        InitializeComponent();
        
        var tgr = new TapGestureRecognizer();
        tgr.Tapped += Tgr_Tapped;
        
        
        this.GestureRecognizers.Add(tgr);
    }

    private void Tgr_Tapped(object sender, TappedEventArgs e)
    {
        IsSelected = !IsSelected;
        Debug.WriteLine($"IsSelected:{IsSelected}");
    }

    public int ItemIndex { get; set; }
    public DataTemplate ItemTemplate { get; set; }

    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(ListItemZero), false, BindingMode.OneWay, null, IsSelectedChanged);

    public bool IsSelected
    {
        get { return (bool)GetValue(IsSelectedProperty); }
        set { SetValue(IsSelectedProperty, value); }
    }

    private static async void IsSelectedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ListItemZero)bindable;

        Debug.WriteLine($"IsSelected:{self.IsSelected}");

        self.DeferredUpdateVisualState();
    }

    public static readonly BindableProperty IsPrimaryProperty = BindableProperty.Create(nameof(IsPrimary), typeof(bool), typeof(ListItemZero), false, BindingMode.OneWay, null, IsPrimaryChanged);
    private bool _pendingUpdate;

    public bool IsPrimary
    {
        get { return (bool)GetValue(IsPrimaryProperty); }
        set { SetValue(IsPrimaryProperty, value); }
    }

    private static void IsPrimaryChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ListItemZero)bindable;

        Debug.WriteLine($"IsSelected:{self.IsSelected}");

        self.DeferredUpdateVisualState();

    }

    private void DeferredUpdateVisualState()
    {
        if (_pendingUpdate == false)
        {
            _pendingUpdate = true;
            // This is called when either IsSelected changes or IsPrimary changes,
            // and if both change, this buffers that down to 1 call to UpdateVisualState.
            Dispatcher.Dispatch(() =>
            {
                UpdateVisualState();
                _pendingUpdate = false;
            }
            );
        }
    }

    private void UpdateVisualState()
    {
        if (IsPrimary)
            VisualStateManager.GoToState(this, "ItemFocused");
        else if (IsSelected)
            VisualStateManager.GoToState(this, "Selected");
        else
            VisualStateManager.GoToState(this, "Normal");
    }
}
