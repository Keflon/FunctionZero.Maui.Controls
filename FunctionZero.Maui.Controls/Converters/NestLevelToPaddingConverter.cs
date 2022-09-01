//using FunctionZero.Maui.Controls;
//using Microsoft.Maui;
//using Microsoft.Maui.Controls;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FunctionZero.Maui.Converters
//{
//    public class NestLevelToPaddingConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            var treeView = GetTreeViewForElement((Element)parameter);

//            try
//            {
//                var nestValue = (int)value - 1;

//                var nestLevel = nestValue * treeView.IndentMultiplier;

//                return new Thickness(nestLevel, 0, 0, 0);
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine(ex);
//            }
//            return null;
//        }

//        private TreeViewZero GetTreeViewForElement(Element parameter)
//        {
//            while (parameter != null)
//                if (parameter is TreeViewZero treeView)
//                    return treeView;
//                else
//                    parameter = parameter.Parent;

//            return null;
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
