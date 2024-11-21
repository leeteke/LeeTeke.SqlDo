using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeeTeke.SqlDo
{
    /// <summary>
    /// SqlDo发生的异常
    /// </summary>
    public class SqlDoException:Exception
    {

        public SqlDoException():base(){}
     
        public SqlDoException(string? message):base(message){}
       
        public SqlDoException(string? message, Exception? innerException):base(message, innerException){}
      
    }
}
