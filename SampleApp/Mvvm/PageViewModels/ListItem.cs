using System.ComponentModel;

namespace SampleApp.Mvvm.PageViewModels
{
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
}


