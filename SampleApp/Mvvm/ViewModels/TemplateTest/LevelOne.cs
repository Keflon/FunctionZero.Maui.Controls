using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SampleApp.Mvvm.ViewModels.TemplateTest
{
    public class LevelOne : BaseVm
    {
        public ObservableCollection<LevelTwo> LevelOneChildren { get; }

        public string Name { get; }

        private bool _isLevelOneExpanded;

        public bool IsLevelOneExpanded
        {
            get { return _isLevelOneExpanded; }
            set
            {
                if (_isLevelOneExpanded != value)
                {
                    _isLevelOneExpanded = value;
                    Debug.WriteLine($"Node {Name} IsLevelOneExpanded changed to {_isLevelOneExpanded}");
                    OnPropertyChanged();
                }
            }
        }

        public LevelOne(string name)
        {
            Name = name;

            LevelOneChildren = new ObservableCollection<LevelTwo>();

            LevelOneChildren.Add(new LevelTwo("1-1"));
            LevelOneChildren.Add(new LevelTwo("1-2") { IsLevelTwoExpanded = true });
            LevelOneChildren.Add(new LevelTwo("1-3"));
        }
    }
}
