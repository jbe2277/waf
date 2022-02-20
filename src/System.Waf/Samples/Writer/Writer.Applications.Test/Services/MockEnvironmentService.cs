using System.ComponentModel.Composition;
using Waf.Writer.Applications.Services;

namespace Test.Writer.Applications.Services;

[Export(typeof(IEnvironmentService)), Export]
public class MockEnvironmentService : IEnvironmentService
{
    public string? DocumentFileName { get; set; }
}
