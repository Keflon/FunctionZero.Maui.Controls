using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Widgets
{
    public class WidgetContainer : BaseWidget<IEnumerable<BaseWidget>>
    {
        public WidgetContainer(
            string title,
            string subtitle,
            string UiHint,
            IEnumerable<BaseWidget> children = null)
                : base(title, subtitle, UiHint)
        {
            Data = children != null ? new ObservableCollection<BaseWidget>(children) : new ObservableCollection<BaseWidget>();
        }
    }
}
