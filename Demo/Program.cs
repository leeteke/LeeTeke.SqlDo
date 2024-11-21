using LeeTeke.SqlDo;
using LeeTeke.SqlDo.MySql;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto.Agreement.Kdf;

namespace Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");



            var mysql = new MySqltest();

            mysql.Init();
            //创建测试
            //mysql.CreateTest();
            //更新测试
            mysql.UpdateTest();
            Console.ReadLine();
        }
    }






    class MySqltest
    {

        public const string PK_id = "id";
        public const string order_id = "order_id";
        public const string user_id = "user_id";
        public const string total_amount = "total_amount";
        public const string status = "status";
        public const string version = "version";
        public const string created_time = "created_time";
        public const string updated_time = "updated_time";
        public const string creator = "creator";
        public const string modifier = "modifier";


        private MySqlService _sql;
        private SqlCmd _cmd;
        public MySqltest()
        {
            _sql = new MySqlService("order_tab");
            _cmd = _sql.GetCmd();
        }


        public void Init()
        {

            MySqlConnectionStringBuilder cs = new MySqlConnectionStringBuilder("ConnectionStrings");
            _sql.ConnectInitialization(cs);
        }


        public void CreateTest()
        {
            var cmd = _cmd.Create(new()
            {
                {order_id ,DateTime.Now.ToFileTime().ToString()},
                {user_id,Guid.NewGuid().ToString("N")},
                {total_amount,15.11 },
                {status,1 },
                {version,0 },
                {created_time,DateTime.Now },
                {updated_time,DateTime.Now},
                {creator,"mysqltest" },
                {modifier,"musqltest" }
            });

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


            var setInt= _sql.Set(upCmd);
            Console.WriteLine( $"执行了：{setInt} 行");

        }



    }
}
