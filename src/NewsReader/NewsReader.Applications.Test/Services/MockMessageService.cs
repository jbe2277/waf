using Jbe.NewsReader.Applications.Services;
using System;
using System.Composition;
using System.Threading.Tasks;

namespace Test.NewsReader.Applications.Services
{
    [Export(typeof(IMessageService)), Export, Shared]
    public class MockMessageService : IMessageService
    {
        public Func<string, Task<bool>> ShowYesNoQuestionAsyncStub { get; set; }


        public Task<bool> ShowYesNoQuestionAsync(string message)
        {
            return ShowYesNoQuestionAsyncStub?.Invoke(message) ?? Task.FromResult(true);
        }
    }
}
