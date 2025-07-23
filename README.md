# LeeTeke.SqlDo
[非ORM]  
本工具主要为Sql语言命令生成器;简单封装了关于Mysql,SQLite,SqlServer的连接使用。并且封装了自定义数据集合类[SqlResult]。  

	** v1.2.0增加了 手动配置映射与自动映射 查询结果对象的功能
## Nuget
[![NUGET](https://img.shields.io/badge/nuget-1.2.0-blue.svg)](https://www.nuget.org/packages/LeeTeke.SqlDo)

    dotnet add package LeeTeke.SqlDo --version 1.2.0


+ LeeTeke.SqlDo --核心包，可用SqlCmd类进行命令生成，可将DbReader手动配置/自动映射到相关对象。  
+ LeeTeke.SqlDo.MySql --连接MySql数据库所用，依赖MySql.Data包，简单封装了连接类 [MySqlService]。  
+ LeeTeke.SqlDo.SQLite --连接SQLite数据库所用，依赖Microsoft.Data.Sqlite.Core包，简单封装了连接类 [SQLiteService]。  
+ LeeTeke.SqlDo.SqlServer --连接SqlServer数据库所用，依赖Microsoft.Data.SqlClient包，简单封装了连接类 [SqlServerService]。  

## 使用说明
最近事多暂时还未整理好(=.=);Demo里简单的写了一下例子(写了点)。