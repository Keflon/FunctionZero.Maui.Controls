using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp.Services
{


    public class ThemeService<T> where T : Enum
    {
        public ThemeService()
        {
        }
    }

    public enum ThemeType
    {
        None,
        Light,
        Dark,
        Developer,
        HighVisibilityLight,
        HighVisibilityDark,
        Disco
    }

    public class SampleAppThemeService : ThemeService<ThemeType>
    {
        public SampleAppThemeService()
        {
            this.CurrentThemeType = ThemeType.Disco;
        }

        public ThemeType CurrentThemeType;
    }

    public class Radish
    {
        public Radish(SampleAppThemeService sampleAppThemeService)
        {
            sampleAppThemeService.CurrentThemeType = ThemeType.Dark;
        }
    }
}
