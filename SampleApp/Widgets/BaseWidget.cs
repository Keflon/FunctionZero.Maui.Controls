using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Widgets
{
    public abstract class BaseWidget<TData> : BaseWidget
    {
        private TData _content;

        public BaseWidget(string title, string subtitle, string UiHint) : base(title, subtitle, UiHint)
        {

        }


        public new TData Data { get => _content; set => _content = value; }

    }

    public abstract class BaseWidget
    {
        public BaseWidget(string title, string subtitle, string UiHint)
        {
        Title = title;
            Subtitle = subtitle;
            this.UiHint = UiHint;
        }
        public string Title { get; }
        public string Subtitle { get; }
        public string UiHint { get; }

        public virtual object Data { get; set; }
    }
}
