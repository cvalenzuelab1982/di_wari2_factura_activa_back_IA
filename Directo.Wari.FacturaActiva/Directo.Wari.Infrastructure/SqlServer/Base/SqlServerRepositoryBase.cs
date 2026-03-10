using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Directo.Wari.Infrastructure.SqlServer.Base
{
    public abstract class SqlServerRepositoryBase
    {
        protected readonly string ConnectionString;

        protected SqlServerRepositoryBase(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("LegacyConnection")!;
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
