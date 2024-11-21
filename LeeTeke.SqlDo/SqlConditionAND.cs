using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeeTeke.SqlDo
{
    public class SqlConditionAND : SqlCondition
    {

        public override string ToString()
        {
            var list = SqlCmd.ConditionToList(this);
            if (list == null)
                return string.Empty;
            return string.Join(" AND ", list);
        }
    }
}
