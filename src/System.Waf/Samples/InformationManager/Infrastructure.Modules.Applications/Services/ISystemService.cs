namespace Waf.InformationManager.Infrastructure.Modules.Applications.Services;

public interface ISystemService
{
    string DataDirectory { get; }

    /// <summary>Commit UI changes (e.g. of TextBox).</summary>
    void CommitUIChanges();
}
