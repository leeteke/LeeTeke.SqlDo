using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
        /// 获取数据
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns>SqlResult</returns>
        /// <exception cref="SqlDoException"></exception>
        public SqlResult Get(string cmd)
        {
            try
            {
                using var connection = GetNewConnection();
                using var myCmd = connection.CreateCommand();
                myCmd.CommandText = cmd;
                using var read = myCmd.ExecuteReader();
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
                using var connection = await GetNewConnectionAsync().ConfigureAwait(false);
                using var myCmd = connection.CreateCommand();
                myCmd.CommandText = cmd;
                using var read = await myCmd.ExecuteReaderAsync().ConfigureAwait(false); ;
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

                using var connection = await GetNewConnectionAsync(cancellationToken).ConfigureAwait(false);
                using var myCmd = connection.CreateCommand();
                myCmd.CommandText = cmd;
                using var read = await myCmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                var result = await DbDataReaderToConverterAsync(read, cancellationToken);
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
        /// <param name="mapper"></param>
        /// <returns>SqlResult</returns>
        /// <exception cref="SqlDoException"></exception>
        public List<T> Get<T>(string cmd, Func<SqlDataMapper<T>, SqlDataMapper<T>> mapper) where T : new()
        {
            try
            {
                using var connection = GetNewConnection();
                using var myCmd = connection.CreateCommand();
                myCmd.CommandText = cmd;
                using var read = myCmd.ExecuteReader();
                var result = DbDataReaderToConverter(read, mapper);
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
        /// <param name="mapper"></param>
        /// <returns>SqlResult</returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task<List<T>> GetAsync<T>(string cmd, Func<SqlDataMapper<T>, SqlDataMapper<T>> mapper) where T : new()
        {
            try
            {
                using var connection = await GetNewConnectionAsync().ConfigureAwait(false);
                using var myCmd = connection.CreateCommand();
                myCmd.CommandText = cmd;
                using var read = await myCmd.ExecuteReaderAsync().ConfigureAwait(false); ;
                var result = await DbDataReaderToConverterAsync<T>(read, mapper);
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
        /// <param name="mapper"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>SqlResult</returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task<List<T>> GetAsync<T>(string cmd, Func<SqlDataMapper<T>, SqlDataMapper<T>> mapper, CancellationToken cancellationToken) where T : new()
        {
            try
            {
                using var connection = await GetNewConnectionAsync(cancellationToken).ConfigureAwait(false);
                using var myCmd = connection.CreateCommand();
                myCmd.CommandText = cmd;
                using var read = await myCmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false); ;
                var result = await DbDataReaderToConverterAsync(read, mapper, cancellationToken);
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
                using var connection = GetNewConnection();
                using var myCmd = connection.CreateCommand();
                myCmd.CommandText = cmd;
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

                using var connection = await GetNewConnectionAsync().ConfigureAwait(false);
                using var myCmd = connection.CreateCommand();
                myCmd.CommandText = cmd;
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
        /// <param name="cancellationToken"></param>
        /// <returns>返回执行数量</returns>
        /// <exception cref="SqlDoException"></exception>
        public async Task<int> SetAsync(string cmd, CancellationToken cancellationToken)
        {
            try
            {

                using var connection = await GetNewConnectionAsync(cancellationToken).ConfigureAwait(false);
                using var myCmd = connection.CreateCommand();
                myCmd.CommandText = cmd;
                int row = await myCmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
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
                using var connection = GetNewConnection();
                using var myCmd = connection.CreateCommand();
                myCmd.CommandText = cmd;
                return myCmd.ExecuteScalar();
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
                using var connection = await GetNewConnectionAsync().ConfigureAwait(false);
                using var myCmd = connection.CreateCommand();
                myCmd.CommandText = cmd;
                var numberr = await myCmd.ExecuteScalarAsync().ConfigureAwait(false);
                return numberr;
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
                using var connection = await GetNewConnectionAsync(cancellationToken).ConfigureAwait(false);
                using var myCmd = connection.CreateCommand();
                myCmd.CommandText = cmd;
                var number = await myCmd.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
                return number;
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



        /// <summary>
        /// DbDataReader转换器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        /// <exception cref="SqlDoException"></exception>
        public static List<T> DbDataReaderToConverter<T>(DbDataReader reader, Func<SqlDataMapper<T>, SqlDataMapper<T>> mapper) where T : new()
        {
            try
            {
                List<T> result = [];

                var sqlMapper = mapper(new SqlDataMapper<T>());

                string[]? keys = null;
                object[]? firstRowData = null;

                while (reader.Read())
                {
                    keys = new string[reader.FieldCount];
                    firstRowData = new object[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        keys[i] = reader.GetName(i);
                        firstRowData[i] = reader.GetValue(i);
                    }
                    break;
                }

                if (keys != null)
                {

                    sqlMapper.SetKeys(keys);

                    var firstData = new T();

                    for (int i = 0; i < firstRowData!.Length; i++)
                    {
                        sqlMapper.SetValue(firstData, i, firstRowData[i]);
                    }
                    result.Add(firstData);

                    while (reader.Read())
                    {

                        var nextData = new T();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {

                            sqlMapper.SetValue(nextData, i, reader.GetValue(i));

                        }
                        result.Add(nextData);
                    }
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
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        /// <exception cref="SqlDoException"></exception>
        public static async Task<List<T>> DbDataReaderToConverterAsync<T>(DbDataReader reader, Func<SqlDataMapper<T>, SqlDataMapper<T>> mapper) where T : new()
        {
            try
            {

                List<T> result = [];

                var sqlMapper = mapper(new SqlDataMapper<T>());

                string[] keys = null!;
                object[] firstRowData = null!;

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    keys = new string[reader.FieldCount];
                    firstRowData = new object[reader.FieldCount]; firstRowData = new object[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        keys[i] = reader.GetName(i);
                        firstRowData[i] = reader.GetValue(i);
                    }
                    break;
                }



                if (keys != null)
                {
                    sqlMapper.SetKeys(keys);
                    var firstData = new T();

                    for (int i = 0; i < firstRowData.Length; i++)
                    {
                        sqlMapper.SetValue(firstData, i, firstRowData[i]);
                    }
                    result.Add(firstData);

                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {

                        var nextData = new T();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {

                            sqlMapper.SetValue(nextData, i, reader.GetValue(i));

                        }
                        result.Add(nextData);
                    }
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
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="mapper"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="SqlDoException"></exception>
        public static async Task<List<T>> DbDataReaderToConverterAsync<T>(DbDataReader reader, Func<SqlDataMapper<T>, SqlDataMapper<T>> mapper, CancellationToken cancellationToken) where T : new()
        {
            try
            {
                List<T> result = [];

                var sqlMapper = mapper(new SqlDataMapper<T>());

                string[] keys = null!;
                object[] firstRowData = null!;

                while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    keys = new string[reader.FieldCount];
                    firstRowData = new object[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        keys[i] = reader.GetName(i);
                        firstRowData[i] = reader.GetValue(i);
                    }
                    break;
                }




                if (keys != null)
                {
                    sqlMapper.SetKeys(keys);

                    var firstData = new T();

                    for (int i = 0; i < firstRowData.Length; i++)
                    {
                        sqlMapper.SetValue(firstData, i, firstRowData[i]);
                    }
                    result.Add(firstData);

                    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                    {

                        var nextData = new T();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {

                            sqlMapper.SetValue(nextData, i, reader.GetValue(i));

                        }
                        result.Add(nextData);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {

                throw new SqlDoException(string.Format("DbDataReaderToConverterAsync:{0}", ex.Message), ex);
            }
        }
    }
}
