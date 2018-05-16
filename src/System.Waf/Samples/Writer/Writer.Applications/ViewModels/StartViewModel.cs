using System.ComponentModel.Composition;
using System.Waf.Applications;
using Waf.Writer.Applications.Services;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Applications.ViewModels
{
    [Export]
    public class StartViewModel : ViewModel<IStartView>
    {
        [ImportingConstructor]
        public StartViewModel(IStartView view, IFileService fileService) : base(view)
        {
            FileService = fileService;
        }

        public IFileService FileService { get; }
    }
}
