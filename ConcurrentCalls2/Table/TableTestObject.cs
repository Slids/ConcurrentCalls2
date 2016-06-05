using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ConcurrentCalls2.Table
{
    class TableTestObject : TableEntity
    {
        public Guid Guid1 { get; set; }

        public Guid Guid2 { get; set; }

        public string String1 { get; set; }

        public string String2 { get; set; }

        public int Int1 { get; set; }

        public int Int2 { get; set; }
    }
}
