using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Threading;

namespace LeeTeke.SqlDo.MySql
{
    public class MySqlService(string defaultSheetName) : SqlService
    {

        public override SQLVersion Version { get; init; } = SQLVersion.MySql;

        public override string DefaultSheetName { get; init; } = defaultSheetName;

        private DbConnectionStringBuilder? _connectionSB;
        public DbConnectionStringBuilder? DbConnectionStringBuilder => _connectionSB;

        public override void ConnectInitialization(DbConnectionStringBuilder dbConnectionStringBuilder)
        {
            try
            {
                using var sql = new MySqlConnection(dbConnectionStringBuilder.ConnectionString);
                sql.Open();
                _connectionSB = dbConnectionStringBuilder;
                sql.Close();

            }
            catch (Exception ex)
            {
                throw new SqlDoException(string.Join("ConnectInitialization:{0}", ex.Message), ex);
            }
        }
        public override async Task ConnectInitializationAsync(DbConnectionStringBuilder dbConnectionStringBuilder)
        {
            try
            {
                using var sql = new MySqlConnection(dbConnectionStringBuilder.ConnectionString);
                await sql.OpenAsync().ConfigureAwait(false);
                _connectionSB = dbConnectionStringBuilder;
                await sql.CloseAsync().ConfigureAwait(false); 
            }
            catch (Exception ex)
            {

                throw new SqlDoException(string.Join("ConnectInitializationAsync:{0}", ex.Message), ex);
            }




        }
        public override async Task ConnectInitializationAsync(DbConnectionStringBuilder dbConnectionStringBuilder, CancellationToken cancellationToken)
        {

            try
            {
                using var sql = new MySqlConnection(dbConnectionStringBuilder.ConnectionString);
                await sql.OpenAsync(cancellationToken).ConfigureAwait(false);
                _connectionSB = dbConnectionStringBuilder;
                await sql.CloseAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {

                throw new SqlDoException(string.Join("ConnectInitializationAsync:{0}", ex.Message), ex);
            }

        }

        public override DbConnection GetNewConnection()
        {
            if (DbConnectionStringBuilder == null)
                throw new SqlDoException("未初始化数据库!");
            var connect = new MySqlConnection(DbConnectionStringBuilder.ConnectionString);
            connect.Open();
            return connect;
        }

        public override async Task<DbConnection> GetNewConnectionAsync()
        {
            if (DbConnectionStringBuilder == null)
                throw new SqlDoException("未初始化数据库!");
            var connect = new MySqlConnection(DbConnectionStringBuilder.ConnectionString);
            await connect.OpenAsync().ConfigureAwait(false);
            return connect;
        }

        public override async Task<DbConnection> GetNewConnectionAsync(CancellationToken cancellationToken)
        {
            if (DbConnectionStringBuilder == null)
                throw new SqlDoException("未初始化数据库!");
            var connect = new MySqlConnection(DbConnectionStringBuilder.ConnectionString);
            await connect.OpenAsync(cancellationToken).ConfigureAwait(false);
            return connect;
        }




    }
}
