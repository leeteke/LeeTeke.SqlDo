using System;
using System.Collections.Generic;
using System.Text;

namespace LeeTeke.SqlDo
{
    public enum SqlFunctions
    {

        Other,
        /// <summary>
        /// 总量
        /// </summary>
        Count,
        /// <summary>
        /// 和
        /// </summary>
        Sum,
        /// <summary>
        /// 平均值
        /// </summary>
        Avg,
        /// <summary>
        /// 第一个
        /// </summary>
        First,
        /// <summary>
        /// 最后一个
        /// </summary>
        Last,
        /// <summary>
        /// 最大
        /// </summary>
        Max,
        /// <summary>
        /// 最小
        /// </summary>
        Min,
        /// <summary>
        /// 连接
        /// </summary>
        Group_Concat,
        /// <summary>
        /// ASCII(字符串表达式)
        /// 返回表达式最左侧字符串的ASCII代码值。
        /// </summary>
        ASCII,
        /// <summary>
        /// CHAR(整数数值)
        /// 将整数数值类型的ASCII代码值转换为字符。整数数值是介于0到255之间的整数。
        /// </summary>
        Char,
        /// <summary>
        /// CHARINDEX(eg1,eg2,startindex)
        /// 返回字符串中指定表达式的开始位置
        /// </summary>
        CharIndex,
        /// <summary>
        /// DIFFERENCE(char_eg,char_eg)
        /// 返回一个0~4之间的整数数值，表示两个字符串表达式SOYNDEX值之间的差异。0表示几乎不同或完全不同。4 表示几乎相同或完全相同。
        /// </summary>
        Difference,
        /// <summary>
        /// LEFT(eg,int_eg)
        /// 从字符串左侧截取指定长度的字符，然后返回。
        /// </summary>
        Left,
        /// <summary>
        /// RIGHT(eg,int_eg)
        ///从字符串右侧截取指定长度的字符，然后返回。
        /// </summary>
        Right,
        /// <summary>
        /// LEN(str_eg) 
        /// 返回字符串表达式的长度，其中不包含末尾的空格。
        /// </summary>
        Len,
        /// <summary>
        /// LOWER (str_eg)
        /// 实现对字符串的大小写转换
        /// </summary>
        Lower,
        /// <summary>
        /// UPPER(str_eg)
        /// 实现对字符串的大小写转换
        /// </summary>
        Upper,
        /// <summary>
        /// LTRIM(str_eg) 
        /// 去掉字符串表达式 左空格
        /// </summary>
        LTrim,
        /// <summary>
        /// RTRIM(str_eg) 
        /// 去掉字符串表达式 右空格
        /// </summary>
        RTrim,
        /// <summary>
        /// NCHAR(int_eg)
        /// 根据Unicode标准的定义，返回指定整数代码的Unicode字符。int_eg是介于0~65535之间的正整数。
        /// </summary>
        NChar,
        /// <summary>
        /// PATINDEX("%pattern",eg)
        /// 返回表达式中某模式第一次出现的起始位置如果没找到返回0。
        /// </summary>
        PatIndex,
        /// <summary>
        /// QUOTENAME(eg1,eg2)
        /// 返回带有分隔符的Unicode字符串
        /// </summary>
        QuoteName,
        /// <summary>
        /// REPLACE(eg1,eg2,eg3) 
        /// 字符串替换函数
        /// </summary>
        Replace,
        /// <summary>
        /// REPLICATE(eg1,int_eg) 
        /// 指定次数的重复表达式
        /// </summary>
        Replicate,
        /// <summary>
        /// REVERSE(eg) 
        /// 字符串逆向转换
        /// </summary>
        Reverse,
        /// <summary>
        /// SPACE(int_eg) 
        /// 重复空格的个数
        /// </summary>
        Space,
        /// <summary>
        /// STUFF(char_eg1,startindex,length,char_eg2)
        /// 删除指定长度的字符，并在指定的位置插入另一组字符。
        /// </summary>
        Stuff,
        /// <summary>
        /// SUBSTRING(eg,start,length)
        /// 字符串截取
        /// </summary>
        SubString,

        /// <summary>
        /// 合并字符串
        /// </summary>
        Concat,
        /// <summary>
        /// 字符串疮毒
        /// </summary>
        Length,
    }
}
