using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;
using System.Threading.Tasks;

namespace Test.NewsReader.Applications.Services
{
    [Export(typeof(IMessageService)), Export, Shared]
    public class MockMessageService : IMessageService
    {
        public Action<string> ShowMessageStub { get; set; }

        public void ShowMessage(string message)
        {
            ShowMessageStub?.Invoke(message);
        }

        public Func<string, Task> ShowMessageAsyncDialogStub { get; set; }
        
        public Task ShowMessageDialogAsync(string message)
        {
            return ShowMessageAsyncDialogStub?.Invoke(message) ?? Task.FromResult((object)null);
        }

        public Func<string, Task<bool>> ShowYesNoQuestionDialogAsyncStub { get; set; }

        public Task<bool> ShowYesNoQuestionDialogAsync(string message)
        {
            return ShowYesNoQuestionDialogAsyncStub?.Invoke(message) ?? Task.FromResult(true);
        }
    }
}
