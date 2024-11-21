using System.Data.Common;
using System.Data.SqlClient;

namespace LeeTeke.SqlDo.SqlServer
{
    /// <summary>
    /// SqlServer数据库服务
    /// </summary>
    /// <param name="is2012Greater">版本号是否大于等于2012</param>
    /// <param name="defaultSheetName"></param>
    public class SqlServerService(bool is2012Greater, string defaultSheetName) : SqlService
    {
        public override SQLVersion Version { get; init; } = is2012Greater ? SQLVersion.SqlServer2012H : SQLVersion.SqlServer2008L;
        public override string DefaultSheetName { get; init; } = defaultSheetName;

        private DbConnectionStringBuilder? _connectionSB;
        public DbConnectionStringBuilder? DbConnectionStringBuilder => _connectionSB;

        public override void ConnectInitialization(DbConnectionStringBuilder dbConnectionStringBuilder)
        {
            try
            {
                using var sql = new SqlConnection(dbConnectionStringBuilder.ConnectionString);
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
                using var sql = new SqlConnection(dbConnectionStringBuilder.ConnectionString);
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
                using var sql = new SqlConnection(dbConnectionStringBuilder.ConnectionString);
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
            var connect = new SqlConnection(DbConnectionStringBuilder.ConnectionString);
            connect.Open();
            return connect;
        }

        public override async Task<DbConnection> GetNewConnectionAsync()
        {
            if (DbConnectionStringBuilder == null)
                throw new SqlDoException("未初始化数据库!");
            var connect = new SqlConnection(DbConnectionStringBuilder.ConnectionString);
            await connect.OpenAsync().ConfigureAwait(false);
            return connect;
        }

        public override async Task<DbConnection> GetNewConnectionAsync(CancellationToken cancellationToken)
        {
            if (DbConnectionStringBuilder == null)
                throw new SqlDoException("未初始化数据库!");
            var connect = new SqlConnection(DbConnectionStringBuilder.ConnectionString);
            await connect.OpenAsync(cancellationToken).ConfigureAwait(false);
            return connect;
        }

    }
}
