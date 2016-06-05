using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcurrentCalls2.Table;
using ConcurrentCalls2.Redis;
using ConcurrentCalls2.SQL;

namespace ConcurrentCalls2
{
    class Program
    {
        private static readonly int numItemsToFind = 1;
        private static readonly int numCalls = 2000;
        private static readonly int numUsers = 1;

        static void Main(string[] args)
        {
            var entries = PopulateTables.populate();
            List<double> tableTimes = TestTable.makeCalls(numCalls, numUsers, entries.Take(1).ToList());
            entries = null;

            var oEntries = PopulateRedis.populate();
            List<double> redisTimes = TestRedis.makeCalls(numCalls, numUsers, oEntries.Take(1).ToList());

            oEntries = PopulateSql.populate();
            List<double> sqlTimes =  TestSql.makeCalls(numCalls, numUsers, oEntries.Take(1).ToList());

            using (var fileStream = File.OpenWrite($"numItems{numItemsToFind}numCalls{numCalls}numUsers{numUsers}.txt"))
            {
                using (var sw = new StreamWriter(fileStream))
                {
                    sw.WriteLine("table,redis,sql");
                    for (int i = 0; i < numCalls; i++)
                        sw.WriteLine($"{tableTimes[i]},{redisTimes[i]},{sqlTimes[i]}");
                    sw.Flush();
                }
            }
        }
    }
}
