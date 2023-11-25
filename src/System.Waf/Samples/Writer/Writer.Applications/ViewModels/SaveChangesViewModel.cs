using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.Writer.Applications.Documents;
using Waf.Writer.Applications.Views;

namespace Waf.Writer.Applications.ViewModels;

[Export, PartCreationPolicy(CreationPolicy.NonShared)]
public class SaveChangesViewModel : ViewModel<ISaveChangesView>
{
    private readonly DelegateCommand yesCommand;
    private readonly DelegateCommand noCommand;
    private bool? result;

    [ImportingConstructor]
    public SaveChangesViewModel(ISaveChangesView view) : base(view)
    {
        yesCommand = new(() => Close(true));
        noCommand = new(() => Close(false));
    }

    public string Title => ApplicationInfo.ProductName;

    public ICommand YesCommand => yesCommand;

    public ICommand NoCommand => noCommand;

    public IReadOnlyList<IDocument> Documents { get; set; } = [];

    public bool? ShowDialog(object owner)
    {
        ViewCore.ShowDialog(owner);
        return result;
    }

    private void Close(bool? dialogResult)
    {
        result = dialogResult;
        ViewCore.Close();
    }
}
