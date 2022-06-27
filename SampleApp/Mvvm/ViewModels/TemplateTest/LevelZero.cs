using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SampleApp.Mvvm.ViewModels.TemplateTest
{
    public class LevelZero : BaseVm
    {
        public ObservableCollection<LevelOne> LevelZeroChildren { get; }

        private bool _isLevelZeroExpanded;

        public bool IsLevelZeroExpanded
        {
            get { return _isLevelZeroExpanded; }
            set
            {
                if (_isLevelZeroExpanded != value)
                {
                    _isLevelZeroExpanded = value;
                    Debug.WriteLine($"Node {Name} IsLevelZeroExpanded changed to {_isLevelZeroExpanded}");
                    OnPropertyChanged();
                }
            }
        }

        public string Name { get; }

        public LevelZero(string name)
        {
            Name = name;

            LevelZeroChildren = new ObservableCollection<LevelOne>();

            LevelZeroChildren.Add(new LevelOne("0-1"));
            LevelZeroChildren.Add(new LevelOne("0-2") { IsLevelOneExpanded = true });
            LevelZeroChildren.Add(new LevelOne("0-3"));

        }
    }
}
