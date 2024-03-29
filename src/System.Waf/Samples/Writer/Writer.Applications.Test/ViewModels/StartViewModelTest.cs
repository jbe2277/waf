﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Waf.Writer.Applications.ViewModels;

namespace Test.Writer.Applications.ViewModels;

[TestClass]
public class StartViewModelTest : ApplicationsTest
{
    [TestMethod]
    public void GetFileService()
    {
        var startViewModel = Get<StartViewModel>();
        Assert.IsNotNull(startViewModel.FileService);
    }
}
