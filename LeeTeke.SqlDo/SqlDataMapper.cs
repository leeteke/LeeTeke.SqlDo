using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LeeTeke.SqlDo
{
    public class SqlDataMapper<T> where T : new()
    {

        private readonly Dictionary<string, Action<T, object>> _temp = [];

        private Action<T, object>?[] _sets = null!;

        private MapperAutoMode? _mode = null;


        private readonly List<PropertyInfo> _propertyInfos;

        public SqlDataMapper()
        {
            _propertyInfos = typeof(T).GetProperties().ToList();
        }

        /// <summary>
        /// 自动映射对象
        /// </summary>
        /// <param name="mode">默认为严格模式</param>
        /// <returns></returns>
        public SqlDataMapper<T> MapperAuto(MapperAutoMode mode = MapperAutoMode.Strict)
        {
            _mode = mode;

            foreach (var pro in _propertyInfos)
            {
                var key = mode switch
                {
                    MapperAutoMode.IgnoreCase => pro.Name.ToLower(),
                    MapperAutoMode.IgnoreUnderscore => pro.Name.Replace("_", string.Empty),
                    MapperAutoMode.IgnoreCaseAndUnderscore => pro.Name.ToLower().Replace("_", string.Empty),
                    MapperAutoMode.Strict or _ => pro.Name
                };
                _temp.TryAdd(key, (kt, vl) =>
                {
                    if (vl is not System.DBNull)
                    {
                        pro?.SetValue(kt, vl);
                    }
                });
            }

            return this;
        }


        /// <summary>
        /// 手动映射属性
        /// </summary>
        /// <typeparam name="TPropety"></typeparam>
        /// <param name="assignAction"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public SqlDataMapper<T> Mapper<TPropety>(Expression<Func<T, TPropety>> assignAction, string key)
        {

            if (assignAction.Body is MemberExpression member)
            {

                if (_temp.TryGetValue(key, out var val))
                {
                    val = (kt, vl) =>
                    {
                        if (vl is not System.DBNull)
                        {
                            var tDp = _propertyInfos.Find(p => p.Name == member.Member.Name);
                            tDp?.SetValue(kt, vl);
                        }
                    };
                }
                else
                {
                    _temp.Add(key, (kt, vl) =>
                    {
                        if (vl is not System.DBNull)
                        {
                            var tDp = _propertyInfos.Find(p => p.Name == member.Member.Name);
                            tDp?.SetValue(kt, vl);
                        }
                    });
                }

            }
            return this;
        }


        /// <summary>
        /// 手动映射属性
        /// </summary>
        /// <typeparam name="TPropety"></typeparam>
        /// <param name="assignAction"></param>
        /// <param name="key"></param>
        /// <param name="whenNullValue">当为null时值为多少</param>
        /// <returns></returns>
        public SqlDataMapper<T> Mapper<TPropety>(Expression<Func<T, TPropety>> assignAction, string key, TPropety whenNullValue)
        {

            if (assignAction.Body is MemberExpression member)
            {
                if (_temp.TryGetValue(key, out var val))
                {
                    val = (kt, vl) =>
                    {
                        var tDp = _propertyInfos.Find(p => p.Name == member.Member.Name);
                        if (vl is System.DBNull)
                        {
                            tDp?.SetValue(kt, whenNullValue);

                        }
                        else
                        {
                            tDp?.SetValue(kt, vl);
                        }
                    };
                }
                else
                {
                    _temp.Add(key, (kt, vl) =>
                    {
                        var tDp = _propertyInfos.Find(p => p.Name == member.Member.Name);
                        if (vl is System.DBNull)
                        {
                            tDp?.SetValue(kt, whenNullValue);

                        }
                        else
                        {
                            tDp?.SetValue(kt, vl);
                        }
                    });
                }

            }
            return this;
        }


        internal void SetKeys(string[] keys)
        {

            _sets = new Action<T, object>?[keys.Length];

            for (int i = 0; i < keys.Length; i++)
            {
                if (_temp.TryGetValue(keys[i], out var set_1))
                {
                    _sets[i] = set_1;
                }
                else if (_mode != null)
                {

                    var names = _mode switch
                    {
                        MapperAutoMode.IgnoreCase => keys[i].ToLower(),
                        MapperAutoMode.IgnoreUnderscore => keys[i].Replace("_", ""),
                        MapperAutoMode.IgnoreCaseAndUnderscore => keys[i].ToLower().Replace("_", ""),
                        _ => null
                    };

                    if (names == null)
                    {
                        continue;
                    }

                    if (_temp.TryGetValue(names, out var set_2))
                    {
                        _sets[i] = set_2;
                    }
                }
            }

        }


        internal void SetValue(T model, int index, object value)
        {
            _sets[index]?.Invoke(model, value);
        }

    }


    /// <summary>
    /// 映射自动模式
    /// </summary>
    public enum MapperAutoMode
    {
        /// <summary>
        /// 严格的匹配
        /// </summary>
        Strict,
        /// <summary>
        /// 忽略大小写
        /// </summary>
        IgnoreCase,
        /// <summary>
        /// 忽略下划线
        /// </summary>
        IgnoreUnderscore,
        /// <summary>
        /// 忽略大小和和下划线
        /// </summary>
        IgnoreCaseAndUnderscore
    }


}
