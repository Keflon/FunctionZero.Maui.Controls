using FunctionZero.CommandZero;
using FunctionZero.Maui.MvvmZero;
using SampleApp.Mvvm.PageViewModels.Tree;
using SampleApp.Mvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SampleApp.Mvvm.PageViewModels
{
    public enum AppFlyoutItems
    {
        WobblyListView = 0,
        LazyListView,
        TurbulentListView,


        BasicTree,
        TemplateSelectorTree,
        SelfEnumerableTree,

        
    }

    public class AppFlyoutPageVm : BasePageVm
    {
        private readonly IPageServiceZero _pageService;



        public ICommand ItemTappedCommand { get; }

        public AppFlyoutPageVm(IPageServiceZero pageService)
        {
            _pageService = pageService;

            _pageService.FlyoutController.FlyoutLayoutBehavior = FlyoutLayoutBehavior.Popover;

            ItemTappedCommand = new CommandBuilder().AddGuard(this).SetExecute(ItemTappedCommandExecute).Build();
        }
        private bool VmInitializer(object obj)
        {
            return true;
        }

        private void ItemTappedCommandExecute(object arg)
        {
            switch ((AppFlyoutItems)arg)
            {
                case AppFlyoutItems.WobblyListView:
                    break;
                case AppFlyoutItems.LazyListView:
                    break;
                case AppFlyoutItems.TurbulentListView:
                    break;
                case AppFlyoutItems.BasicTree:
                    _pageService.FlyoutController.SetDetailVm(typeof(BasicTreePageVm), true);
                    break;
                case AppFlyoutItems.TemplateSelectorTree:
                    _pageService.FlyoutController.SetDetailVm(typeof(TemplateSelectorTreePageVm), true);
                    break;
                case AppFlyoutItems.SelfEnumerableTree:
                    _pageService.FlyoutController.SetDetailVm(typeof(SelfEnumerableTreePageVm), true);
                    break;
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

        }

        public override void OnOwnerPageAppearing()
        {
            base.OnOwnerPageAppearing();
                    _pageService.FlyoutController.SetDetailVm(typeof(BasicTreePageVm), true);
        }
    }
}

