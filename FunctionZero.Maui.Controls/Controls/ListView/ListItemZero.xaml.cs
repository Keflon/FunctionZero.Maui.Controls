using System.Diagnostics;

namespace FunctionZero.Maui.Controls;

public partial class ListItemZero : ContentView
{
	public ListItemZero()
	{
		InitializeComponent();

#if !ANDROID
        var tgr = new TapGestureRecognizer();
        tgr.Tapped += Tgr_Tapped;
        this.GestureRecognizers.Add(tgr);
#endif
    }

#if !ANDROID
    private void Tgr_Tapped(object sender, EventArgs e)
	{
		IsSelected = !IsSelected;
        Debug.WriteLine($"IsSelected:{IsSelected}");
	}
#endif

    public int ItemIndex { get; set; }
	public DataTemplate ItemTemplate { get; set; }

    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(ListItemZero), false, BindingMode.TwoWay, null, IsSelectedChanged);

    private static async void IsSelectedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (ListItemZero)bindable;
        
        Debug.WriteLine($"IsSelected:{self.IsSelected}");

        self.RotateTo(360).ContinueWith(async (t) => self.RotateTo(0));

    }

    public bool IsSelected
    {
        get { return (bool)GetValue(IsSelectedProperty); }
        set { SetValue(IsSelectedProperty, value); }
    }
}