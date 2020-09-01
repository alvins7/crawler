using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace crawler
{
    class Program
    {
        static void Main() => StartSync();
        //线程数
        static int TheadNum=10;
        /// <summary>
        /// 程序开始
        /// </summary>
        public static void StartSync()
        {
            for (int i = 0; i < 10; i++)
            {
                abAsync.Post(i);
            }
            //终止线程任务
            abAsync.Complete();
            Console.WriteLine("Post finished");
            //等待任务结束
            abAsync.Completion.Wait();
            Console.WriteLine("Process finished");
        }
        /// <summary>
        /// dataflow开始
        /// </summary>
        public static ActionBlock<int> abAsync = new ActionBlock<int>((i) =>
        {
            Thread.Sleep(1000);
            Console.WriteLine(i + " ThreadId:" + Thread.CurrentThread.ManagedThreadId + " Execute Time:" + DateTime.Now);
            try
            {
                SendAsync1();
            }
            catch (Exception ex)
            {
                string log = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ex.ToString();
                Console.WriteLine(log);
            }
        }, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = TheadNum });
        /// <summary>
        /// 发起GET请求
        /// </summary>
        private static void SendAsync1()
        {
            var httpClient = new HttpClient();
            AddDefaultHeaders(httpClient);
            var response = httpClient.GetAsync("https://www.baidu.com").Result;
            var data = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(data);
        }
        /// <summary>
        /// 添加请求头信息
        /// </summary>
        /// <param name="httpClient"></param>
        private static void AddDefaultHeaders(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("Referer", "https://www.baidu.com");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 13_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Mobile/15E148");
        }
    }
}
