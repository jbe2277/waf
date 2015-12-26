using System.Configuration;
using System.Data.Common;

namespace Waf.BookLibrary.Library.Applications.Data
{
    internal static class DbConnectionFactory
    {
        public static DbConnection CreateConnection(string connectionName)
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionName];

            var factory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);

            DbConnection connection = factory.CreateConnection();

            connection.ConnectionString = connectionStringSettings.ConnectionString;

            return connection;
        }
    }
}
