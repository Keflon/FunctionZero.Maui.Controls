using SampleApp.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Mvvm.PageViewModels.MultiView
{
    public class MultiViewModalPageVm : BasePageVm
    {
        private string[] _modalViewNames = { "first", "second", "third", "fourth" };
        private int _modalViewIndex = 0;
        private string _topViewName;

        public string TopViewName { get => _topViewName; set => SetProperty(ref _topViewName, value); }

        public MultiViewModalPageVm()
        {
            this.AddPageTimer(1500, Tick, null, null);
        }

        private void Tick(object obj)
        {
            _modalViewIndex = (_modalViewIndex + 1) % _modalViewNames.Length;
            TopViewName = _modalViewNames[_modalViewIndex];
            Debug.WriteLine(_modalViewIndex);
        }
    }
}
