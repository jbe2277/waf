using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "<Pending>", Scope = "type", Target = "~T:Waf.NewsReader.Presentation.Views.FeedItemView")]
[assembly: SuppressMessage("Security", "CA5350:Do Not Use Weak Cryptographic Algorithms", Justification = "<Pending>", Scope = "member", Target = "~M:Waf.NewsReader.Presentation.Services.DataService.GetHash~System.String")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Waf.NewsReader.Presentation.Services.WebStorageService.TrySilentSignIn~System.Threading.Tasks.Task{System.Boolean}")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Waf.NewsReader.Presentation.Services.SyndicationService.RetrieveFeed(System.Uri)~System.Threading.Tasks.Task{Waf.NewsReader.Applications.Services.FeedDto}")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "Waf.NewsReader.Presentation.Services.WebStorageService")]
