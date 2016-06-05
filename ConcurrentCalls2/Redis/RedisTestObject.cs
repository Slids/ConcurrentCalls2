using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentCalls2.Redis
{
    class RedisTestObject
    {
        public string Key { get; set; }

        public Guid Guid1 { get; set; }

        public Guid Guid2 { get; set; }

        public string String1 { get; set; }

        public string String2 { get; set; }

        public int Int1 { get; set; }

        public int Int2 { get; set; }
    }
}
