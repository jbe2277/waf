# Win Application Framework (WAF)
#### Successor of the WPF Application Framework

The Win Application Framework (WAF) is a lightweight Framework that helps you to create well structured XAML Applications. It supports you in applying a Layered Architecture and the Model-View-ViewModel (aka MVVM, M-V-VM, PresentationModel) pattern.

## Version 4

Starting with version 4 the framework comes with the new name Win Application Framework instead of WPF Application Framework. This rename shows that the framework supports now various application types:
-	WPF (Windows Presentation Foundation)
-	UWP (Universal Windows Platform)
-	Basic support for all .NET Core based applications

## Pre-release

Version 4 is still under development and not production ready yet.

At the moment it is recommended to use an older stable version of this framework. You will find these versions on CodePlex: http://waf.codeplex.com

## NuGet Packages

Package | Usage | Successor of
--- | --- | ---
[System.Waf.Wpf](https://www.nuget.org/packages/System.Waf.Wpf) | For WPF applications | [waf](https://www.nuget.org/packages/waf)
[System.Waf.Uwp](https://www.nuget.org/packages/System.Waf.Uwp) | For Universal Windows Apps | 
[System.Waf.Core](https://www.nuget.org/packages/System.Waf.Core) | For .NET Core based applications | 
. |  | 
[System.Waf.UnitTesting.Wpf](https://www.nuget.org/packages/System.Waf.UnitTesting.Wpf) | For unit testing of WPF applications | [waf.testing](https://www.nuget.org/packages/waf.testing)
[System.Waf.UnitTesting.Core](https://www.nuget.org/packages/System.Waf.UnitTesting.Core) | For unit testing of .NET Core based applications | 

## Sample Applications
Name | Type | Description | Links
--- | --- | --- | ---
[Jbe NewsReader](https://github.com/jbe2277/waf/tree/master/src/NewsReader) | UWP | A simple and fast RSS and ATOM news feed reader. | [Windows Store](https://www.microsoft.com/store/apps/jbe-newsreader/9nblggh4p3b6)
[Waf Music Manager](http://jbe2277.github.io/musicmanager/) | WPF | Fast application that makes fun to manage the local music collection.<br/>*WinRT, Async, Await, Drag & Drop* |
[Waf DotNetPad](http://jbe2277.github.io/dotnetpad) | WPF | Code editor to program with C# or Visual Basic.<br/>*.NET Compiler Platform, Roslyn, AvalonEdit, Auto completion* |
[Waf Information Manager](https://github.com/jbe2277/waf/tree/master/src/System.Waf/Samples/InformationManager) | WPF | A modular application that comes with a fake email client and an address book.<br/>*Modular Architecture, UI Wizard, DataContractSerializer* | [Docu](https://github.com/jbe2277/waf/blob/master/src/System.Waf/Samples/InformationManager/InformationManager.docx)
[Waf Book Library](https://github.com/jbe2277/waf/tree/master/src/System.Waf/Samples/BookLibrary) | WPF | Supports the user to manage his books. Borrowed books can be tracked by this application.<br/>*Entity Framework, Validation, Sort & Filter, Reporting* | [Docu](https://github.com/jbe2277/waf/blob/master/src/System.Waf/Samples/BookLibrary/BookLibrary.docx)
[Waf Writer](https://github.com/jbe2277/waf/tree/master/src/System.Waf/Samples/Writer) | WPF | A simplified word processing application.<br/>*Ribbon, Tabbed MDI, Recent files, Print, Localization* | [Docu](https://github.com/jbe2277/waf/blob/master/src/System.Waf/Samples/Writer/Writer.docx)

## License

The license of this framework has been changed from the Microsoft Public License (Ms-PL) to the [MIT License](LICENSE) with version 4.
