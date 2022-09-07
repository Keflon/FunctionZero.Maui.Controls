using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionZero.Maui.Controls
{
    public partial class PlatformSetup
    {
        private static PlatformSetup _instance;

        private static bool _setupSucceeded = false;

        public static bool TryHookPlatformTouch()
        {
            if (_instance == null)
                _instance = new PlatformSetup();
            
            return _setupSucceeded;
        }
    }
}
