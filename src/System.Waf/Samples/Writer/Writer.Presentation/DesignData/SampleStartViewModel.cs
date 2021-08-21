using System.Waf.Applications;
using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Presentation.DesignData
{
    public class SampleStartViewModel : StartViewModel
    {
        public SampleStartViewModel() : base(new MockStartView(), new MockFileService())
        {
            ((MockFileService)FileService).RecentFileList = new RecentFileList();
            FileService.RecentFileList.AddFile(@"C:\Users\Admin\My Documents\Document 1.rtf");
            FileService.RecentFileList.AddFile(@"C:\Users\Admin\My Documents\Win Application Framework (WAF).rtf");
            FileService.RecentFileList.AddFile(@"C:\Users\Admin\My Documents\WAF Writer\Readme.rtf");
            FileService.RecentFileList.RecentFiles[0].IsPinned = true;
        }


        private class MockStartView : MockView, IStartView
        {
        }
    }
}
