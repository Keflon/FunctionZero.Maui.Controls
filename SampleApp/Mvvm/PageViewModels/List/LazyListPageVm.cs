using SampleApp.Mvvm.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SampleApp.Mvvm.PageViewModels.List
{
    public class LazyListPageVm : BasePageVm
    {
        public IList SampleLazyListData { get; }
        public ICommand RemainingItemsChangedCommand { get; }
        public LazyListPageVm()
        {
                    SampleLazyListData = new ObservableCollection<ListItem>();
            for (int c = 0; c< 14; c++)
                SampleLazyListData.Add(new ListItem($"Hello {c}", (double)110.0 + (double) Math.Sin(c / 9.0) * 40));

            RemainingItemsChangedCommand = new Command(RemainingItemsChangedCommandExecute);
        }
        int _addingMoreCount = 0;
        private async void RemainingItemsChangedCommandExecute(object obj)
        {
            var remainingItems = (int)obj;
            Debug.WriteLine($"Remaining Items: {remainingItems}");

            if (remainingItems < 10)
            {
                int numToRequest = (10 - remainingItems) - _addingMoreCount;

                if (numToRequest > 0)
                {
                    bool startAdding = false;

                    if (_addingMoreCount == 0)
                        startAdding = true;

                    _addingMoreCount += numToRequest;

                    if (startAdding)
                    {
                        while (_addingMoreCount > 0)
                        {
                            await Task.Delay(200);
                            SampleLazyListData.Add(new ListItem($"Hello {SampleLazyListData.Count}", (double)110.0 + (double)Math.Sin(SampleLazyListData.Count / 9.0) * 40));
                            _addingMoreCount--;
                        }
                    }
                }
            }
        }
    }
}
