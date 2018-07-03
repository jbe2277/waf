# Win Application Framework (WAF)
#### Successor of the WPF Application Framework

The Win Application Framework (WAF) is a lightweight Framework that helps you to create well-structured XAML Applications in WPF and UWP. It supports you in applying various architectural patterns:
* [Layered Architecture](https://github.com/jbe2277/waf/wiki/Layered-Architecture)
* [Modular Architecture](https://github.com/jbe2277/waf/wiki/Modular-Architecture)
* [Model-View-ViewModel pattern](https://github.com/jbe2277/waf/wiki/Model-View-ViewModel-Pattern)

**How to get started?**
* WAF comes with realistic real-world sample applications. Please have a look at them. See Sample Applications below.
* The [Wiki](https://github.com/jbe2277/waf/wiki) provides further guidance.
* The fastest way to create a WAF project is by using the [WAF Visual Studio Project Template](https://marketplace.visualstudio.com/items?itemName=jbe2277.WAFProjectTemplate). Please update the WAF NuGet packages to the latest stable version after project creation.

## Version 4

Starting with version 4 the framework comes with the new name Win Application Framework instead of WPF Application Framework. This rename shows that the framework supports now various application types:
-	WPF (Windows Presentation Foundation)
-	UWP (Universal Windows Platform)
-	Core (Basic support for all .NET based applications)

## Previous versions

Older versions of WAF 1.0 - 3.2 can be found at CodePlex: http://waf.codeplex.com

## NuGet Packages

Package | Usage | Successor of
--- | --- | ---
[System.Waf.Wpf](https://www.nuget.org/packages/System.Waf.Wpf) | For WPF applications | [waf](https://www.nuget.org/packages/waf)
[System.Waf.Uwp](https://www.nuget.org/packages/System.Waf.Uwp) | For Universal Windows Apps | 
[System.Waf.Core](https://www.nuget.org/packages/System.Waf.Core) | For all .NET based applications | 
[System.Waf.UnitTesting.Wpf](https://www.nuget.org/packages/System.Waf.UnitTesting.Wpf) | For unit testing of WPF applications | [waf.testing](https://www.nuget.org/packages/waf.testing)
[System.Waf.UnitTesting.Core](https://www.nuget.org/packages/System.Waf.UnitTesting.Core) | For unit testing of all .NET based applications | 

## Sample Applications
Name | Type | Description | Links
--- | --- | --- | ---
[Jbe NewsReader](https://github.com/jbe2277/waf/tree/master/src/NewsReader) | UWP | A simple and fast RSS and ATOM news feed reader.<br/>><ul><li>Architecture: [Layering](https://github.com/jbe2277/waf/wiki/Layered-Architecture), [MVVM](https://github.com/jbe2277/waf/wiki/Model-View-ViewModel-Pattern), Async patterns</li><li>Sync feeds with multiple devices via OneDrive</li><li>WebAccount authentication and End-to-End encryption</li><li>Responsive UI with Navigation pane</li><li>Integrated Web Browser view</li><li>Support for light and dark Theme</li><li>Localized (English and German)</li></ul>| [Windows Store](https://www.microsoft.com/store/apps/jbe-newsreader/9nblggh4p3b6)
[Waf Writer](https://github.com/jbe2277/waf/tree/master/src/System.Waf/Samples/Writer) | WPF | A simplified word processing application.<br/><ul><li>Architecture: [Layering](https://github.com/jbe2277/waf/wiki/Layered-Architecture), [MVVM](https://github.com/jbe2277/waf/wiki/Model-View-ViewModel-Pattern)</li><li>Ribbon & Tabbed MDI (Multiple Document Interface)</li><li>Animated transition between pages</li><li>Most recently used file list (MRU)</li><li>Message service, Open/Save dialog service</li><li>Print preview & Print dialog</li><li>Localized (English and German)</li></ul> | [Doc](https://github.com/jbe2277/waf/blob/master/src/System.Waf/Samples/Writer/Writer.docx?raw=true)
[Waf Book Library](https://github.com/jbe2277/waf/tree/master/src/System.Waf/Samples/BookLibrary) | WPF | Supports the user to manage his books. Borrowed books can be tracked by this application.<br/><ul><li>Architecture: [Layering](https://github.com/jbe2277/waf/wiki/Layered-Architecture), [Extensions](https://github.com/jbe2277/waf/wiki/Modular-Architecture#4-alternative-extensions), [MVVM](https://github.com/jbe2277/waf/wiki/Model-View-ViewModel-Pattern), [DMVVM](https://github.com/jbe2277/waf/wiki/DataModel-View-ViewModel-Pattern)</li><li>Entity Framework with SQLite</li><li>Validation rules</li><li>Sort & Filter in the DataGrid</li><li>Reporting via WPF FlowDocument & Print support</li></ul> | [Doc](https://github.com/jbe2277/waf/blob/master/src/System.Waf/Samples/BookLibrary/BookLibrary.docx?raw=true)
[Waf Information Manager](https://github.com/jbe2277/waf/tree/master/src/System.Waf/Samples/InformationManager) | WPF | A modular application that comes with a fake email client and an address book.<br/><ul><li>Architecture: [Layering](https://github.com/jbe2277/waf/wiki/Layered-Architecture), [Modularization](https://github.com/jbe2277/waf/wiki/Modular-Architecture), [MVVM](https://github.com/jbe2277/waf/wiki/Model-View-ViewModel-Pattern)</li><li>Office format ZIP container shared with all modules (Package API and DataContractSerializer)</li><li>Validation rules</li><li>Extensible navigation view & context sensitive toolbar</li><li>Wizard dialog</li></ul> | [Doc](https://github.com/jbe2277/waf/blob/master/src/System.Waf/Samples/InformationManager/InformationManager.docx?raw=true)
[Waf Music Manager](https://jbe2277.github.io/musicmanager/) | WPF | Fast application that makes fun to manage the local music collection.<br/>*WinRT, Media playback, File queries & properties, Async/await, Drag & Drop, ClickOnce* |
[Waf DotNetPad](https://jbe2277.github.io/dotnetpad) | WPF | Code editor to program with C# or Visual Basic.<br/>*.NET Compiler Platform, Roslyn, AvalonEdit, Auto completion, Async/await, ClickOnce* |
[Waf DotNetApiBrowser](https://jbe2277.github.io/DotNetApiBrowser) | WPF | Windows application for browsing the public API of .NET Assemblies and NuGet packages.<br/>*.NET Compiler Platform, Roslyn, AvalonEdit, NuGet, Async/await, Validation, ClickOnce* |
