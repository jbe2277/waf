# Win Application Framework (WAF)
#### Successor of the WPF Application Framework

The Win Application Framework (WAF) is a lightweight Framework that helps you to create well-structured XAML Applications (MAUI, Xamarin, WPF, WinUI). It supports you in applying various architectural patterns:
- [Layered Architecture](https://github.com/jbe2277/waf/wiki/Layered-Architecture)
- [Model-View-ViewModel Pattern](https://github.com/jbe2277/waf/wiki/Model-View-ViewModel-Pattern)
- [DataModel View ViewModel Pattern](https://github.com/jbe2277/waf/wiki/DataModel-View-ViewModel-Pattern)

**How to get started?**
- WAF comes with realistic real-world sample applications. Please have a look at them. See Sample Applications below.
- The [Wiki](https://github.com/jbe2277/waf/wiki) provides further guidance.

## Supported Platforms

-	***.Core** *(.NET 6.0 and .NET Standard 2.0)*: Support for all .NET based applications.
-	***.Wpf** *(.NET 6.0 and .NET Framework 4.7.2)*: Extended support for Windows Presentation Foundation (WPF).

## NuGet Packages

Package | Usage | Successor of
--- | --- | ---
[System.Waf.Core](https://www.nuget.org/packages/System.Waf.Core) | For all .NET based applications | 
[System.Waf.Wpf](https://www.nuget.org/packages/System.Waf.Wpf) | For WPF applications | [waf](https://www.nuget.org/packages/waf)
[System.Waf.UnitTesting.Core](https://www.nuget.org/packages/System.Waf.UnitTesting.Core) | For unit testing of all .NET based applications | 
[System.Waf.UnitTesting.Wpf](https://www.nuget.org/packages/System.Waf.UnitTesting.Wpf) | For unit testing of WPF applications | [waf.testing](https://www.nuget.org/packages/waf.testing)

## Features

*System.Waf.Core*
- *Foundation*
    - `Cache`: Provides support for [caching](https://github.com/jbe2277/waf/wiki/Cache-Pattern) a value.
    - `Model`: Base class that implements [INotifyPropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged).
    - `ObservableList`: Extends the `ObservableCollection` with support for `INotifyCollectionChanging` and `INotifyCollectionItemChanged`.
    - `ObservableListViewCore`: [Provide change notifications for sorting and filtering.](https://github.com/jbe2277/waf/wiki/ObservableListView%3A-Provide-change-notifications-for-sorting-and-filtering)
    - `SynchronizingList`: Represents a collection that synchronizes all its items with the items of the specified original collection.
    - `ThrottledAction`: [Throttling](https://github.com/jbe2277/waf/wiki/Throttling-to-improve-responsiveness) of multiple method calls to improve the responsiveness of an application.
    - `ValidatableModel`: Base class for a model that supports validation by implementing [INotifyDataErrorInfo](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifydataerrorinfo).
    - `WeakEvent`: Supports listening to events via a weak reference. This can prevent memory leaks. See [WeakEvent](https://github.com/jbe2277/waf/wiki/Weak-Event) page for more details.
-	*Applications*
    -	`ApplicationInfo`: Provides information about the running application.
    - `(Async)DelegateCommand`: An implementation of [ICommand](https://docs.microsoft.com/en-us/dotnet/api/system.windows.input.icommand) that delegates Execute and CanExecute.
    - `RecentFileList`: Most recently used (MRU) file list.
    - `ViewModelCore`: [ViewModel](https://github.com/jbe2277/waf/wiki/Model-View-ViewModel-Pattern) base class with a simple approach to set the DataContext.
-	*Presentation*
    -	`SettingsService`: Load and save user settings as an XML file.

*System.Waf.Wpf*
- *Foundation*
    - `DataErrorInfoSupport`: Helper class for working with the legacy [IDataErrorInfo](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.idataerrorinfo) interface.
- *Applications*    
    - `ObservableListView`: [Same as ObservableListViewCore but using weak event handlers.](https://github.com/jbe2277/waf/wiki/ObservableListView%3A-Provide-change-notifications-for-sorting-and-filtering)
    - `ViewModel`: [ViewModel](https://github.com/jbe2277/waf/wiki/Model-View-ViewModel-Pattern) base class which sets the DataContext delayed via the Dispatcher.
- *Presentation*
    - `DispatcherHelper`: Implementation for DoEvents.
    - `ResourceHelper`: Helper methods to manage resources in WPF.
    - `ValidationHelper`: Support for data validation tracking.
    - *Converters*
        - `BoolToVisibilityConverter`: Converts a boolean value to and from a Visibility value.
        - `InvertBooleanConverter`: Inverts a boolean value.
        - `NullToVisibilityConverter`: Null condition to return the associated Visibility value.
        - `StringFormatConverter`: Converts an object into a formatted string.
        - `ValidationErrorsConverter`: Converts a ValidationError collection to a multi-line string error message.
    - *Services*
        - `FileDialogService`: Shows an open or save file dialog box.
        - `MessageService`: Shows messages via the MessageBox.
       
*System.Waf.UnitTesting.Core*
-	`AssertHelper`: Assertion helper methods for expected exceptions, [CanExecuteChanged](https://docs.microsoft.com/en-us/dotnet/api/system.windows.input.icommand.canexecutechanged) event and [PropertyChanged](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.inotifypropertychanged) event.
-	`UnitTestSynchronizationContext`: [Synchronization context](https://docs.microsoft.com/en-us/dotnet/api/system.threading.synchronizationcontext) for unit tests that simulates the behavior of the WPF or Windows Forms synchronization context.

## Sample Applications
Name | Type | Description
--- | --- | ---
[Waf NewsReader](https://github.com/jbe2277/waf/tree/master/src/NewsReader) | MAUI<br/>.NET 8 | A simple and fast RSS and ATOM news feed reader.<br/><ul><li>Platforms: Android, Windows and iOS</li><li>Architecture: [Layering](https://github.com/jbe2277/waf/wiki/Layered-Architecture), [MVVM](https://github.com/jbe2277/waf/wiki/Model-View-ViewModel-Pattern), Async patterns</li><li>Sync feeds with multiple devices via MS Graph (OneDrive)</li><li>OAuth authentication</li><li>Responsive UI with Navigation pane</li><li>Validation (Add feed view)</li><li>Localized (English and German)</li></ul> 
[Waf Writer](https://github.com/jbe2277/waf/tree/master/src/System.Waf/Samples/Writer) | WPF<br/>.NET 8 | A simplified word processing application.<br/><ul><li>Architecture: [Layering](https://github.com/jbe2277/waf/wiki/Layered-Architecture), [MVVM](https://github.com/jbe2277/waf/wiki/Model-View-ViewModel-Pattern)</li><li>Ribbon & Tabbed MDI (Multiple Document Interface)</li><li>Animated transition between pages</li><li>Most recently used file list (MRU)</li><li>Message service, Open/Save dialog service</li><li>Print preview & Print dialog</li><li>Localized (English and German)</li></ul>
[Waf Book Library](https://github.com/jbe2277/waf/tree/master/src/System.Waf/Samples/BookLibrary) | WPF<br/>.NET 8 | Supports the user to manage his books. Borrowed books can be tracked by this application.<br/><ul><li>Architecture: [Layering](https://github.com/jbe2277/waf/wiki/Layered-Architecture), [Extensions](https://github.com/jbe2277/waf/wiki/Modular-Architecture#4-alternative-extensions), [MVVM](https://github.com/jbe2277/waf/wiki/Model-View-ViewModel-Pattern), [DMVVM](https://github.com/jbe2277/waf/wiki/DataModel-View-ViewModel-Pattern)</li><li>Entity Framework with SQLite</li><li>Validation rules</li><li>Sort & Filter in the DataGrid</li><li>Reporting via WPF FlowDocument & Print support</li></ul>
[Waf Information Manager](https://github.com/jbe2277/waf/tree/master/src/System.Waf/Samples/InformationManager) | WPF<br/>.NET 8 | A modular application that comes with a fake email client and an address book.<br/><ul><li>Architecture: [Layering](https://github.com/jbe2277/waf/wiki/Layered-Architecture), [Modularization](https://github.com/jbe2277/waf/wiki/Modular-Architecture), [MVVM](https://github.com/jbe2277/waf/wiki/Model-View-ViewModel-Pattern)</li><li>Office format ZIP container shared with all modules (Package API and DataContractSerializer)</li><li>Validation rules</li><li>Extensible navigation view & context sensitive toolbar</li><li>Wizard dialog</li></ul>
[Waf Music Manager](https://jbe2277.github.io/musicmanager/) | WPF<br/>.NET 8 | Fast application that makes fun to manage the local music collection.<br/>*MS Store (MSIX), WinRT, Media playback, File queries & properties, Async/await, Drag & Drop*
[Waf DotNetPad](https://jbe2277.github.io/dotnetpad) | WPF<br/>.NET 8 | Code editor to program with C# or Visual Basic.<br/>*MS Store (MSIX), .NET Compiler Platform, Roslyn, AvalonEdit, Auto completion, Async/await*
[Waf File Hash Generator](https://jbe2277.github.io/fhg/) | WinUI 3<br/>.NET 8 | Simple tool that generates the hash values of one or more files.<br/>*MS Store (MSIX), Async, Progress, Drag & Drop*
