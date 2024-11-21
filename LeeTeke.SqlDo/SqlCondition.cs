using System;
using System.Collections.Generic;
using System.Text;

namespace LeeTeke.SqlDo
{
    public abstract class SqlCondition : List<(string? key, object value, SqlOperator perator)>
    {

        public void Add(string key, object value, SqlOperator operatorEnum = SqlOperator.Equal)
        {
            base.Add((key, value, operatorEnum));
        }

        public void AddSqlFormula(string formula)
        {
            base.Add((null, new SqlFormulaType(formula), SqlOperator.OnlyFormula));
        }

    }
}
