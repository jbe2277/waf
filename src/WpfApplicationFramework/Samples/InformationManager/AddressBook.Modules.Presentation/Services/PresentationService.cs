using System;
using System.ComponentModel.Composition;
using System.Windows;
using Waf.InformationManager.Common.Applications.Services;

namespace Waf.InformationManager.AddressBook.Modules.Presentation.Services
{
    [Export(typeof(IPresentationService))]
    internal class PresentationService : IPresentationService
    {
        public void Initialize()
        {
            // This module doesn't require XAML resources until now.
        }
    }
}
