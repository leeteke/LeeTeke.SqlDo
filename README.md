# LeeTeke.SqlDo
[非ORM]  
本工具主要为Sql语言命令生成器;简单封装了关于Mysql,SQLite,SqlServer的连接使用。并且封装了自定义数据集合类[SqlResult]。  

+ LeeTeke.SqlDo --核心包，单独仅可用SqlCmd类进行命令生成。  
+ LeeTeke.SqlDo.MySql --连接SySql数据用，依赖MySql.Data包，简单封装了连接类 [MySqlService]。  
+ LeeTeke.SqlDo.SQLite --连接Sqlite数据用，依赖Microsoft.Data.Sqlite.Core包，简单封装了连接类 [SQLiteService]。  
+ LeeTeke.SqlDo --连接SySql数据用，依赖Microsoft.Data.SqlClient包，简单封装了连接类 [SqlServerService]。  

## 使用说明
最近事多暂时还未整理好(=.=);Demo里简单的写了一下例子(写了点)。