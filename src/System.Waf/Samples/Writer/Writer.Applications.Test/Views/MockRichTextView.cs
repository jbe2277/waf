using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;

namespace Test.Writer.Applications.Views;

[Export(typeof(IRichTextView)), PartCreationPolicy(CreationPolicy.NonShared)]
public class MockRichTextView : MockView<RichTextViewModel>, IRichTextView
{
}
