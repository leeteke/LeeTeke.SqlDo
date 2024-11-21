using System;
using System.Collections.Generic;
using System.Text;

namespace LeeTeke.SqlDo
{
   public class SqlFormulaType
    {
        public string Value { get;}
        public SqlFormulaType(string value)
        {
            Value = $"({value})";
        }
        public override string ToString()
        {
            return Value;
        }
    }
}
