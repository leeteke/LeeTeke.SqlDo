using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LeeTeke.SqlDo
{
    public class SqlAntiInjection
    {
        private static readonly Regex RegSystemThreats = new Regex(@"\s*or\s|\sor\s*|\s?;\s?|\s?drop\s|\s?grant\s|^'|\s?--|\s?union\s|\s?delete\s|\s?truncate\s|\s?#|" + @"\s?sysobjects\s?|\s?xp_.*?|\s?syslogins\s?|\s?sysremote\s?|\s?sysusers\s?|\s?sysxlogins\s?|\s?sysdatabases\s?|\s?aspnet_.*?|\s?exec\s?",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// 检查命令
        /// </summary>
        /// <param name="cmdString"></param>
        /// <returns>返回true则为检测到非法数据</returns>
        public static (bool security, string erroStr) SecurityStr(params string[] cmdString)
        {
            foreach (var item in cmdString)
            {
                if (RegSystemThreats.IsMatch(item))
                {
                    return (false, item);
                }
            }
            return (true, string.Empty);
        }

    }
}
