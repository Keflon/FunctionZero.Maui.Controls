using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.MarkupExtensions
{
    public class TranslationService<TEnum> where TEnum : Enum
    {
        public class LanguageExtension : BaseLanguageExtension<TEnum>
        {

        }
    }
}
//public /*abstract*/ class BaseLanguageExtension<TEnum> : IMarkupExtension<Binding>, INotifyPropertyChanged where TEnum : Enum
