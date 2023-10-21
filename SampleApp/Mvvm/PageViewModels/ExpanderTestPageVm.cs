using Microsoft.Maui.Controls;
using SampleApp.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Mvvm.PageViewModels
{
    public class ExpanderTestPageVm : BasePageVm
    {
        public ObservableCollection<ExpanderItemVm> Children { get; }
        public ExpanderTestPageVm()
        {
            Children = new();

            var item1 = new ExpanderItemVm("Home", null);
            var item2 = new ExpanderItemVm("Test", null);
            var item3 = new ExpanderItemVm("Settings", null);

            item1.Children.Add(new ExpanderItemVm("Home Page 1", null));

            var testItem1 = new ExpanderItemVm("Test List 1", null);
            var testItem2 = new ExpanderItemVm("Test List 2", null);

            testItem1.Children.Add(new ExpanderItemVm("Item 1-1", null));
            testItem1.Children.Add(new ExpanderItemVm("Item 1-2", null));

            testItem2.Children.Add(new ExpanderItemVm("Item 2-1", null));
            testItem2.Children.Add(new ExpanderItemVm("Item 2-2", null));

            item2.Children.Add(testItem1);
            item2.Children.Add(testItem2);

            item3.Children.Add(new ExpanderItemVm("Settings Page 1", null));

            Children.Add(item1);
            Children.Add(item2);
            Children.Add(item3);


        }
    }
}
