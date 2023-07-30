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
        private Color _maskColor;
        private Color _maskEdgeColor;
        private float _backgroundAlpha;

        public string TargetName
        {
            get => _targetName;
            set => SetProperty(ref _targetName, value);
        }
        public float BackgroundAlpha
        {
            get => _backgroundAlpha;
            set => SetProperty(ref _backgroundAlpha, value);
        }

        public CircleMaskPageVm()
        {
            _ = DoTheThingAsync();
        }

        public Color MaskColor
        {
            get => _maskColor;
            set => SetProperty(ref _maskColor, value);
        }
        public Color MaskEdgeColor
        {
            get => _maskEdgeColor;
            set => SetProperty(ref _maskEdgeColor, value);
        }

        private async Task DoTheThingAsync()
        {
            while (true)
            {
                await Task.Delay(2000);
                BackgroundAlpha = 0.5F;

                TargetName = "banana";
                MaskColor = Colors.Red;
                MaskEdgeColor = Colors.Black;
                await Task.Delay(2000);

                TargetName = "radish";
                MaskColor = Colors.Purple;
                MaskEdgeColor = Colors.Black;
                await Task.Delay(2000);

                TargetName = "melon";
                MaskColor = Colors.Blue;
                MaskEdgeColor = Colors.Red;
                await Task.Delay(2000);

                BackgroundAlpha = 0.8F;
                await Task.Delay(1000);

                BackgroundAlpha = 1F;
                await Task.Delay(1000);

              
                BackgroundAlpha = 0.5F;
                await Task.Delay(1000);

                TargetName = "grapefruit";
                MaskColor = Colors.Yellow;
                MaskEdgeColor = Colors.Black;
                await Task.Delay(2000);

                TargetName = "";
            }
        }
    }
}
