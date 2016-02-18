using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.Writer.Applications.ViewModels;
using System.Waf.Applications;
using System.Waf.UnitTesting;
using Waf.Writer.Applications.Documents;
using Test.Writer.Applications.Documents;
using Test.Writer.Applications.Services;
using System.Waf.Applications.Services;

namespace Test.Writer.Applications.ViewModels
{
    [TestClass]
    public class StartViewModelTest : TestClassBase
    {
        [TestMethod]
        public void GetFileService()
        {
            StartViewModel startViewModel = Container.GetExportedValue<StartViewModel>();
            Assert.IsNotNull(startViewModel.FileService);
        }
    }
}
