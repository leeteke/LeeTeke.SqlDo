using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace LeeTeke.SqlDo.SQLite
{
    public class SQLiteService(string defaultSheetName) : SqlService
    {
        public override SQLVersion Version { get; init; } = SQLVersion.SQLite;

        public override string DefaultSheetName { get; init; } = defaultSheetName;

        private DbConnectionStringBuilder? _connectionSB;
        public DbConnectionStringBuilder? DbConnectionStringBuilder => _connectionSB;

        public override void ConnectInitialization(DbConnectionStringBuilder dbConnectionStringBuilder)
        {
            try
            {
                using var sql = new SqliteConnection(dbConnectionStringBuilder.ConnectionString);
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
                using var sql = new SqliteConnection(dbConnectionStringBuilder.ConnectionString);
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
                using var sql = new SqliteConnection(dbConnectionStringBuilder.ConnectionString);
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
            var connect = new SqliteConnection(DbConnectionStringBuilder.ConnectionString);
            connect.Open();
            return connect;
        }

        public override async Task<DbConnection> GetNewConnectionAsync()
        {
            if (DbConnectionStringBuilder == null)
                throw new SqlDoException("未初始化数据库!");
            var connect = new SqliteConnection(DbConnectionStringBuilder.ConnectionString);
            await connect.OpenAsync().ConfigureAwait(false);
            return connect;
        }

        public override async Task<DbConnection> GetNewConnectionAsync(CancellationToken cancellationToken)
        {
            if (DbConnectionStringBuilder == null)
                throw new SqlDoException("未初始化数据库!");
            var connect = new SqliteConnection(DbConnectionStringBuilder.ConnectionString);
            await connect.OpenAsync(cancellationToken).ConfigureAwait(false);
            return connect;
        }



        #region SQLite专用


        /// <summary>
        /// 加密数据库
        /// </summary>
        /// <param name="newDbName"></param>
        /// <param name="pwd"></param>
        /// <param name="newDbPath"></param>
        /// <exception cref="SqlDoException"></exception>
        public void EncryptDb(string newDbName, string pwd, string? newDbPath = null)
        {
            try
            {
                if (!SqlAntiInjection.SecurityStr(pwd).security)
                    throw new SqlDoException(string.Join("不符合标准的值：{0}", pwd));

                using var dbCmd = GetDbCommand(
                    $"ATTACH DATABASE '{newDbPath}{newDbName}.db' AS {newDbName} KEY '{pwd}';" +
                    $"SELECT sqlcipher_export('{newDbName}');" +
                    $"DETACH DATABASE {newDbName};"
                );
                dbCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new SqlDoException("EncryptDb", ex);
            }

        }

        /// <summary>
        /// 加密数据库
        /// </summary>
        /// <param name="newDbName"></param>
        /// <param name="pwd"></param>
        /// <param name="newDbPath"></param>
        /// <returns></returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task EncryptDbAsync(string newDbName, string pwd, string? newDbPath = null)
        {
            try
            {
                if (!SqlAntiInjection.SecurityStr(pwd).security)
                    throw new SqlDoException(string.Join("不符合标准的值：{0}", pwd));

                using var dbCmd = await GetDbCommandAsync(
                    $"ATTACH DATABASE '{newDbPath}{newDbName}.db' AS {newDbName} KEY '{pwd}';" +
                    $"SELECT sqlcipher_export('{newDbName}');" +
                    $"DETACH DATABASE {newDbName};"
                ).ConfigureAwait(false);
                await dbCmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SqlDoException("EncryptDbAsync", ex);
            }

        }


        /// <summary>
        /// 加密数据库
        /// </summary>
        /// <param name="newDbName"></param>
        /// <param name="pwd"></param>
        /// <param name="newDbPath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task EncryptDbAsync(string newDbName, string pwd, string? newDbPath, CancellationToken cancellationToken)
        {
            try
            {
                if (!SqlAntiInjection.SecurityStr(pwd).security)
                    throw new SqlDoException(string.Join("不符合标准的值：{0}", pwd));

                using var dbCmd = await GetDbCommandAsync(
                    $"ATTACH DATABASE '{newDbPath}{newDbName}.db' AS {newDbName} KEY '{pwd}';" +
                    $"SELECT sqlcipher_export('{newDbName}');" +
                    $"DETACH DATABASE {newDbName};"
                    , cancellationToken
                ).ConfigureAwait(false);
                await dbCmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SqlDoException("EncryptDbAsync", ex);
            }

        }



        /// <summary>
        /// 解密加密数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="dbPath"></param>
        /// <exception cref="SqlDoException"></exception>
        public void DecryptDb(string dbName, string? dbPath = null)
        {
            try
            {
                using var dbCmd = GetDbCommand(
                   $"ATTACH DATABASE '{dbPath}{dbName}.db' AS {dbName} KEY '';" +
                   $"SELECT sqlcipher_export('{dbName}');" +
                   $"DETACH DATABASE {dbName};"
                );
                dbCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new SqlDoException("DecryptDb", ex);
            }

        }

        /// <summary>
        /// 解密加密数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="dbPath"></param>
        /// <returns></returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task DecryptDbAsync(string dbName, string? dbPath = null)
        {
            try
            {

                using var dbCmd = await GetDbCommandAsync(
                    $"ATTACH DATABASE '{dbPath}{dbName}.db' AS {dbName} KEY '';" +
                   $"SELECT sqlcipher_export('{dbName}');" +
                   $"DETACH DATABASE {dbName};"
                   ).ConfigureAwait(false);
                await dbCmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SqlDoException("DecryptDbAsync", ex);
            }

        }


        /// <summary>
        /// 解密加密数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="dbPath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task DecryptDbAsync(string dbName, string? dbPath, CancellationToken cancellationToken)
        {
            try
            {


                using var dbCmd = await GetDbCommandAsync(
                   $"ATTACH DATABASE '{dbPath}{dbName}.db' AS {dbName} KEY '';" +
                   $"SELECT sqlcipher_export('{dbName}');" +
                   $"DETACH DATABASE {dbName};"
                    , cancellationToken
                ).ConfigureAwait(false);
                await dbCmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SqlDoException("DecryptDbAsync", ex);
            }

        }

      

        /// <summary>
        /// 重置加密数据库的密码
        /// </summary>
        /// <param name="newPwd"></param>
        /// <exception cref="SqlDoException"></exception>
        public void ResetDbPassword(string newPwd)
        {
            try
            {
                if (!SqlAntiInjection.SecurityStr(newPwd).security)
                    throw new SqlDoException(string.Join("不符合标准的值：{0}", newPwd));

                using var dbCmd = GetDbCommand($"PRAGMA rekey = '{newPwd}'");
                dbCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new SqlDoException("ResetDbPassword", ex);
            }

        }

        /// <summary>
        /// 重置加密数据库的密码
        /// </summary>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task ResetDbPasswordAsync(string newPwd)
        {
            try
            {
                if (!SqlAntiInjection.SecurityStr(newPwd).security)
                    throw new SqlDoException(string.Join("不符合标准的值：{0}", newPwd));

                using var dbCmd = await GetDbCommandAsync($"PRAGMA rekey = '{newPwd}'").ConfigureAwait(false);
                await dbCmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SqlDoException("ResetDbPasswordAsync", ex);
            }

        }


        /// <summary>
        /// 重置加密数据库的密码
        /// </summary>
        /// <param name="newPwd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task ResetDbPasswordAsync(string newPwd, CancellationToken cancellationToken)
        {
            try
            {
                if (!SqlAntiInjection.SecurityStr(newPwd).security)
                    throw new SqlDoException(string.Join("不符合标准的值：{0}", newPwd));

                using var dbCmd = await GetDbCommandAsync($"PRAGMA rekey = '{newPwd}'", cancellationToken
                ).ConfigureAwait(false);
                await dbCmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SqlDoException("ResetDbPasswordAsync", ex);
            }

        }

        #endregion
    }
}
