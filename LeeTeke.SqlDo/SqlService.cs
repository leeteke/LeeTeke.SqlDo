using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LeeTeke.SqlDo
{
    public abstract class SqlService
    {

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns></returns>
        public abstract SQLVersion Version { get; init; }

        /// <summary>
        /// 默认表的名称
        /// </summary>
        public abstract string DefaultSheetName { get; init; }


        private SqlCmd? _cmd;//匹配的命令

        /// <summary>
        /// 获取新的
        /// </summary>
        /// <returns></returns>
        public abstract DbConnection GetNewConnection();

        /// <summary>
        /// 获取新的
        /// </summary>
        /// <returns></returns>
        public abstract Task<DbConnection> GetNewConnectionAsync();
        /// <summary>
        /// 获取新的
        /// </summary>
        /// <returns></returns>
        public abstract Task<DbConnection> GetNewConnectionAsync(CancellationToken cancellationToken);
        /// <summary>
        /// 连接初始化
        /// </summary>
        /// <param name="dbConnectionStringBuilder"></param>
        /// <returns></returns>
        public abstract void ConnectInitialization(DbConnectionStringBuilder dbConnectionStringBuilder);
        /// <summary>
        /// 连接初始化
        /// </summary>
        /// <param name="dbConnectionStringBuilder"></param>
        /// <returns></returns>
        public abstract Task ConnectInitializationAsync(DbConnectionStringBuilder dbConnectionStringBuilder);
        /// <summary>
        /// 连接初始化
        /// </summary>
        /// <param name="dbConnectionStringBuilder"></param>
        /// <returns></returns>
        public abstract Task ConnectInitializationAsync(DbConnectionStringBuilder dbConnectionStringBuilder, CancellationToken cancellationToken);

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns>DbDataReader</returns>
        /// <exception cref="SqlDoException"></exception>
        public DbDataReader ExecuteCmd(string cmd)
        {
            try
            {
                using var myCmd = GetDbCommand(cmd);
                return myCmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new SqlDoException(string.Format("Execute:{0}", cmd), ex);
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task<DbDataReader> ExecuteCmdAsync(string cmd)
        {
            try
            {
                using var mycmd = await GetDbCommandAsync(cmd).ConfigureAwait(false);
                return await mycmd.ExecuteReaderAsync().ConfigureAwait(false); ;

            }
            catch (Exception ex)
            {
                throw new SqlDoException(string.Format("ExecuteAsync:{0}", cmd), ex);
            }
        }
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task<DbDataReader> ExecuteCmdAsync(string cmd, CancellationToken cancellationToken)
        {
            try
            {
                using var mycmd = await GetDbCommandAsync(cmd, cancellationToken).ConfigureAwait(false);
                return await mycmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false); ;
            }
            catch (Exception ex)
            {
                throw new SqlDoException(string.Format("ExecuteAsync:{0}", cmd), ex);
            }
        }


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns>SqlResult</returns>
        /// <exception cref="SqlDoException"></exception>
        public SqlResult Get(string cmd)
        {
            try
            {
                using var read = ExecuteCmd(cmd);
                var result = DbDataReaderToConverter(read);
                return result;
            }
            catch (Exception ex)
            {
                throw new SqlDoException(string.Format("Get:{0}", cmd), ex);
            }
        }


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns>SqlResult</returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task<SqlResult> GetAsync(string cmd)
        {
            try
            {
                using var read = await ExecuteCmdAsync(cmd);
                var result = await DbDataReaderToConverterAsync(read);
                return result;
            }
            catch (Exception ex)
            {
                throw new SqlDoException(string.Format("GetAsync:{0}", cmd), ex);
            }
        }


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="cancellationToken"></param>
        /// <returns>SqlResult</returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task<SqlResult> GetAsync(string cmd, CancellationToken cancellationToken)
        {
            try
            {
                using var read = await ExecuteCmdAsync(cmd, cancellationToken);
                var result = await DbDataReaderToConverterAsync(read, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                throw new SqlDoException(string.Format("GetAsync:{0}", cmd), ex);
            }
        }


        /// <summary>
        /// 设置数据，包括更新，写入与删除
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns>返回执行数量</returns>
        /// <exception cref="SqlDoException"></exception>
        public int Set(string cmd)
        {
            try
            {
                using var myCmd = GetDbCommand(cmd);
                int row = myCmd.ExecuteNonQuery();
                return row;
            }
            catch (Exception ex)
            {
                throw new SqlDoException(string.Format("Set:{0}", cmd), ex);
            }
        }


        /// <summary>
        /// 设置数据，包括更新，写入与删除
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns>返回执行数量</returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task<int> SetAsync(string cmd)
        {
            try
            {

                using var myCmd = await GetDbCommandAsync(cmd).ConfigureAwait(false);
                int row = await myCmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                return row;
            }
            catch (Exception ex)
            {
                throw new SqlDoException(string.Format("SetAsync:{0}", cmd), ex);
            }
        }
        /// <summary>
        /// 设置数据，包括更新，写入与删除
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellation"></param>
        /// <returns>返回执行数量</returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task<int> SetAsync(string cmd, CancellationToken cancellation)
        {
            try
            {

                using var myCmd = await GetDbCommandAsync(cmd, cancellation).ConfigureAwait(false);
                int row = await myCmd.ExecuteNonQueryAsync(cancellation).ConfigureAwait(false);
                return row;
            }
            catch (Exception ex)
            {
                throw new SqlDoException(string.Format("SetAsync:{0}", cmd), ex);
            }
        }

        /// <summary>
        /// 计算数据
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns>数据结果</returns>
        /// <exception cref="SqlDoException"></exception>
        public object? Calculate(string cmd)
        {
            try
            {
                using var read = GetDbCommand(cmd);
                return read.ExecuteScalar();
            }
            catch (Exception ex)
            {

                throw new SqlDoException(string.Format("Calculate:{0}", cmd), ex);
            }
        }


        /// <summary>
        /// 计算数据
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns>数据结果</returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task<object?> CalculateAsync(string cmd)
        {
            try
            {
                using var read = await GetDbCommandAsync(cmd).ConfigureAwait(false);
                return await read.ExecuteScalarAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {

                throw new SqlDoException(string.Format("CalculateAsync:{0}", cmd), ex);
            }
        }

        /// <summary>
        /// 计算数据
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>数据结果</returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task<object?> CalculateAsync(string cmd, CancellationToken cancellationToken)
        {
            try
            {
                using var read = await GetDbCommandAsync(cmd, cancellationToken).ConfigureAwait(false);
                return await read.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

                throw new SqlDoException(string.Format("CalculateAsync:{0}", cmd), ex);
            }
        }



        /// <summary>
        /// 获取帮助方法
        /// *必须设置 DefaultSheetName *否则会异常
        /// </summary>
        /// <returns>返回帮助方法</returns>
        public SqlCmd GetCmd()
        {
            if (DefaultSheetName == null)
                throw new Exception("DefaultSheetName is null !");

            _cmd ??= new SqlCmd(DefaultSheetName, Version);

            return _cmd;
        }

        /// <summary>
        /// DbDataReader转换器
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        /// <exception cref="SqlDoException"></exception>
        public static SqlResult DbDataReaderToConverter(DbDataReader reader)
        {
            try
            {
                SqlResult result = new SqlResult(reader.FieldCount); ;
                while (reader.Read())
                {
                    object[] values = new object[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        result.Header[i] = reader.GetName(i);
                        values[i] = reader.GetValue(i);
                    }
                    result.Add(values);
                    break;
                }
                while (reader.Read())
                {
                    object[] values = new object[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        values[i] = reader.GetValue(i);
                    }
                    result.Add(values);
                }
                return result;
            }
            catch (Exception ex)
            {

                throw new SqlDoException(string.Format("DbDataReaderToConverter:{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// DbDataReader转换器
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        /// <exception cref="SqlDoException"></exception>
        public static async Task<SqlResult> DbDataReaderToConverterAsync(DbDataReader reader)
        {
            try
            {
                SqlResult result = new SqlResult(reader.FieldCount); ;
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    object[] values = new object[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        result.Header[i] = reader.GetName(i);
                        values[i] = reader.GetValue(i);
                    }
                    result.Add(values);
                    break;
                }
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    object[] values = new object[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        values[i] = reader.GetValue(i);
                    }
                    result.Add(values);
                }
                return result;
            }
            catch (Exception ex)
            {

                throw new SqlDoException(string.Format("DbDataReaderToConverterAsync:{0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// DbDataReader转换器
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="SqlDoException"></exception>
        public static async Task<SqlResult> DbDataReaderToConverterAsync(DbDataReader reader, CancellationToken cancellationToken)
        {
            try
            {
                SqlResult result = new SqlResult(reader.FieldCount); ;
                while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    object[] values = new object[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        result.Header[i] = reader.GetName(i);
                        values[i] = reader.GetValue(i);
                    }
                    result.Add(values);
                    break;
                }
                while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    object[] values = new object[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        values[i] = reader.GetValue(i);
                    }
                    result.Add(values);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new SqlDoException(string.Format("DbDataReaderToConverterAsync:{0}", ex.Message), ex);
            }
        }


        protected DbCommand GetDbCommand(string cmd)
        {
            var connection = GetNewConnection();
            var myCmd = connection.CreateCommand();
            myCmd.CommandText = cmd;
            return myCmd;
        }
        protected async Task<DbCommand> GetDbCommandAsync(string cmd)
        {
            var connection = await GetNewConnectionAsync().ConfigureAwait(false);
            var myCmd = connection.CreateCommand();
            myCmd.CommandText = cmd;
            return myCmd;
        }
        protected async Task<DbCommand> GetDbCommandAsync(string cmd, CancellationToken cancellationToken)
        {
            var connection = await GetNewConnectionAsync(cancellationToken).ConfigureAwait(false);
            var myCmd = connection.CreateCommand();
            myCmd.CommandText = cmd;
            return myCmd;
        }
    }
}
