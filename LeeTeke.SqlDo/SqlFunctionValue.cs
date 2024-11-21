using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeeTeke.SqlDo
{
    public class SqlFunctionValue
    {
        public SqlFunctions Function { get; set; }

        public object[] Args { get; set; }

        public SqlFunctionValue(SqlFunctions function, object[] args)
        {
            Function = function;
            Args = args;
        }

        public override string ToString()
        {
            return $"{SqlCmd.GetFunctionsEnum(Function)}({string.Join(",", Args)})";
        }

    }
}
