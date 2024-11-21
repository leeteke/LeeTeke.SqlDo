using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeeTeke.SqlDo
{
    public class SqlString
    {
        private string _string;
        public SqlString(string str)=>_string=str;

        public override string ToString()
        {
            return _string.Replace("'","\\'");
        }
    }
}
