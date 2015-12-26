using System;
using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Test.InformationManager.EmailClient.Modules.Applications.Views
{
    [Export(typeof(INewEmailView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class MockNewEmailView : MockView, INewEmailView
    {
        public static Action<MockNewEmailView> ShowAction { get; set; }

        public object Owner { get; private set; }

        public bool IsVisible { get; private set; }


        public void Show(object owner)
        {
            this.Owner = owner;
            this.IsVisible = true;
            if (ShowAction != null) { ShowAction(this); }
        }

        public void Close()
        {
            this.Owner = null;
            this.IsVisible = false;
        }
    }
}
