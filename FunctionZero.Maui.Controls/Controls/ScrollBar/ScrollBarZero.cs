using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Controls.ScrollBar
{
    public class ScrollBarZero : ContentView
    {
        public ScrollBarZero()
        {
            //this.GestureController.CompositeGestureRecognizers[0].

            _scrollView = new ScrollView();
            _scrollView.Content = new Grid();// { BackgroundColor = Colors.Yellow };
            
            _scrollView.Scrolled += _scrollView_Scrolled;
            this.Content = _scrollView;

            _scrollView.HorizontalScrollBarVisibility = ScrollBarVisibility.Always;
        }

        private void _scrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            ScrollY = e.ScrollY;
        }

        public static readonly BindableProperty ContentHeightProperty =
            BindableProperty.Create(nameof(ContentHeight), typeof(double), typeof(ScrollBarZero), (double)0, BindingMode.OneWay, null, ContentHeightChanged);

        private static void ContentHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (ScrollBarZero)bindable;
            self._scrollView.Content.HeightRequest = (double)newValue;
        }

        public double ContentHeight
        {
            get => (double)GetValue(ContentHeightProperty);
            set => SetValue(ContentHeightProperty, value);
        }


        public static readonly BindableProperty ScrollYProperty =
    BindableProperty.Create(nameof(ScrollY), typeof(double), typeof(ScrollBarZero), (double)0, BindingMode.TwoWay, null, ScrollYChanged, null, CoerceScrollYValue);
        // ATTENTION: TwoWay Binding a double to a ScrollOffset on a ScrollView can lose precision by varying amounts on different platforms, causing an event storm!
        // Ignoring small changes prevents the storm.
        private static object CoerceScrollYValue(BindableObject bindable, object value)
        {
            var self = (ScrollBarZero)bindable;
            if(Math.Abs(self.ScrollY - (double)value)<1.0)
                return self.ScrollY;
            return value;
        }

        private readonly ScrollView _scrollView;

        private static void ScrollYChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (ScrollBarZero)bindable;

            self._scrollView.ScrollToAsync(0, (double)newValue, false);
        }

        public double ScrollY
        {
            get => (double)GetValue(ScrollYProperty);
            set => SetValue(ScrollYProperty, value);
        }
    }
}
