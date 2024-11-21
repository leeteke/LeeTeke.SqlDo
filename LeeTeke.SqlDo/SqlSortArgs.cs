using System;
using System.Collections.Generic;
using System.Text;

namespace LeeTeke.SqlDo
{
    public class SqlSortArgs : Dictionary<string, bool>
    {
        public override string ToString()
        {
            StringBuilder sb = new();
            foreach (var item in this)
            {
                sb.Append(item.Key);
                sb.Append(item.Value ? " ASC," : " DESC,");
            }

            if (sb.Length > 3)
            {
                sb.Remove(sb.Length - 1, 1);
                sb.Insert(0, " ORDER BY ");
            }

            return sb.ToString();
        }
    }
}
