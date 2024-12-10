using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeeTeke.SqlDo
{
    /// <summary>
    /// sql字符串 自动将'符号格式化，方便某些string写入
    /// </summary>
    public class SqlString
    {
        private string _string;
        /// <summary>
        /// sql字符串 自动将'符号格式化，方便某些string写入
        /// </summary>
        /// <param name="str">原始字符串</param>
        public SqlString(string str)=>_string=str;

        public override string ToString()
        {
            return _string.Replace("'","\\'");
        }
    }
}
