using Android.Text.Method;
using Android.Views;
using System.Diagnostics;

namespace FunctionZero.Maui.Controls
{
    // All the code in this file is only included on Android.
    public partial class PlatformSetup
    {
        private float _touchX;
        private float _touchY;
        private float _offset;
        private bool _tapTouchCandidate;
        private PlatformSetup()
        {
            Microsoft.Maui.Handlers.ContentViewHandler.Mapper.AppendToMapping("ListViewZeroTouch", (handler, view) =>
            {
                if (view is ListViewZero listView)
                {
                    handler.PlatformView.Touch += (s, e) => PlatformView_Touch(listView, e);
                }
            });
            _setupSucceeded = true;
        }

        private void PlatformView_Touch(ListViewZero sender, Android.Views.View.TouchEventArgs e)
        {
            Debug.WriteLine(e.Event.Action);

            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    _touchX = e.Event.GetX();
                    _touchY = e.Event.GetY();
                    _offset = sender.ScrollOffset;
                    _tapTouchCandidate = true;
                    break;

                case MotionEventActions.Move:
                    if ((Math.Abs(e.Event.GetX() - _touchX) > 20) && (Math.Abs(e.Event.GetY() - _touchY) > 20))
                        _tapTouchCandidate = false;
                    break;

                case MotionEventActions.Up:
                    // Don't send a touch notification if the ListView has scrolled.
                    if (sender.ScrollOffset == _offset)
                        if (_tapTouchCandidate)
                        {
                            var point = new PointF(
                                _touchX / (float)DeviceDisplay.MainDisplayInfo.Density,
                                _touchY / (float)DeviceDisplay.MainDisplayInfo.Density);


                            sender.ReceivePlatformTap(point.X, point.Y);
                        }
                    break;
                case MotionEventActions.Cancel:
                default:
                    _tapTouchCandidate = true;
                    break;
            }
        }
    }
}