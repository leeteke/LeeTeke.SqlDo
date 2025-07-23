using LeeTeke.SqlDo;
using LeeTeke.SqlDo.MySql;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto.Agreement.Kdf;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");


            MySqltest mySqltest = new MySqltest();


            mySqltest.Init();

            mySqltest.CreateTest();

            mySqltest.GetTest();



            Console.ReadLine();
        }
    }




    class MySqltest
    {

        public const string PK_id = "id";
        public const string order_id = "order_id";
        public const string user_id = "user_id";
        public const string total_amount = "total_amount";
        public const string status_code = "status_code";
        public const string created_time = "created_time";
        public const string updated_time = "updated_time";
        public const string creator = "creator";
        public const string modifier = "modifier";


        private MySqlService _sql;
        private SqlCmd _cmd;
        public MySqltest()
        {
            _sql = new MySqlService("order_info");
            _cmd = _sql.GetCmd();
        }


        public void Init()
        {
             MySqlConnectionStringBuilder cs = new MySqlConnectionStringBuilder("ConnectionStrings");
            _sql.ConnectInitialization(cs);
        }


        public void CreateTest()
        {

            var cmd = _cmd.Create([new()
            {
                {order_id ,DateTime.Now.ToFileTime().ToString()},
                {user_id,Guid.NewGuid().ToString("N")},
                {total_amount,15.11 },
                {status_code,1 },
                {created_time,DateTime.Now },
                {updated_time,DateTime.Now},
                {creator,"mysqltest" },
                {modifier,"musqltest" }
            },
            new  ()
            {
                {order_id ,DateTime.Now.ToFileTime().ToString()},
                {user_id,Guid.NewGuid().ToString("N")},
                {total_amount,15.11 },
                {status_code,1 },
                {created_time,DateTime.Now },
                {updated_time,DateTime.Now},
                {creator,"mysqltest" },
                {modifier,"musqltest" }
            }
            ]);

            var executedInt = _sql.Set(cmd);
            Console.WriteLine($"执行了：{executedInt} 次");
        }


        public void UpdateTest()
        {

            var fistCmd = _cmd.GetFirst(null, [order_id]);

            var filstResult = _sql.Get(fistCmd);

            var orderId = filstResult.GET<long>(0, order_id);

            Console.WriteLine($"查询到ID:{orderId}");


            var upCmd = _cmd.Update(order_id, orderId, new()
            {
                {updated_time, DateTime.Now},
                { modifier,"updateTest"}
            });


            var setInt = _sql.Set(upCmd);
            Console.WriteLine($"执行了：{setInt} 行");

        }


        public void GetTest()
        {

            var cmd = _cmd.Get(null, null, null, SqlKeys.AllKeys, ulong.MaxValue);


            //使用SqlResult
            SqlResult sqlResult = _sql.Get(cmd);
            List<MySqltestEntity> data_0 = [];
            for (int i = 0; i < sqlResult.Count; i++)
            {
                data_0.Add(new()
                {
                    Id = sqlResult.GET<long>(i, PK_id),
                    OrderId = sqlResult.GET<string>(i, order_id)!,
                    UserId = sqlResult.GET<string>(i, user_id)!,
                    TotalAmount = sqlResult.GET<decimal>(i, total_amount),
                    StatusCode = sqlResult.GET<sbyte>(i, status_code),
                    CreatedTime = sqlResult.GET<DateTime>(i, created_time),
                    UpdatedTime = sqlResult.GET<DateTime>(i, updated_time),
                    Creator = sqlResult.GET<string>(i, creator)!,
                    ModifierPP = sqlResult.GET<string>(i, modifier)!,
                });
            }


            //使用手动映射
            //只映射配置的属性
            List<MySqltestEntity> data_1 = _sql.Get<MySqltestEntity>(cmd,
                x => x.Mapper(p => p.Id, PK_id)
                .Mapper(p => p.OrderId, order_id)
                .Mapper(p => p.UserId, user_id)
                .Mapper(p => p.TotalAmount, total_amount)
                .Mapper(p => p.StatusCode, status_code)
                .Mapper(p => p.CreatedTime, created_time)
                .Mapper(p => p.UpdatedTime, updated_time)
                .Mapper(p => p.Creator, creator)
                .Mapper(p => p.ModifierPP, modifier, "-")//如果该值为DBNull的化，那么则使用这个"-"值
                );


            //使用自动映射
            List<MySqltestEntity> data_2 = _sql.Get<MySqltestEntity>(cmd,
                x => x.MapperAuto(MapperAutoMode.IgnoreCaseAndUnderscore)//模式使用忽略大小写且忽略下划线
                      .Mapper(p => p.ModifierPP, modifier, "-")//名称差异大，不一致，可继续使用.Mapper 方法进行手动补充

          );

        }



    }




    internal class MySqltestEntity
    {
        public long Id { get; set; }

        public string OrderId { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public int StatusCode { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Creator { get; set; } = null!;
        public string? ModifierPP { get; set; } = null!;

    }

}
