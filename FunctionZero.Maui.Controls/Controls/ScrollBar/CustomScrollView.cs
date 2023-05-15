using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FunctionZero.Maui.Controls.ScrollBar
{
    public class CustomScrollView : ScrollView, IDisposable
    {
        public CustomScrollView()
        {
            var view = new AbsoluteLayout();
            view.WidthRequest = 2;
            view.HeightRequest = 2;
            view.VerticalOptions = LayoutOptions.Start;
            Content = view;

            base.Scrolled += CustomScrollView_Scrolled;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if ((width > 0) && (height > 0))
            {
                Content.WidthRequest = width;
                Content.HeightRequest = height;
                TrySetMargin();
            }
        }

        public AbsoluteLayout Canvas => (AbsoluteLayout)Content;

        private void CustomScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            Content.TranslationY = e.ScrollY;
        }

        #region ContentHeight

        public static readonly BindableProperty ContentHeightProperty =
            BindableProperty.Create(nameof(ContentHeight), typeof(double), typeof(CustomScrollView), (double)0, BindingMode.OneWay, null, ContentHeightChanged);

        private static void ContentHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (CustomScrollView)bindable;

            self.TrySetMargin();
        }

        private void TrySetMargin()
        {
            if (Content.HeightRequest > 0)
                Content.Margin = new Thickness(0, 0, 0, ContentHeight - Content.HeightRequest);
        }

        public double ContentHeight
        {
            get => (double)GetValue(ContentHeightProperty);
            set => SetValue(ContentHeightProperty, value);
        }

        #endregion

        #region ScrollYRequest

        public static readonly BindableProperty ScrollYRequestProperty =
BindableProperty.Create(nameof(ScrollYRequest), typeof(double), typeof(CustomScrollView), (double)0, BindingMode.TwoWay, null, ScrollYRequestChanged, null, CoerceScrollYValue);
        // ATTENTION: TwoWay Binding a double to a ScrollOffset on a ScrollView can lose precision by varying amounts on different platforms, causing an event storm!
        // Ignoring small changes prevents the storm.
        private static object CoerceScrollYValue(BindableObject bindable, object value)
        {
            var self = (CustomScrollView)bindable;
            if (Math.Abs(self.ScrollY - (double)value) < 1.0)
                return self.ScrollY;
            return value;
        }

        private static void ScrollYRequestChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (CustomScrollView)bindable;
            self.ScrollToAsync(0, (double)newValue, false);
        }

        public void Dispose()
        {

        }

        public double ScrollYRequest
        {
            get => (double)GetValue(ScrollYRequestProperty);
            set => SetValue(ScrollYRequestProperty, value);
        }

        #endregion

    }

}
