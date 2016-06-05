using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Threading;
using System.Configuration;

namespace ConcurrentCalls2.Table
{
    public static class PopulateTables
    {
        private static readonly string _tableKey = ConfigurationManager.AppSettings["tableKey"];
        private static readonly string _tableStorage = ConfigurationManager.AppSettings["tableStorage"];

        public static List<Tuple<string,string>> populate()
        {
            ConcurrentBag<Tuple<string, string>> bag = new ConcurrentBag<Tuple<string, string>>();

            var sc = new StorageCredentials(_tableStorage, _tableKey);
            var sa = new CloudStorageAccount(sc, true);
            var ctc = sa.CreateCloudTableClient();
            var tRef = ctc.GetTableReference("TestTable");
            tRef.DeleteIfExists();
            bool success = true;
            do
            {
                try
                {
                    var table = tRef.CreateIfNotExists();
                    success = true;
                }
                catch
                {
                    success = false;
                    Thread.Sleep(1000);
                }
            } while (!success);

            Parallel.For(0, 1000, (i) =>
                {
                    string partKey = Guid.NewGuid().ToString();
                    var to = new TableBatchOperation();
                    for (int j = 0; j < 10; j++)
                    {
                        TableTestObject tto = new TableTestObject();
                        tto.PartitionKey = partKey;
                        tto.RowKey = Guid.NewGuid().ToString();
                        tto.Guid1 = Guid.NewGuid();
                        tto.Guid2 = Guid.NewGuid();
                        tto.Int1 = 1;
                        tto.Int2 = 2;
                        tto.String1 = "Hello";
                        tto.String2 = "World";
                        to.Insert(tto);
                        if (j == 0)
                            bag.Add(new Tuple<string, string>(tto.PartitionKey, tto.RowKey));
                    };
                    tRef.ExecuteBatch(to);
                }
            );
            return bag.ToList();

        }

    }
}
