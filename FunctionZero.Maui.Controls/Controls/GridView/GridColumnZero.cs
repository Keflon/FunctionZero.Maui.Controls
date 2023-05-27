using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Controls
{
    public class GridColumnZero : BindableObject
    {
        public GridColumnZero()
        {
            
        }
        #region ItemTemplateProperty

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(GridColumnZero), null, BindingMode.OneWay);

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        #endregion

        #region ItemContainerStyleProperty

        public static readonly BindableProperty ItemContainerStyleProperty = BindableProperty.Create(nameof(ItemContainerStyle), typeof(Style), typeof(GridColumnZero), null, BindingMode.OneWay, null, ItemContainerStyleChanged);

        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        private static void ItemContainerStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (GridColumnZero)bindable;
        }

        #endregion

        #region Width

        public static readonly BindableProperty WidthProperty = BindableProperty.Create(nameof(Width), typeof(double), typeof(GridColumnZero), null, BindingMode.OneWay, null, WidthChanged);

        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        private static void WidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var self = (GridColumnZero)bindable;
        }

        #endregion
    }
}
