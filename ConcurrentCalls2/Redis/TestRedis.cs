using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Configuration;

namespace ConcurrentCalls2.Redis
{
    public static class TestRedis
    {

        private static readonly string _redisConnection = ConfigurationManager.AppSettings["redis"];

        public static List<double> makeCalls(int numCalls, int numUsers, List<string> toCall)
        {
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(_redisConnection);
            var db = connection.GetDatabase();
            var times = new ConcurrentQueue<double>();

            Parallel.For(0, numCalls, new ParallelOptions() { MaxDegreeOfParallelism = numUsers }, (i) =>
            {
                DateTime dt = DateTime.Now;
                var o = JsonConvert.DeserializeObject<RedisTestObject>(db.StringGet(toCall[i % toCall.Count]));
                times.Enqueue((DateTime.Now - dt).TotalMilliseconds);
            }
            );

            return times.ToList();
        }
    }
}
