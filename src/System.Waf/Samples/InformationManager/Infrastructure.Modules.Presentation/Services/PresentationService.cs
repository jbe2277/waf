﻿using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using Waf.InformationManager.Common.Applications.Services;

namespace Waf.InformationManager.Infrastructure.Modules.Presentation.Services;

internal sealed class PresentationService : IPresentationService
{
    public void Initialize()
    {
        FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
    }
}
