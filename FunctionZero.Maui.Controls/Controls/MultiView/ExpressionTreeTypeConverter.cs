using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace FunctionZero.Maui.Controls
{
        [TypeConverter(typeof(ExpressionTreeTypeConverter))]
    public class ExpressionTreeTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return base.CanConvertFrom(context, sourceType);

        }

        public override bool CanConvertTo(ITypeDescriptorContext context, [NotNullWhen(true)] Type destinationType)
        {
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var ep = ExpressionParserZero.Binding.ExpressionParserFactory.GetExpressionParser();
            var retval = ep.Parse(value as string);

            return retval;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}