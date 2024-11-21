using System.Collections;
using System.Data.SqlTypes;

namespace LeeTeke.SqlDo
{
    public class SqlCmd
    {
        #region 静态
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="sheet">表名</param>
        /// <param name="data">名称与值</param>
        /// <returns></returns>
        public static string Create(string sheet, Dictionary<string, object> data)
        {
            string keys = string.Join(',', data.Select(p => p.Key));

            string values = string.Join(',', data.Select(p => GetValueString(p.Value)));

            return $"INSERT INTO {sheet} ({keys}) VALUES ({values})"; ;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sheet">表名</param>
        /// <param name="and">And参数</param>
        /// <returns></returns>
        public static string Delete(string sheet, SqlConditionAND and)
        {
            return $"DELETE  FROM {sheet} WHERE ({and})";
        }

        /// <summary>
        /// 更新数据   
        /// </summary>
        /// <param name="sheet">表名</param>
        /// <param name="and">and参数</param>
        /// <param name="or">or参数</param>
        /// <param name="keyValue">属性与值</param>
        /// <returns></returns>
        public static string Update(string sheet, SqlConditionAND? and, SqlConditionOR? or, Dictionary<string, object> keyValue)
        {
            var whats = KeyValueToString(keyValue);

            var where = WhereSplicing(and, or);

            return $"UPDATE {sheet} SET {whats}{where}";
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sheet">表名</param>
        /// <param name="and">and参数</param>
        /// <param name="or">or参数</param>
        /// <param name="sort">排序</param>
        /// <param name="keys">属性名</param>
        /// <param name="maximum">最大获取量，0为无限制</param>
        /// <param name="ver">数据库版本</param>
        /// <returns></returns>
        public static string Get(string sheet, SqlConditionAND? and, SqlConditionOR? or, SqlSortArgs? sort, string[] keys, ulong maximum, SQLVersion ver)
        {
            var where = WhereSplicing(and, or);
            var keyStr = KeysToString(keys);
            if (maximum < 1)
            {
                return $"SELECT {keyStr} FROM {sheet}{where}{sort}";
            }
            return ver switch
            {
                SQLVersion.SqlServer2012H or SQLVersion.SqlServer2008L => $"SELECT TOP {maximum} {keyStr} FROM {sheet}{where}{sort}",
                SQLVersion.MySql or SQLVersion.SQLite => $"SELECT {keyStr} FROM {sheet}{where}{sort} LIMIT {maximum}",
                _ => "Erro",
            };
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sheet">表名</param>
        /// <param name="and">and参数</param>
        /// <param name="or">or参数</param>
        /// <param name="sort">排序</param>
        /// <param name="keys">属性名</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页多少</param>
        /// <param name="ver">版本号</param>
        /// <returns></returns>
        public static string Paging(string sheet, SqlConditionAND? and, SqlConditionOR? or, SqlSortArgs? sort, string[] keys, ulong pageIndex, ulong pageSize, SQLVersion ver)
        {
            var where = WhereSplicing(and, or);
            var keyStr = KeysToString(keys);

            return ver switch
            {
                SQLVersion.SqlServer2012H => $"SELECT {keyStr} FROM {sheet}{where}{sort} OFFSET {(pageIndex - 1) * pageSize}  ROWS FETCH NEXT {pageSize} ROWS ONLY",
                SQLVersion.SqlServer2008L => $"SELECT TOP {pageSize} {keyStr} FROM (SELECT ROW_Number() Over({sort}) AS RowNumber,* FROM {sheet} ) Temp_ROW {where} AND RowNumber>{(pageIndex - 1) * pageSize}",
                SQLVersion.MySql => $"SELECT {keyStr} FROM {sheet}{where}{sort} LIMIT {(pageIndex - 1) * pageSize},{pageSize}",
                SQLVersion.SQLite => $"SELECT {keyStr} FROM {sheet}{where}{sort} LIMIT {(pageIndex - 1) * pageSize} offset {pageSize}",
                _ => "Erro",
            };
        }

        /// <summary>
        /// 分页命令拼接法
        /// </summary>
        /// <param name="selectCmd"></param>
        /// <param name="fromCmd"></param>
        /// <param name="whereCmd"></param>
        /// <param name="sortCmd"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="ver"></param>
        /// <returns></returns>
        public static string PagingCmdFormat(string selectCmd, string fromCmd, string? whereCmd, string? sortCmd, ulong pageIndex, ulong pageSize, SQLVersion ver)
        {
            return ver switch
            {
                SQLVersion.SqlServer2012H => $"SELECT {selectCmd} FROM {fromCmd} {whereCmd} {sortCmd} OFFSET {(pageIndex - 1) * pageSize}  ROWS FETCH NEXT {pageSize} ROWS ONLY",
                SQLVersion.SqlServer2008L => $"SELECT TOP {pageSize} {selectCmd} FROM (SELECT ROW_Number() Over({sortCmd}) AS RowNumber,* FROM {fromCmd} ) Temp_ROW {whereCmd} AND RowNumber>{(pageIndex - 1) * pageSize}",
                SQLVersion.MySql => $"SELECT {selectCmd} FROM {fromCmd} {whereCmd} {sortCmd} LIMIT {(pageIndex - 1) * pageSize},{pageSize}",
                SQLVersion.SQLite => $"SELECT {selectCmd} FROM {fromCmd} {whereCmd} {sortCmd} LIMIT {(pageIndex - 1) * pageSize} offset {pageSize}",
                _ => "Erro",
            };
        }

        /// <summary>
        /// 其它功能
        /// </summary>
        /// <param name="sheet">表名</param>
        /// <param name="functions">功能</param>
        /// <param name="key">属性</param>
        /// <param name="and">and参数</param>
        /// <param name="or">or参数</param>
        /// <returns></returns>
        public static string Functions(string sheet, SqlFunctions functions, string key, SqlConditionAND? and, SqlConditionOR? or)
        {
            var where = WhereSplicing(and, or);
            return $"SELECT {GetFunctionsEnum(functions)}({key}) FROM {sheet}{where}";
        }

        /// <summary>
        /// 分组统计
        /// </summary>
        /// <param name="sheet">表名</param>
        /// <param name="and">and参数</param>
        /// <param name="or">or参数</param>
        /// <param name="keys">属性名</param>
        /// <returns></returns>
        public static string GroupBy(string sheet, string[] keys, SqlConditionAND? and, SqlConditionOR? or)
        {
            var where = WhereSplicing(and, or);
            var keyStr = KeysToString(keys);

            return $"SELECT {keyStr} FROM {sheet}{where} GROUP BY {keyStr}";
        }


        /// <summary>
        /// 是否存在，简化版本的获取
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="and"></param>
        /// <param name="or"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string Exist(string sheet, SqlConditionAND? and, SqlConditionOR? or, SQLVersion ver)
        {
            return SqlCmd.Get(sheet, and, or, null, SqlKeys.AllKeys, 1, ver);
        }
        #endregion

        #region Helper
        internal static string KeyValueToString(Dictionary<string, object> keyValue)
        {
            return string.Join(',', keyValue.Select(p => $"{p.Key} = {GetValueString(p.Value)}"));
        }

        internal static string[]? ConditionToList(SqlCondition? condition)
        {
            if (condition == null || condition.Count < 1)
            {
                return null;
            }
            string[] reulst = new string[condition.Count];
            for (int i = 0; i < condition.Count; i++)
            {
                if (condition[i].perator == SqlOperator.OnlyFormula)
                {
                    reulst[i] = GetValueString(condition[i].value);
                    continue;
                }
                if (condition[i].key != null && condition[i].value != null)
                {
                    reulst[i] = $"{condition[i].key} {GetOperator(condition[i].perator)} {GetValueString(condition[i].value)}";
                }

            }
            return reulst;
        }


        internal static string KeysToString(string[] keys)
        {
            //if (keys == null && keys.Length == 0)
            //    return "";
            //if (keys.Contains(SqlKeys.All.First()))
            //{
            //    return SqlKeys.All.First();
            //}
            return string.Join(",", keys);
        }

        public static string WhereSplicing(SqlConditionAND? and, SqlConditionOR? or)
        {
            string? andStr, orStr;

            andStr = and?.ToString();
            if (!string.IsNullOrWhiteSpace(andStr))
                andStr = $" WHERE ({andStr})";

            orStr = or?.ToString();
            if (!string.IsNullOrWhiteSpace(orStr))
                if (string.IsNullOrWhiteSpace(andStr))
                    orStr = $" WHERE ({orStr})";
                else
                    orStr = $" AND ({orStr})";

            return $"{andStr}{orStr}";
        }



        /// <summary>
        /// 获取操作符
        /// </summary>
        /// <param name="perator"></param>
        /// <returns></returns>
        public static string GetOperator(SqlOperator perator) => perator switch
        {
            SqlOperator.Equal => "=",
            SqlOperator.NotEqual => "<>",
            SqlOperator.Greater => ">",
            SqlOperator.Less => "<",
            SqlOperator.GreaterEQ => ">=",
            SqlOperator.LessEQ => "<=",
            SqlOperator.Between => "BETWEEN",
            SqlOperator.Like => "LIKE",
            SqlOperator.NotNetween => "NOT BETWEEN",
            SqlOperator.NotLike => "NOT LIKE",
            SqlOperator.In => "IN",
            SqlOperator.NotIn => "NOT IN",
            _ => "=",
        };

        /// <summary>
        /// 获取value值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetValueString(object value) => value switch
        {
            string p => SqlAntiInjection.SecurityStr(p).security ? $"'{p}'" : throw new Exception($"非法值 {p}"),
            SqlString p => $"'{p}'",
            int p => $"{p}",
            long p => $"{p}",
            float p => $"{p}",
            double p => $"{p}",
            decimal p => $"{p}",
            Enum p => $"{p.GetHashCode()}",
            DateTime p => $"'{p:yyyy-MM-dd HH:mm:ss.fff}'",
            SqlFormulaType p => p.Value,
            SqlFunctionValue p => p.ToString(),
            IEnumerable p => $"({IEnumerableConver(p)})",
            _ => $"{value}",
        };

        internal static string IEnumerableConver(IEnumerable value)
        {
            List<string> result = new List<string>();
            foreach (var item in value)
            {
                result.Add(GetValueString(item));
            }
            return string.Join(',', result);
        }

        public static string GetFunctionsEnum(SqlFunctions what) => what switch
        {
            SqlFunctions.Count => "COUNT",
            SqlFunctions.Sum => "SUM",
            SqlFunctions.Avg => "AVG",
            SqlFunctions.Max => "MAX",
            SqlFunctions.Min => "Min",
            SqlFunctions.First => "FIRST",
            SqlFunctions.Last => "LAST",
            SqlFunctions.Group_Concat => "GROUP_CONCAT",
            SqlFunctions.ASCII => "ASCII",
            SqlFunctions.Char => "CHAR",
            SqlFunctions.CharIndex => "CHARINDEX",
            SqlFunctions.Difference => "DIFFERENCE",
            SqlFunctions.Left => "LEFT",
            SqlFunctions.Right => "RIGHT",
            SqlFunctions.Len => "LEN",
            SqlFunctions.Length => "LENGTH",
            SqlFunctions.Lower => "LOWER",
            SqlFunctions.Upper => "UPPER",
            SqlFunctions.LTrim => "LTRIM",
            SqlFunctions.RTrim => "RTRIM",
            SqlFunctions.NChar => "NCHAR",
            SqlFunctions.PatIndex => "PATINDEX",
            SqlFunctions.QuoteName => "QUOTENAME",
            SqlFunctions.Replace => "REPLACE",
            SqlFunctions.Replicate => "REPLICATE",
            SqlFunctions.Reverse => "REVERSE",
            SqlFunctions.Space => "SPACE",
            SqlFunctions.Stuff => "STUFF",
            SqlFunctions.SubString => "SUBSTRING",
            SqlFunctions.Concat => "CONCAT",

            _ => "COUNT"
        };
        #endregion

        #region 实例

        public string Sheet { get; init; }
        public SQLVersion Version { get; init; }

        public SqlCmd(string sheet, SQLVersion ver)
        {
            if (string.IsNullOrWhiteSpace(sheet))
                throw new Exception("sheet 不能为空");
            Sheet = sheet;
            Version = ver;
        }

        #region 创建
        public string Create(Dictionary<string, object> data) => SqlCmd.Create(Sheet, data);
        #endregion

        #region 删除
        public string Delete(SqlConditionAND and) => SqlCmd.Delete(Sheet, and);

        public string Delete(string key, object value) => SqlCmd.Delete(Sheet, new SqlConditionAND() { new(key, value, SqlOperator.Equal) });
        #endregion

        #region 更新
        public string Update(SqlConditionAND? and, SqlConditionOR? or, Dictionary<string, object> keyValue) => SqlCmd.Update(Sheet, and, or, keyValue);

        public string Update(SqlConditionAND? and, Dictionary<string, object> keyValue) => SqlCmd.Update(Sheet, and, null, keyValue);

        public string Update(string key, object value, Dictionary<string, object> keyValue) => SqlCmd.Update(Sheet, new SqlConditionAND { new(key, value, SqlOperator.Equal) }, null, keyValue);
        #endregion


        #region 获取

        public string Get(SqlConditionAND? and, SqlConditionOR? or, SqlSortArgs? sort, string[] keys, ulong maximum) => SqlCmd.Get(Sheet, and, or, sort, keys, maximum, Version);
        public string Get(string key, object value, string[] keys, SqlSortArgs? sort = null) => SqlCmd.Get(Sheet, new SqlConditionAND { new(key, value, SqlOperator.Equal) }, null, sort, keys, 0, Version);
        public string GetFirst(SqlConditionAND? and, string[] keys, SqlSortArgs? sort = null) => SqlCmd.Get(Sheet, and, null, sort, keys, 1, Version);
        public string GetFirst(string key, object value, string[] keys, SqlSortArgs? sort = null) => SqlCmd.Get(Sheet, new SqlConditionAND { new(key, value, SqlOperator.Equal) }, null, sort, keys, 1, Version);
        public string GetFirst(string key, object value, SqlSortArgs? sort = null) => SqlCmd.Get(Sheet, new SqlConditionAND { new(key, value, SqlOperator.Equal) }, null, sort, SqlKeys.AllKeys, 1, Version);
        #endregion

        #region 分页

        public string Paging(SqlConditionAND? and, SqlConditionOR? or, SqlSortArgs? sort, string[] keys, ulong pageIndex, ulong pageSize) => SqlCmd.Paging(Sheet, and, or, sort, keys, pageIndex, pageSize, Version);

        public string Paging(SqlConditionAND? and, SqlSortArgs? sort, string[] keys, ulong pageIndex, ulong pageSize) => SqlCmd.Paging(Sheet, and, null, sort, keys, pageIndex, pageSize, Version);

        public string Paging(SqlSortArgs? sort, string[] keys, ulong pageIndex, ulong pageSize) => SqlCmd.Paging(Sheet, null, null, sort, keys, pageIndex, pageSize, Version);

        public string Paging(string[] keys, ulong pageIndex, ulong pageSize) => SqlCmd.Paging(Sheet, null, null, null, keys, pageIndex, pageSize, Version);
        public string Paging(string key, string value, SqlSortArgs? sort, string[] keys, ulong pageIndex, ulong pageSize) => SqlCmd.Paging(Sheet, new SqlConditionAND { new(key, value, SqlOperator.Equal) }, null, sort, keys, pageIndex, pageSize, Version);

        public string PagingCmdFormat(string selectCmd, string fromCmd, string? whereCmd, string? sortCmd, ulong pageIndex, ulong pageSize) => SqlCmd.PagingCmdFormat(selectCmd, fromCmd, whereCmd, sortCmd, pageIndex, pageSize, Version);

        #endregion

        #region Function功能

        public string Functions(SqlFunctions functions, string key, SqlConditionAND? and, SqlConditionOR? or) => SqlCmd.Functions(Sheet, functions, key, and, or);

        public string Functions(SqlFunctions functions, string key) => SqlCmd.Functions(Sheet, functions, key, null, null);
        #endregion

        #region GroupBy

        public string GroupBy(string[] keys, SqlConditionAND? and, SqlConditionOR? or) => SqlCmd.GroupBy(Sheet, keys, and, or);
        public string GroupBy(string[] keys) => SqlCmd.GroupBy(Sheet, keys, null, null);
        public string GroupBy(string key) => SqlCmd.GroupBy(Sheet, new string[] { key }, null, null);
        #endregion



        #region Exist

        public string Exist(SqlConditionAND? and, SqlConditionOR? or) => SqlCmd.Exist(Sheet, and, or, Version);

        public string Exist(string key, object value) => SqlCmd.Exist(Sheet, new SqlConditionAND { new(key, value, SqlOperator.Equal) }, null, Version);
        #endregion

        #endregion
    }
}
