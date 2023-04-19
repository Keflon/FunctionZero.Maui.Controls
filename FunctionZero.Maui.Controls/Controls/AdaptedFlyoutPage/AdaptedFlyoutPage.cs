using System.ComponentModel;

namespace FunctionZero.Maui.Controls
{
    public class AdaptedFlyoutPage : FlyoutPage
    {
        Page _oldFlyout;
        public AdaptedFlyoutPage()
        {
#if WINDOWS
            PropertyChanged += AdaptedFlyoutPage_PropertyChanged;
#endif
            _oldFlyout = null;
        }

        private void AdaptedFlyoutPage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FlyoutPage.Flyout))
            {
                if (_oldFlyout != null)
                    _oldFlyout.PropertyChanged -= FlyoutFlyout_PropertyChanged;

                if (Flyout != null)
                    Flyout.PropertyChanged += FlyoutFlyout_PropertyChanged;

                _oldFlyout = Flyout;
            }
        }
        private async void FlyoutFlyout_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Page.IsFocused))
            {
                if (FlyoutLayoutBehavior == FlyoutLayoutBehavior.Popover)
                {
                    if (Flyout.IsFocused == false)
                    {
                        // Delay is necessary otherwise closing the Flyout using the hamburger menu
                        // immediately repoens it, for no sane reason.
                        await Task.Delay(120);
                        IsPresented = false;
                    }
                }
            }
        }
    }
}

