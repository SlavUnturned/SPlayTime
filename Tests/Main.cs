using System;
using System.Threading;
using NUnit.Framework;
using SPlayTime;
using Steamworks;

namespace Tests
{
    public class Tests
    {
        static void Main()
        {
            new Tests().DBTest();
        }
        [Test]
        public void DBTest()
        {
            IDBConnector connector = new MySqlDBConnector(new Config
            {
                ConnectionString = ConnectionStringBuilder
                    .Create()
                    .UseDataBase("test")
                    .Build(),
                TableName = "SRanks",
                IdColumn = "SteamId",
                TimeColumn = "Points"
            });
            if (!connector.TryConnect()) Assert.Fail("Can't connect to database");
            for (int i = 0; i < 100; i++)
                Console.WriteLine(connector.GetSeconds("0"));
            Assert.Pass();
        }
    }
}