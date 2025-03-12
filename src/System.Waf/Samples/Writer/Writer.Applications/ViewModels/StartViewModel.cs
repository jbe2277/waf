using System.Waf.Applications;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Applications.ViewModels;

public class StartViewModel(IStartView view, IFileService fileService) : ViewModel<IStartView>(view)
{
    public IFileService FileService { get; } = fileService;
}
