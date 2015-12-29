using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.Presentation.Services;
using System.Reflection;
using System.Windows;
using System.Threading;
using System.Globalization;

namespace Test.Waf.Presentation.Services
{
    [TestClass]
    public class MessageServiceTest
    {
        [TestMethod]
        public void MessageBoxResultTest()
        {
            MessageService messageService = new MessageService();
            PropertyInfo messageBoxResultInfo = typeof(MessageService).GetProperty("MessageBoxResult", 
                BindingFlags.Static | BindingFlags.NonPublic);
            
            Assert.AreEqual(MessageBoxResult.None, (MessageBoxResult)messageBoxResultInfo.GetValue(null, null));
        }

        [TestMethod]
        public void MessageBoxOptionsTest()
        {
            MessageService messageService = new MessageService();
            PropertyInfo messageBoxOptionsInfo = typeof(MessageService).GetProperty("MessageBoxOptions",
                BindingFlags.Static | BindingFlags.NonPublic);

            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            Assert.AreEqual(MessageBoxOptions.None, (MessageBoxOptions)messageBoxOptionsInfo.GetValue(null, null));

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar-SA");
            Assert.AreEqual(MessageBoxOptions.RtlReading, (MessageBoxOptions)messageBoxOptionsInfo.GetValue(null, null));
        }
    }
}
