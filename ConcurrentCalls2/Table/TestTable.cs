using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Configuration;

namespace ConcurrentCalls2
{
    public static class TestTable
    {
        private static readonly string _tableKey = ConfigurationManager.AppSettings["tableKey"];
        private static readonly string _tableStorage = ConfigurationManager.AppSettings["tableStorage"];

        public static List<double> makeCalls(int numCalls, int numUsers, List<Tuple<string,string>> toCall)
        {
            var sc = new StorageCredentials(_tableStorage, _tableKey);
            var sa = new CloudStorageAccount(sc, true);
            var ctc = sa.CreateCloudTableClient();
            var tRef = ctc.GetTableReference("TestTable");
            int totalNum = toCall.Count;
            var times = new ConcurrentQueue<double>();

            Parallel.For(0, numCalls, new ParallelOptions() { MaxDegreeOfParallelism = numUsers}, (i) =>
                {
                    var op = TableOperation.Retrieve(toCall[i % toCall.Count].Item1, toCall[i % toCall.Count].Item2);
                    DateTime dt = DateTime.Now;
                    tRef.Execute(op);
                    times.Enqueue((DateTime.Now - dt).TotalMilliseconds);
                }
            );

            return times.ToList();
        }
    }
}
