using FunctionZero.Maui.MarkupExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Translations
{
    public class LanguageService<TEnum> where TEnum : Enum
    {
        public class LangExtension : BaseLanguageExtension<TEnum>
        {
            public LangExtension() : base("languageResource")
            {

            }
        }
    }
}
