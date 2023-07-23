using SampleApp.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Mvvm.PageViewModels.Mask
{
    public class CircleMaskPageVm : BasePageVm
    {
        private string _targetName;

        public string TargetName
        {
            get => _targetName;
            set => SetProperty(ref _targetName, value);
        }

        public CircleMaskPageVm()
        {
            _ = DoTheThingAsync();
        }

        private async Task DoTheThingAsync()
        {
            while (true)
            {
                await Task.Delay(2000);
                TargetName = "banana";
                await Task.Delay(2000);
                TargetName = "radish";
                await Task.Delay(2000);
                TargetName = "melon";
                await Task.Delay(2000);
                TargetName = "grapefruit";
            }
        }
    }
}
