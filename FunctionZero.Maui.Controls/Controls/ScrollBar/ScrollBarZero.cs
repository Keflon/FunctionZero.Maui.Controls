﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Controls.ScrollBar
{
    public class ScrollViewZero : ScrollView
    {
        public ScrollViewZero()
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Always;
        }


        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);

        }


        #region ContentHeight

        public static readonly BindableProperty ContentHeightProperty =
            BindableProperty.Create(nameof(ContentHeight), typeof(double), typeof(ScrollViewZero), (double)0, BindingMode.OneWay, null, ContentHeightChanged);

        private static void ContentHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (ScrollViewZero)bindable;

        }

        public double ContentHeight
        {
            get => (double)GetValue(ContentHeightProperty);
            set => SetValue(ContentHeightProperty, value);
        }

        #endregion

        #region ScrollYRequest

        public static readonly BindableProperty ScrollYRequestProperty =
BindableProperty.Create(nameof(ScrollYRequest), typeof(double), typeof(ScrollViewZero), (double)0, BindingMode.TwoWay, null, ScrollYRequestChanged, null, CoerceScrollYValue);
        // ATTENTION: TwoWay Binding a double to a ScrollOffset on a ScrollView can lose precision by varying amounts on different platforms, causing an event storm!
        // Ignoring small changes prevents the storm.
        private static object CoerceScrollYValue(BindableObject bindable, object value)
        {
            var self = (ScrollViewZero)bindable;
            if (Math.Abs(self.ScrollY - (double)value) < 1.0)
                return self.ScrollY;
            return value;
        }

        private static void ScrollYRequestChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (ScrollViewZero)bindable;
            self.ScrollToAsync(0, (double)newValue, false);
        }

        public double ScrollYRequest
        {
            get => (double)GetValue(ScrollYRequestProperty);
            set => SetValue(ScrollYRequestProperty, value);
        } 

        #endregion
    }
}
