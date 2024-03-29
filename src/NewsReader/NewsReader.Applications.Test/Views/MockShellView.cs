﻿using System.Waf.Applications;
using System.Waf.UnitTesting.Mocks;
using Waf.NewsReader.Applications.ViewModels;
using Waf.NewsReader.Applications.Views;

namespace Test.NewsReader.Applications.Views;

public class MockShellView : MockView, IShellView
{
    private readonly Stack<object> pageStack = new();
    private object currentPage = null!;

    public object CurrentPage 
    { 
        get => currentPage;
        private set
        {
            currentPage = value;
            ((ShellViewModel)DataContext!).InternalSetCurrentPage(CurrentPage);
        }
    }
    
    public Task PushAsync(object page) { pageStack.Push(page); CurrentPage = page; return Task.CompletedTask; }

    public Task PopAsync() { CurrentPage = pageStack.Pop(); return Task.CompletedTask; }

    public void CloseFlyout() { }

    public ViewPair<TView, TViewModel> GetCurrentView<TView, TViewModel>() where TView : IView where TViewModel : IViewModelCore 
            => GetCurrentViewOrNull<TView, TViewModel>() ?? throw new InvalidOperationException($"Wrong current view. Expected: {typeof(TView).Name}, Actual: {CurrentPage?.GetType().Name ?? "null"}");

    public ViewPair<TView, TViewModel>? GetCurrentViewOrNull<TView, TViewModel>() where TView : IView where TViewModel : IViewModelCore => CurrentPage is TView v ? new(v) : null;
}

