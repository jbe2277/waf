using System.ComponentModel;
using System.Runtime.Serialization;
using System.Waf.Foundation;

namespace Waf.InformationManager.Common.Domain
{
    /// <summary>
    /// Defines the base class for a model with validation support.
    /// </summary>
    [DataContract]
    public abstract class ValidationModel : Model, IDataErrorInfo
    {
        private DataErrorInfoSupport dataErrorInfoSupport;


        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationModel"/> class.
        /// </summary>
        protected ValidationModel()
        {
            Initialize();
        }


        string IDataErrorInfo.Error => dataErrorInfoSupport.Error;

        string IDataErrorInfo.this[string columnName] => dataErrorInfoSupport[columnName];


        private void Initialize()
        {
            dataErrorInfoSupport = new DataErrorInfoSupport(this);
        }
        
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Initialize();
        }
    }
}
