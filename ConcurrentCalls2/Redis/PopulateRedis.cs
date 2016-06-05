using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using System.Configuration;
namespace ConcurrentCalls2.Redis
{
    class PopulateRedis
    {
        private static readonly string _redisConnection = ConfigurationManager.AppSettings["redis"];
        public static List<string> populate()
        {
            var bag = new ConcurrentBag<string>();
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect( _redisConnection + ",allowAdmin=true");
            var db = connection.GetDatabase();
            var endpoints = connection.GetEndPoints(true);
            foreach(var e in endpoints)
            {
                var s = connection.GetServer(e);
                s.FlushAllDatabases();
            }

            Parallel.For(0, 10000, (i) =>
            {
                string partKey = Guid.NewGuid().ToString();
                RedisTestObject tto = new RedisTestObject();
                tto.Key = partKey;
                tto.Guid1 = Guid.NewGuid();
                tto.Guid2 = Guid.NewGuid();
                tto.Int1 = 1;
                tto.Int2 = 2;
                tto.String1 = "Hello";
                tto.String2 = "World";
                bag.Add(tto.Key);
                db.StringSet(tto.Key, JsonConvert.SerializeObject(tto));
            }
            );
            return bag.ToList();
        }
    }
}
