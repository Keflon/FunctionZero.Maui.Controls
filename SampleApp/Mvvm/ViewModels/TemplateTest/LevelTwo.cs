using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SampleApp.Mvvm.ViewModels.TemplateTest
{
    public class LevelTwo : BaseVm
    {
        public ObservableCollection<LevelThree> LevelTwoChildren { get; }

        public string Name { get; set; }

        private bool _isLevelTwoExpanded;

        public bool IsLevelTwoExpanded
        {
            get { return _isLevelTwoExpanded; }
            set
            {
                if (_isLevelTwoExpanded != value)
                {
                    _isLevelTwoExpanded = value;
                    Debug.WriteLine($"Node {Name} IsLevelTwoExpanded changed to {_isLevelTwoExpanded}");
                    OnPropertyChanged();
                }
            }
        }

        public LevelTwo(string name)
        {
            Name = name;

            LevelTwoChildren = new ObservableCollection<LevelThree>();

            LevelTwoChildren.Add(new LevelThree("2-1"));
            LevelTwoChildren.Add(new LevelThree("2-2"));
            LevelTwoChildren.Add(new LevelThree("2-3"));
        }
    }
}
