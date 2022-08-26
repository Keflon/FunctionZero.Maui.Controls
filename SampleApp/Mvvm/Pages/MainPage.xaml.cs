using SampleApp.Mvvm.PageViewModels;
using System;
using System.Diagnostics;

namespace SampleApp.Mvvm.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageVm();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            TestLabel.Text=((Button)sender).Text;
        }
    }
}