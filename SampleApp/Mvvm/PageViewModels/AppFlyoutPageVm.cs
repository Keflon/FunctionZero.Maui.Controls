using FunctionZero.CommandZero;
using FunctionZero.Maui.MvvmZero;
using SampleApp.Mvvm.PageViewModels.Expander;
using SampleApp.Mvvm.PageViewModels.List;
using SampleApp.Mvvm.PageViewModels.Mask;
using SampleApp.Mvvm.PageViewModels.MultiView;
using SampleApp.Mvvm.PageViewModels.Translations;
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
        TurbulentTree,
        CircleMask,
        Jay,
        ExpanderBar, 
        ExpanderBarTest,
        MultiViewModal,
        TranslationHome
    }



    public class AppFlyoutPageVm : BasePageVm
    {
        private readonly IPageServiceZero _pageService;
    private bool _isExpanded;

    public bool IsExpanded
    {
            get => _isExpanded;
            set=> SetProperty(ref _isExpanded, value);
    }
        public ICommand ItemTappedCommand { get; }

        public AppFlyoutPageVm(IPageServiceZero pageService)
        {
            _pageService = pageService;

            _pageService.FlyoutController.FlyoutLayoutBehavior = FlyoutLayoutBehavior.Popover;

            ItemTappedCommand = new CommandBuilder().AddGuard(this).SetExecute(ItemTappedCommandExecute).Build();

            AddPageTimer(1000, (obj) => IsExpanded = !IsExpanded, null, null);
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
                    _pageService.FlyoutController.SetDetailVm(typeof(WobblyListPageVm), true);
                    break;
                case AppFlyoutItems.LazyListView:
                    _pageService.FlyoutController.SetDetailVm(typeof(LazyListPageVm), true);
                    break;
                case AppFlyoutItems.TurbulentListView:
                    _pageService.FlyoutController.SetDetailVm(typeof(TurbulentListPageVm), true);
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
                case AppFlyoutItems.TurbulentTree:
                    _pageService.FlyoutController.SetDetailVm(typeof(TurbulentTreePageVm), true);
                    break;
                case AppFlyoutItems.CircleMask:
                    _pageService.FlyoutController.SetDetailVm(typeof(CircleMaskPageVm), true);
                    break;
                case AppFlyoutItems.Jay:
                    _pageService.FlyoutController.SetDetailVm(typeof(JayBirthdayPageVm), true);
                    break;
                case AppFlyoutItems.ExpanderBar:
                    _pageService.FlyoutController.SetDetailVm(typeof(ExpanderBarPageVm), true);
                    break;
                case AppFlyoutItems.ExpanderBarTest:
                    _pageService.FlyoutController.SetDetailVm(typeof(ExpanderTestPageVm), true);
                    break;
                case AppFlyoutItems.MultiViewModal:
                    _pageService.FlyoutController.SetDetailVm(typeof(MultiViewModalPageVm), true);
                    break;
                case AppFlyoutItems.TranslationHome:
                    _pageService.FlyoutController.SetDetailVm(typeof(TranslationHomePageVm), true);
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
            _pageService.FlyoutController.SetDetailVm(typeof(ExpanderBarPageVm), true);
        }
    }
}

