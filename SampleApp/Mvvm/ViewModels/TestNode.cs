using FunctionZero.TreeZero;
using System.ComponentModel;
using System.Diagnostics;

namespace SampleApp.Mvvm.ViewModels
{
    public class TestNode : Node<TestNode>
    {
        public TestNode(string name)
        {
            Name = name;
        }

        public string Name { get; }

        private bool _isDataExpanded;

        public bool IsDataExpanded
        {
            get { return _isDataExpanded; }
            set
            {
                if (_isDataExpanded != value)
                {
                    _isDataExpanded = value;
                    Debug.WriteLine($"Node {Name} IsDataExpanded changed to {IsDataExpanded}");
                    OnPropertyChanged();
                }
            }
        }
    }
}