using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeeTeke.SqlDo
{
    public class SqlLikeString
    {

        private string _string;
        /// <summary>
        /// sqlLike字符串 
        /// </summary>
        /// <param name="str">原始字符串</param>
        public SqlLikeString(string str) => _string = str;

        public override string ToString()
        {
            return $"'{LikeTransfer(_string)}'";

        }


        /// <summary>
        /// 相似转义
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string LikeTransfer(string str)
        {

            var value = SqlString.Transfer(str).Replace(@"%", @"\%").Replace(@"_", @"\_");
            if (value.StartsWith(@"\%") || value.StartsWith(@"\_"))
            {
                value = value[1..];
            }

            if (value.EndsWith(@"\%") || value.EndsWith(@"\_"))
            {
                var tag = value.Last();
                value = value.Remove(value.Length - 2) + tag;
            }

            return value;
        }
    }
}
