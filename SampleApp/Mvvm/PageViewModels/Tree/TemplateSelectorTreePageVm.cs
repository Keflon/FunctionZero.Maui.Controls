using SampleApp.Mvvm.ViewModels;
using SampleApp.Mvvm.ViewModels.TemplateTest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Mvvm.PageViewModels.Tree
{
    public class TemplateSelectorTreePageVm : BasePageVm
    {
        public LevelZero RootNode { get; }
        /*
            IsRootVisible="{Binding IsRootVisible}" 
            ItemsSource="{Binding RootNode}" 
            SelectedItem="{Binding SelectedItem}"
            SelectedItems="{Binding SelectedItems}"
        */
        private bool _isRootVisible;
        public bool IsRootVisible { get => _isRootVisible; set => SetProperty(ref _isRootVisible, value); }

        private object _selectedItem;
        public object SelectedItem { get => _selectedItem; set => SetProperty(ref _selectedItem, value); }

        public ObservableCollection<object> SelectedItems { get; }


        public class PickerStuff
        {
            public PickerStuff(string name, SelectionMode mode)
            {
                Name = name;
                Mode = mode;
            }

            public string Name { get; }
            public SelectionMode Mode { get; }
        }

        public List<PickerStuff> PickerData { get; }

        public TemplateSelectorTreePageVm()
        {
            RootNode = new LevelZero("Root") { IsLevelZeroExpanded = true };

            PickerData = new()
            {
                new PickerStuff("None", SelectionMode.None),
                new PickerStuff("Single", SelectionMode.Single),
                new PickerStuff("Multiple", SelectionMode.Multiple),
            };

            SelectedItems = new();

        }


    }
}
