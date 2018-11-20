using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Xml;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Polly;
using Snowflake.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DotNetCore.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ////读取数据库连接配置文件1
            //var config = new ConfigurationBuilder()
            //    .AddInMemoryCollection()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .Build();
            //var appConfigProvider = new ServiceCollection().AddOptions().Configure<AppConfigurations>
            //    (config.GetSection("ConnectionStrings")).BuildServiceProvider();
            //var appConfigurations = appConfigProvider.GetService<IOptions<AppConfigurations>>().Value;

            //Console.WriteLine(appConfigurations.DefaultConnection);

            //TestPolly();

            //.NET Core中的性能测试工具BenchmarkDotnet
            //var summary = BenchmarkRunner.Run<Md5VsSha256>();

            //雪花算法
            //IdWorker 应该实例化一次。否则，将会重复
            //var worker = new IdWorker(1, 1);
            //long id = worker.NextId();

            ////值元组
            //var testTuple = TestTupleValue();
            //var age = testTuple.age;
            //解构元组
            //(int age, string name) = TestTupleValue();
            //var x = age;

            //TestLocals();
            TestCSRedis();
            Console.ReadLine();
        }

        /// <summary>
        /// Polly重试机制
        /// </summary>
        private static void TestPolly()
        {
            try
            {
                var retryTwoTimesPolicy = Policy.Handle<DivideByZeroException>()
                    .Retry(3, (ex, count) => //重试策略
                    {
                        Console.WriteLine("执行失败! 重试次数 {0}", count);
                        Console.WriteLine("异常来自 {0}", ex.GetType().Name);
                    });

                retryTwoTimesPolicy.Execute(() =>
                {
                    Compute();
                });
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine($"Excuted Failed,Message: ({e.Message})");
            }
        }

        private static int Compute()
        {
            var a = 0;
            return 1 / a;
        }

        private static Tuple<int, int, int, int, int, int, int> Tuple4()
        {
            return new Tuple<int, int, int, int, int, int, int>(1, 1, 1, 1, 1, 1, 1);
        }


        #region C#6.0

        /// <summary>
        /// 
        /// </summary>
        private static void TestInter(string name)
        {
            var x = "conn";
            //1.字符串嵌入值(String interpolation) 
            Console.WriteLine($"name:{x}");

            //2.空值运算符(Null-conditional operators)
            var account = new Account();
            var age = account.AgeList?[0].ToString();

            //3.对象初始化器(Index Initializers)
            IDictionary<int, string> dict = new Dictionary<int, string>()
            {
                [1] = "first",
                [2] = "second"
            };
            foreach (var dic in dict)
            {
                Console.WriteLine($"key: {dic.Key} value:{dic.Value}");
            }

            if (name == null)
            {
                //5.nameof表达式 (nameof expressions)
                throw new ArgumentNullException(nameof(name));
            }
        }

        ////4.导入静态类(Using Static)
        //using static System.Math;//注意这里不是命名空间哦
        //Console.WriteLine($"之前的使用方式: {Math.Pow(4, 2)}");
        //Console.WriteLine($"导入后可直接使用方法: {Pow(4,2)}");

        #endregion

        #region C#7.0

        /// <summary>
        /// 值元组 Framework4.7 C#7.0
        /// </summary>
        private static (int age, string name) TestTupleValue()
        {
            return (18, "张三");
        }

        /// <summary>
        /// C# 7.0 匹配模式
        /// </summary>
        private static void TestPattern(object obj)
        {
            if (obj is int b) //is判断
            {
                int d = b + 10; //加10
                Console.WriteLine(d); //输出
            }
        }

        /// <summary>
        /// C# 7.0局部变量
        /// </summary>
        private static void TestLocals()
        {
            int x = 3;
            ref int x1 = ref x;//通过ref关键字 把x赋值给x1
            x1 = 2;
            //我们通过ref关键字把x赋给了x1,如果是值类型的传递,那么对x将毫无影响 还是输出3.
            //好处不言而喻,在某些特定的场合,我们可以直接用ref来引用传递,减少了值传递所需要开辟的空间.
            Console.WriteLine($"改变后的变量{nameof(x)}值为{x}");//输出值为2
        }

        /// <summary>
        /// C# 7.0局部函数
        /// </summary>
        private static void TestFunc()
        {
            int sum = Add(100, 200);

            int Add(int a, int b)
            {
                return a + b;
            }

        }

        #endregion

        private static void TestCSRedis()
        {
            //普通订阅
            RedisHelper.Subscribe(
                ("chan1", msg => Console.WriteLine(msg.Body)),
                ("chan2", msg => Console.WriteLine(msg.Body)));

            //模式订阅（通配符）
            RedisHelper.PSubscribe(new[] { "test*", "*test001", "test*002" }, msg => {
                Console.WriteLine($"PSUB   {msg.MessageId}:{msg.Body}    {msg.Pattern}: chan:{msg.Channel}");
            });
            //模式订阅已经解决的难题：
            //1、分区的节点匹配规则，导致通配符最大可能匹配全部节点，所以全部节点都要订阅
            //2、本组 "test*", "*test001", "test*002" 订阅全部节点时，需要解决同一条消息不可执行多次

            //发布
            RedisHelper.Publish("chan1", "123123123");
            RedisHelper.Publish("chan2", "test3123");
        }
    }

    public class SingleVsFirst
    {
        private readonly List<string> _haystack = new List<string>();
        private readonly int _haystackSize = 1000000;
        public List<string> Needles => new List<string> { "StartNeedle", "MiddleNeedle", "EndNeedle" };

        public SingleVsFirst()
        {
            //Add a large amount of items to our list
            Enumerable.Range(1, _haystackSize).ToList().ForEach(t => _haystack.Add(t.ToString()));

            //one at the start
            _haystack.Insert(0, Needles[0]);

            //one right in the middle
            _haystack.Insert(_haystackSize / 2, Needles[1]);

            //one at the end
            _haystack.Insert(_haystack.Count - 1, Needles[2]);
        }

        [ParamsSource(nameof(Needles))]
        public string Needle { get; set; }

        [Benchmark]
        public string Single() => _haystack.SingleOrDefault(t => t == Needle);
        [Benchmark]
        public string First() => _haystack.FirstOrDefault(t => t == Needle);

    }

    public class Md5VsSha256
    {
        private const int N = 10000;
        private readonly byte[] _data;
        private readonly SHA256 _sha256 = SHA256.Create();
        private readonly MD5 _md5 = MD5.Create();

        public Md5VsSha256()
        {
            _data = new byte[N];
            new Random(42).NextBytes(_data);
        }

        [Benchmark]
        public byte[] Sha256()
        {
            return _sha256.ComputeHash(_data);
        }

        [Benchmark]
        public byte[] Md5()
        {
            return _md5.ComputeHash(_data);
        }
    }

    /// <summary>
    /// C#6.0自动属性初始化
    /// </summary>
    public class Account
    {
        public string Name { get; set; } = "summit";
        public int Age { get; set; } = 22;
        public IList<int> AgeList
        {
            get;
            set;
        } = new List<int> { 10, 20, 30, 40, 50 };
    }

    public class Book
    {
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int Year { get; set; }
    }
}
