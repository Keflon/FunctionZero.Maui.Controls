using SampleApp.Mvvm.ViewModels;
using SampleApp.Mvvm.ViewModels.TemplateTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Mvvm.PageViewModels.Tree
{
    public class TemplateSelectorTreePageVm : BasePageVm
    {
        public LevelZero RootNode { get; }

        public TemplateSelectorTreePageVm()
        {
            RootNode = new LevelZero("Root") { IsLevelZeroExpanded = true };

        }
    }
}
