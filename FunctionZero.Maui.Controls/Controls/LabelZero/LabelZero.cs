using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FunctionZero.Maui.Controls
{
    public class LabelZero : Label
    {
        public LabelZero()
        {
            TapGestureRecognizer tgr = new TapGestureRecognizer();
            tgr.Tapped += (_, __) => Command?.Execute(CommandParameter);
            this.GestureRecognizers.Add(tgr);
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(LabelZero), null, BindingMode.OneWay, null, CommandChanged);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        private static void CommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (LabelZero)bindable;
        }




        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(LabelZero), null, BindingMode.OneWay, null, CommandParameterChanged);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        private static void CommandParameterChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (LabelZero)bindable;
        }
    }
}
