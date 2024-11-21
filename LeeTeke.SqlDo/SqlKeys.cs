using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeeTeke.SqlDo
{
    public static class SqlKeys
    {
        /// <summary>
        /// 所有的数
        /// </summary>
        public static string[] AllKeys => new string[] { All };
        /// <summary>
        /// 代表所有的表示
        /// </summary>
        public const string All = "*";

        /// <summary>
        ///  去除重复
        /// </summary>
        public const string Distinct = " DISTINCT ";
    }
}
