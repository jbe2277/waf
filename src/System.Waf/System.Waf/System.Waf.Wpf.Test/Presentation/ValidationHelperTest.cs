#if NET461
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Presentation;
using System.Windows.Controls;
using System.Waf.UnitTesting;
using System.Windows.Data;
using System.Windows;

namespace Test.Waf.Presentation
{
    [TestClass]
    public class ValidationHelperTest
    {
        [TestMethod]
        public void IsEnabledTest()
        {
            var view = new UserControl();
            Assert.IsFalse(ValidationHelper.GetIsEnabled(view));

            ValidationHelper.SetIsEnabled(view, true);
            Assert.IsTrue(ValidationHelper.GetIsEnabled(view));

            // It's not allowed to deactivate the tracker when it was enabled.
            AssertHelper.ExpectedException<ArgumentException>(() => ValidationHelper.SetIsEnabled(view, false));
        }

        [TestMethod]
        public void IsValidTest()
        {
            var view = new UserControl();
            var viewModel = new MockViewModel();
            view.DataContext = viewModel;

            Assert.IsTrue(ValidationHelper.GetIsValid(view));
            
            // It is not allowed to call the set method
            AssertHelper.ExpectedException<InvalidOperationException>(() => ValidationHelper.SetIsValid(view, true));

            // But we can set a binding.
            var binding = new Binding(nameof(MockViewModel.IsValid));
            
            // ValidationHelper.IsEnabled is false => exception
            AssertHelper.ExpectedException<InvalidOperationException>(() => 
                BindingOperations.SetBinding(view, ValidationHelper.IsValidProperty, binding));
            ClearIsValidBinding(view);

            ValidationHelper.SetIsEnabled(view, true);

            // Binding must have the mode OneWayToSource => exception
            AssertHelper.ExpectedException<InvalidOperationException>(() =>
                BindingOperations.SetBinding(view, ValidationHelper.IsValidProperty, binding));
            ClearIsValidBinding(view);

            // Now everything is correct => no exception
            binding = new Binding(nameof(MockViewModel.IsValid));
            binding.Mode = BindingMode.OneWayToSource;
            BindingOperations.SetBinding(view, ValidationHelper.IsValidProperty, binding);


            ValidationHelper.InternalSetIsValid(view, false);
            Assert.IsFalse(ValidationHelper.GetIsValid(view));
        }

        private void ClearIsValidBinding(DependencyObject obj)
        {
            try
            {
                BindingOperations.ClearBinding(obj, ValidationHelper.IsValidProperty);
            }
            catch
            {
            }
        }


        private class MockViewModel
        {
            public bool IsValid { get; set; }
        }
    }
}
#endif
