using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Configuration;

namespace ConcurrentCalls2.SQL
{
    class TestSql
    {

        public static List<double> makeCalls(int numCalls, int numUsers, List<string> toCall)
        {
            var times = new ConcurrentQueue<double>();

            int amount = toCall.Count;
            Parallel.For(0, numCalls, new ParallelOptions() { MaxDegreeOfParallelism = numUsers }, (i) =>
            {
                string rowKey = toCall[(i % amount)];
                DateTime dt = DateTime.Now;
                using (var ctx = new testDBEntities())
                {
                    var query = from c in ctx.YourTables
                                where c.RowKey == rowKey
                                select c;
                    var o = query.SingleOrDefault();
                };
                times.Enqueue((DateTime.Now - dt).TotalMilliseconds);
            }
            );

            return times.ToList();
        }
    }
}
