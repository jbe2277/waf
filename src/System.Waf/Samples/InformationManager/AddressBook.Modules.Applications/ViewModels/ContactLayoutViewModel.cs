﻿using System.Waf.Applications;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;

namespace Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;

public class ContactLayoutViewModel : ViewModel<IContactLayoutView>
{
    private object? contactListView;
    private object? contactView;

    public ContactLayoutViewModel(IContactLayoutView view) : base(view)
    {
    }

    public object? ContactListView
    {
        get => contactListView;
        set => SetProperty(ref contactListView, value);
    }

    public object? ContactView
    {
        get => contactView;
        set => SetProperty(ref contactView, value);
    }
}
