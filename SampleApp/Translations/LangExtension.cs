﻿using FunctionZero.Maui.MarkupExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Translations
{
    public class LangExtension : BaseLanguageExtension<LangStrings>
    {
        public LangExtension() : base("languageResource")
        {

        }
    }
}