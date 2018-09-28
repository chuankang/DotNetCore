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

            //值元组
            var testTuple = TestTupleValue();
            var age = testTuple.age;

            Console.ReadLine();
        }

        /// <summary>
        /// Polly重试机制
        /// </summary>
        public static void TestPolly()
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

        static int Compute()
        {
            var a = 0;
            return 1 / a;
        }

        /// <summary>
        /// 值元组 Framework4.7 C#7.0
        /// </summary>
        /// <returns></returns>
        public static (int age, string name) TestTupleValue()
        {
            return (18, "张三");
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


}
