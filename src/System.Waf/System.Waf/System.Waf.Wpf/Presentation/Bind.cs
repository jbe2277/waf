using System.Globalization;
using System.Windows.Data;

namespace System.Waf.Presentation
{
    /// <summary>Culture aware binding, which connects the properties of binding target objects (typically, WPF elements) with any data source. It uses the <see cref="CultureInfo.CurrentCulture"/> for converting the values.</summary>
    public class Bind : Binding
    {
        /// <summary>Initializes a new instance of the <see cref="Bind"/> class.</summary>
        public Bind() : base()
        {
            ConverterCulture = CultureInfo.CurrentCulture;
        }

        /// <summary>Initializes a new instance of the <see cref="Bind"/> class.</summary>
        /// <param name="path">The initial <see cref="Binding.Path"/> for the binding.</param>
        public Bind(string path) : base(path)
        {
            ConverterCulture = CultureInfo.CurrentCulture;
        }
    }
}
