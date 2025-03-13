using Foundation;
using System.Globalization;
using Waf.NewsReader.Applications.Services;

namespace Waf.NewsReader.MauiSystem.Platforms.iOS.Services;

// See https://github.com/mono/mono/issues/16827
internal sealed class LocalizationService : ILocalizationService
{
    public void Initialize()
    {
        var preferredLanguage = NSLocale.PreferredLanguages.FirstOrDefault()?.Replace("_", "-", StringComparison.Ordinal);
        if (preferredLanguage == null) return;
        if (CultureInfo.CurrentCulture.Name == preferredLanguage) return;
        var cultureCode = string.Join('-', preferredLanguage.Split('-').Skip(1));
        var supportedCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

        // Note: This is a very simple approach to find the first culture with the country code -> might come with the wrong language
        var culture = supportedCultures.FirstOrDefault(x => x.Name[3..] == cultureCode);
        if (culture != null)
        {
            CultureInfo.CurrentCulture = CultureInfo.DefaultThreadCurrentCulture = culture;
        }
    }
}