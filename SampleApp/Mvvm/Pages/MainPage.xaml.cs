using SampleApp.Mvvm.PageViewModels;
using System;

namespace SampleApp.Mvvm.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageVm();
        }
    }
}