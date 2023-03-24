using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Controls.ScrollBar
{
    public class CustomScrollView : ScrollView, IDisposable
    {
        public CustomScrollView()
        {
            base.Scrolled += CustomScrollView_Scrolled;
        }

        private void CustomScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            Content.TranslationY = e.ScrollY;
        }

        protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            return base.MeasureOverride(widthConstraint, heightConstraint - 50.0);
        }

        protected override Size ArrangeOverride(Rect bounds)
        {
            //var newbounds = new Rect(bounds.X, bounds.Y, bounds.Width / 2.0, bounds.Height / 2.0);
            //return base.ArrangeOverride(newbounds);
            return base.ArrangeOverride(bounds);
        }


        #region ContentHeight

        public static readonly BindableProperty ContentHeightProperty =
            BindableProperty.Create(nameof(ContentHeight), typeof(double), typeof(CustomScrollView), (double)0, BindingMode.OneWay, null, ContentHeightChanged);

        private static void ContentHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (CustomScrollView)bindable;
            //self.content = new Size(300, self.ContentHeight);
            self.Content.Margin = new Thickness(0, 0, 0, self.ContentHeight - self.Content.Height);
            //self.Content.Margin = new Thickness(0, 0, 0, 100);


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
