using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LeeTeke.SqlDo
{
    public class SqlResult : List<object[]>
    {
        private string[] _header;
        public string[] Header { get => _header; }

        public SqlResult(int cloumn)
        {
            _header = new string[cloumn];
        }

        public new object[] this[int index]
        {
            get => base[index];
            set
            {
                if (value.Length == Header.Length)
                {
                    base[index] = value;
                }
                else if (value == null)
                {
                    base[index] = new object[Header.Length];
                }
                else if (value.Length < Header.Length)
                {
                    object[] newValue = new object[Header.Length];
                    value.CopyTo(newValue, 0);
                    base[index] = newValue;
                }
                else
                {
                    base[index] = value.Take(Header.Length).ToArray();
                }

            }
        }

        public object this[int index, string header]
        {
            get => this[index][Array.IndexOf(Header, header)];

            set => this[index][Array.IndexOf(Header, header)] = value;
        }



        public new void Add(object[] value)
        {

            if (value.Length == Header.Length)
            {
                base.Add(value);
            }
            else if (value == null)
            {
                base.Add(new object[Header.Length]);
            }
            else if (value.Length < Header.Length)
            {
                object[] newValue = new object[Header.Length];
                value.CopyTo(newValue, 0);
                base.Add(newValue);
            }
            else
            {
                base.Add(value.Take(Header.Length).ToArray());
            }

        }

        public void AddRange(SqlResult data)
        {
            if (data == null)
                return;
            if (data.Header.Length != Header.Length)
                throw new Exception("数据列长度不一致");

            base.AddRange(data);
        }
        public new void AddRange(IEnumerable<object[]> collection)
        {
            throw new Exception("不允许通过此方法添加collection,请 添加SqlData类");
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T? GET<T>(int index, string header)
        {
            var value = this[index][Array.IndexOf(Header, header)];
            switch (value)
            {
                case T result:
                    return result;
                case System.DBNull:
                    return default;
                default:
                    try { return (T)value; } catch { }
                    return default;
            }
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T? GET<T>(int index, int headerIndex)
        {
            var value = this[index][headerIndex];
            switch (value)
            {
                case T result:
                    return result;
                case System.DBNull:
                    return default;
                default:
                    try { return (T)value; } catch { }
                    return default;
            }
        }


    }
}
