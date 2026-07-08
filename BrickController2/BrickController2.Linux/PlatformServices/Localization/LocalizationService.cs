using BrickController2.PlatformServices.Localization;
using System.Globalization;
using System.Threading;

namespace BrickController2.Linux.PlatformServices.Localization;

public class LocalizationService : ILocalizationService
{
    private CultureInfo? _cultureInfo;

    public CultureInfo DefaultCultureInfo
    {
        get
        {
            var cultureName = CultureInfo.CurrentUICulture.Name;
            return string.IsNullOrWhiteSpace(cultureName)
                ? new CultureInfo("en")
                : CultureInfo.CurrentUICulture;
        }
    }

    public CultureInfo CurrentCultureInfo
    {
        get
        {
            _cultureInfo ??= DefaultCultureInfo;
            return _cultureInfo;
        }

        set
        {
            _cultureInfo = value;
            CultureInfo.CurrentCulture = value;
            CultureInfo.CurrentUICulture = value;
            Thread.CurrentThread.CurrentCulture = value;
            Thread.CurrentThread.CurrentUICulture = value;
        }
    }
}
