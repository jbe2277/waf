﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Waf.UnitTesting;
using Test.Writer.Applications.Documents;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.ViewModels;

namespace Test.Writer.Applications.ViewModels;

[TestClass]
public class RichTextViewModelTest : ApplicationsTest
{
    [TestMethod]
    public void PropertyChangedEventTest()
    {
        var viewModel = Get<RichTextViewModel>();
        var documentType = new MockRichTextDocumentType();
        var document = new MockRichTextDocument(documentType);
        viewModel.Document = document;

        Assert.AreEqual(document, viewModel.Document);

        AssertHelper.PropertyChangedEvent(viewModel, x => x.IsBold, () => viewModel.IsBold = true);
        Assert.IsTrue(viewModel.IsBold);

        AssertHelper.PropertyChangedEvent(viewModel, x => x.IsItalic, () => viewModel.IsItalic = true);
        Assert.IsTrue(viewModel.IsItalic);

        AssertHelper.PropertyChangedEvent(viewModel, x => x.IsUnderline, () => viewModel.IsUnderline = true);
        Assert.IsTrue(viewModel.IsUnderline);

        AssertHelper.PropertyChangedEvent(viewModel, x => x.IsNumberedList, () => viewModel.IsNumberedList = true);
        Assert.IsTrue(viewModel.IsNumberedList);

        AssertHelper.PropertyChangedEvent(viewModel, x => x.IsBulletList, () => viewModel.IsBulletList = true);
        Assert.IsTrue(viewModel.IsBulletList);

        AssertHelper.PropertyChangedEvent(viewModel, x => x.IsSpellCheckEnabled, () => viewModel.IsSpellCheckEnabled = true);
        Assert.IsTrue(viewModel.IsSpellCheckEnabled);
    }

    [TestMethod]
    public void SyncWithShellServiceTest()
    {
        var shellService = Get<IShellService>();
        var viewModel = Get<RichTextViewModel>();

        Assert.AreNotEqual(viewModel, shellService.ActiveEditingCommands);
        Assert.IsFalse(shellService.ActiveEditingCommands.IsSpellCheckAvailable);

        AssertHelper.PropertyChangedEvent(shellService, x => x.ActiveEditingCommands, () => viewModel.IsVisible = true);
        Assert.IsTrue(viewModel.IsVisible);
        Assert.AreEqual(viewModel, shellService.ActiveEditingCommands);
        Assert.IsTrue(shellService.ActiveEditingCommands.IsSpellCheckAvailable);

        viewModel.IsVisible = false;
        Assert.AreNotEqual(viewModel, shellService.ActiveEditingCommands);
        Assert.IsFalse(shellService.ActiveEditingCommands.IsSpellCheckAvailable);
    }
}
