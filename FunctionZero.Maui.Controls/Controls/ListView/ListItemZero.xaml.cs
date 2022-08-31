using System.Diagnostics;

namespace FunctionZero.Maui.Controls;

public partial class ListItemZero : ContentView
{
	public ListItemZero()
	{
		InitializeComponent();
	}

	public int ItemIndex { get; set; }
	public DataTemplate ItemTemplate { get; set; }

	//public object ItemBindingContext { get; set; }
}