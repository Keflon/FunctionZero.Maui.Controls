using FunctionZero.Maui.Controls;
using Microsoft.Maui.Platform;
using System.Diagnostics;

namespace SampleApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}