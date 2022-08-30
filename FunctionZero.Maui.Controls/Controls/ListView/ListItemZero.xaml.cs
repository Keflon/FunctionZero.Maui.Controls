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

	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();

		if (BindingContext != null)
		{
			string s = BindingContext.GetType().ToString();
			if (BindingContext.GetType().ToString().EndsWith("ListItem") == false)
			{
				Debug.WriteLine("false");
			}
		}
	}

	protected override void OnParentChanged()
	{
		base.OnParentChanged();
	}
}