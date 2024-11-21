using System;
using System.Collections.Generic;
using System.Text;

namespace LeeTeke.SqlDo
{
   public enum SqlOperator
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal=0,
        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual=1,
        /// <summary>
        /// 大于
        /// </summary>
        Greater=2,
        /// <summary>
        /// 小于
        /// </summary>
        Less=3,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterEQ=4,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessEQ=5,
        /// <summary>
        /// 在某个范围内
        /// </summary>
        Between=6,
        /// <summary>
        /// 包含
        /// </summary>
        Like=7,
        /// <summary>
        /// 不在某个范围内
        /// </summary>
        NotNetween=8,
        /// <summary>
        /// 不包含
        /// </summary>
        NotLike=9,
        /// <summary>
        /// 在
        /// </summary>
        In=10,
        /// <summary>
        /// 不在
        /// </summary>
        NotIn=11,
        /// <summary>
        /// 仅公式
        /// </summary>
        OnlyFormula=12,
    }
}
