using System;
using Polly;

namespace DotNetCore.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var retryTwoTimesPolicy =
                    Policy
                        .Handle<DivideByZeroException>()
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

    }
}
