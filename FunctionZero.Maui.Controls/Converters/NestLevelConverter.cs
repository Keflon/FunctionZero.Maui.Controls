using FunctionZero.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Converters
{
    public class NestLevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                // The nest-level starts at 1. Oops!
                var nestLevel = (int)value-1;
                double multiplier;
                if (parameter is Element element)
                    multiplier = GetMultiplier((Element)parameter);
                else if (double.TryParse(parameter as string, out multiplier) == false)
                    multiplier = 30;
                return new Thickness(nestLevel * multiplier, 0, 0, 0);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }

        private double GetMultiplier(Element parameter)
        {
            while (parameter != null)
            {
                if (parameter is TreeViewZero tv)
                    return tv.IndentMultiplier;

                parameter = parameter.Parent;
            }
            throw new InvalidOperationException($"{parameter.GetType()} does not have an ancestor of type TreeViewZero!");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
