using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Concurrent;
using Microsoft.Azure.Management.Sql;
using System.Data.Entity;


namespace ConcurrentCalls2.SQL
{
    class PopulateSql
    {
        private static readonly string _sqlConnection = ConfigurationManager.AppSettings["sql"];
        public static List<string> populate()
        {
            var bag = new ConcurrentQueue<YourTable>();

            //This does the work of setting the table up
            using (var ctx = new testDBEntities())
            {
                //This will create the specified table in your 
                //ctx.Database.ExecuteSqlCommand(
                //    "IF  NOT EXISTS (SELECT * FROM sys.objects" +
                //    "WHERE object_id = OBJECT_ID(N'[dbo].[YourTable]') AND type in (N'U'))" +
                //    "BEGIN" +
                //    "CREATE TABLE[dbo].[YourTable](" +
                //    "RowKey VARCHAR(255) NOT NULL PRIMARY KEY, " +
                //    "Guid1 UNIQUEIDENTIFIER, " +
                //    "Guid2 UNIQUEIDENTIFIER, " +
                //    "String1 VARCHAR(255), " +
                //    "String2 VARCHAR(255), " +
                //    "Int1 INT, " +
                //    "Int2 INT " +
                //    ")" +
                //    "END");
                ctx.Database.ExecuteSqlCommand(
                    "TRUNCATE TABLE YourTable");


                Parallel.For(0, 10000, (i) =>
                {
                    YourTable sqlObject = new YourTable();
                    sqlObject.RowKey = Guid.NewGuid().ToString();
                    sqlObject.Guid1 = Guid.NewGuid();
                    sqlObject.Guid2 = Guid.NewGuid();
                    sqlObject.Int1 = 1;
                    sqlObject.Int2 = 2;
                    sqlObject.String1 = "Hello";
                    sqlObject.String2 = "World";
                    bag.Enqueue(sqlObject);
                }
                );
                ctx.YourTables.AddRange(bag);
                ctx.SaveChanges();
            }

            return bag.Select(a => a.RowKey).ToList();
        }
    }
}
