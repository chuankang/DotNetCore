using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Polly;
using Snowflake.Core;

namespace DotNetCore.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
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

            TestLocals();
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
}
