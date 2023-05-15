using SampleApp.Mvvm.ViewModels;
using SampleApp.Mvvm.ViewModels.TemplateTest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SampleApp.Mvvm.PageViewModels
{
    public class MainPageVm : BaseVm
    {
        private TestNode _spareNode;
        private LevelTwo _spareTemplateNode;

        private bool _treeDance;
        public bool TreeDance
        {
            get => _treeDance;
            set => SetProperty(ref _treeDance, value);
        }

        private bool _listDance;
        public bool ListDance
        {
            get => _listDance;
            set => SetProperty(ref _listDance, value);
        }

        private ListItem _selectedItem;
        public ListItem SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        private ObservableCollection<ListItem> _selectedItems;
        public ObservableCollection<ListItem> SelectedItems
        {
            get => _selectedItems;
            set => SetProperty(ref _selectedItems, value);
        }

        private double _listViewScrollOffset;
        public double ListViewScrollOffset
        {
            get => _listViewScrollOffset;
            set => SetProperty(ref _listViewScrollOffset, value);
        }

        private double _treeViewScrollOffset;
        public double TreeViewScrollOffset
        {
            get => _treeViewScrollOffset;
            set => SetProperty(ref _treeViewScrollOffset, value);
        }

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

        public class ListItem : INotifyPropertyChanged
        {
            private string _name;
            private double _offset;

            public ListItem(string name, double offset)
            {
                Name = name;
                Offset = offset;
            }

            public string Name
            {
                get => _name;
                set
                {
                    if (_name != value)
                    {
                        _name = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                    }
                }
            }
            public double Offset
            {
                get => _offset;
                set
                {
                    if (_offset != value)
                    {
                        _offset = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Offset)));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public ICommand RemainingItemsChangedCommand { get; }
        public MainPageVm()
        {
            SampleData = GetSampleTree();

            SampleListData = new ObservableCollection<ListItem>();

            //for (int c = 0; c < 52440; c++)
            for (int c = 0; c < 400; c++)
                SampleListData.Add(new ListItem($"Hello {c}", (double)110.0 + (double)Math.Sin(c / 9.0) * 40));

            SelectedItems = new ObservableCollection<ListItem>();
            //SelectedItems.CollectionChanged += (sender, e) => { Debug.WriteLine($"VM Count:{SelectedItems.Count}"); };

            SampleLazyListData = new ObservableCollection<ListItem>();
            for (int c = 0; c < 14; c++)
                SampleLazyListData.Add(new ListItem($"Hello {c}", (double)110.0 + (double)Math.Sin(c / 9.0) * 40));

            RemainingItemsChangedCommand = new Command(RemainingItemsChangedCommandExecute);

            PickerData = new()
            {
                new PickerStuff("None", SelectionMode.None),
                new PickerStuff("Single", SelectionMode.Single),
                new PickerStuff("Multiple", SelectionMode.Multiple),
            };

            SampleTemplateTestData = new LevelZero("Root") { IsLevelZeroExpanded = true };

            Device.StartTimer(TimeSpan.FromMilliseconds(77), Tick);
            //Device.StartTimer(TimeSpan.FromMilliseconds(15), Tick);

            Device.StartTimer(TimeSpan.FromMilliseconds(20), Tick2);


            Task.Delay(1500).ContinueWith((d) => SelectedItem = (ListItem)SampleListData[4]);
            Task.Delay(2000).ContinueWith((d) => SelectedItem = (ListItem)SampleListData[5]);
            Task.Delay(2500).ContinueWith((d) => SelectedItem = (ListItem)SampleListData[6]);

        }

        bool _addingMore = false;
        private async void RemainingItemsChangedCommandExecute(object obj)
        {
            var remainingItems = (int)obj;
            var extraItemCount = 5 - remainingItems;

            Debug.WriteLine($"Remaining Items: {remainingItems}");

            if (_addingMore == false)
            {
                _addingMore = true;
                if (extraItemCount > 0)
                {
                    int startIndex = SampleLazyListData.Count;
                    for (int c= startIndex; c< startIndex+extraItemCount;c++)
                        //await Task.Delay(200);
                        SampleLazyListData.Add(new ListItem($"Hello {c}", (double)110.0 + (double)Math.Sin(c / 9.0) * 40));
                }
                _addingMore = false;
            }

        }

        private int _listCount;
        private bool Tick2()
        {
            if (ListDance == false)
                return true;
#if false


            if (_listCount % 2 == 0)
                for (int c = 1; c < 6; c++)
                    SampleListData.RemoveAt(0);
            else
                for (int c = 1; c < 6; c++)
                    SampleListData.Insert(c, new ListItem($"BORG {_listCount}", (float)110.0 + (float)Math.Sin(_listCount / 9.0) * 40));

            ((ListItem)SampleListData[0]).Name = _listCount.ToString();
            ((ListItem)SampleListData[0]).Offset = (float)110.0 + (float)Math.Sin(_listCount / 9.0) * 40;


#elif true

            //var scale = (Math.Sin(_listCount / 223.0 * Math.Cos(_listCount / 337.0))) / 2.0 + 1.0;
            var scale = Math.Sin(_listCount / 223.0) / 2.0 + 1.0;
            ListViewScrollOffset = (double)scale * SampleListData.Count * 25;

#elif false

            //if ((_listCount % 16) == 0)
                for (int c = 0; c < 8; c++)
                {
                    //if (((_listCount>>4) & (1 << c)) != 0)
                    if (((_listCount) & (1 << c)) != 0)
                    {
                        if (SelectedItems.Contains(SampleListData[c + 10]) == false)
                            SelectedItems.Add((ListItem)SampleListData[c + 10]);
                    }
                    else
                    {
                        if (SelectedItems.Contains(SampleListData[c + 10]) == true)
                            SelectedItems.Remove((ListItem)SampleListData[c + 10]);
                    }
                }
#endif
            _listCount++;
            return true;
        }

        private int _count;
        private bool _isRootVisible;

        public int Count { get => _count; set => SetProperty(ref _count, value); }
        public bool IsRootVisible { get => _isRootVisible; set => SetProperty(ref _isRootVisible, value); }

        private bool Tick()
        {
            if (TreeDance == false)
                return true;

#if false
            //var scale = (Math.Sin(_listCount / 223.0 * Math.Cos(_listCount / 337.0))) / 2.0 + 1.0;
            var scale = Math.Sin(Count / 2.0) / 2.0;
            TreeViewScrollOffset = (float)scale * 100;
#else

            //IsRootVisible = (Count & 8)==0;
            if ((Count % 8) < 2)
            {
                ((TestNode)SampleData).IsDataExpanded = !((TestNode)SampleData).IsDataExpanded;
                SampleTemplateTestData.IsLevelZeroExpanded = !SampleTemplateTestData.IsLevelZeroExpanded;
            }

            if ((Count % 3) != 0)
            {
                ((TestNode)SampleData).Children[1].IsDataExpanded = !((TestNode)SampleData).Children[1].IsDataExpanded;
                SampleTemplateTestData.LevelZeroChildren[1].IsLevelOneExpanded = !SampleTemplateTestData.LevelZeroChildren[1].IsLevelOneExpanded;
            }
            else
            {
                //var node = ((TestNode)SampleData).Children[1];

                //if (node.Children.Count == 3)
                //{
                //    _spareNode = node.Children[1];
                //    node.Children.RemoveAt(1);
                //}
                //else
                //{
                //    node.Children.Insert(1, _spareNode);
                //}
            }

            {
                //var node = SampleTemplateTestData.LevelZeroChildren[1];

                //if (node.LevelOneChildren.Count == 3)
                //{
                //    _spareTemplateNode = node.LevelOneChildren[1];
                //    node.LevelOneChildren.RemoveAt(1);
                //}
                //else
                //{
                //    node.LevelOneChildren.Insert(1, _spareTemplateNode);
                //}
            }
#endif
            Count++;

            return true;
        }

        public object SampleData { get; }
        public IList SampleListData { get; }
        public LevelZero SampleTemplateTestData { get; }

        public IList SampleLazyListData { get; }

        private object GetSampleTree()
        {
            var root = new TestNode("Root");
            var child0 = new TestNode("0");
            var child1 = new TestNode("1");
            var child2 = new TestNode("2");

            root.IsDataExpanded = true;

            root.Children.Add(child0);
            root.Children.Add(child1);
            root.Children.Add(child2);

            child1.IsDataExpanded = true;

            new TestNode("0-0").Parent = child0;
            new TestNode("0-1").Parent = child0;
            new TestNode("0-2").Parent = child0;

            new TestNode("1-0").Parent = child1;
            new TestNode("1-1").Parent = child1;
            new TestNode("1-2").Parent = child1;

            new TestNode("2-0").Parent = child2;
            new TestNode("2-1").Parent = child2;
            new TestNode("2-2").Parent = child2;

            return root;

            var retval = new List<object>();
            retval.Add(root);
            return retval;
        }


        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(SelectedItem))
            {
                Debug.WriteLine($"SelectedItem:{(SelectedItem?.Name ?? "null")}");
            }
        }
    }
}

