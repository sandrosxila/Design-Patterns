using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using MoreLinq;
using NUnit.Framework;

namespace Singleton
{
    internal class Program
    {
        public interface IDatabase
        {
            int GetPopulation(string name);
        }

        public class SingletonDatabase : IDatabase
        {
            private Dictionary<string, int> capitals;
            private static int instanceCount = 0;
            public static int Count => instanceCount;

            private SingletonDatabase()
            {
                Console.WriteLine("Initializing Database!!!");
                capitals = File.ReadAllLines(
                    Path.Combine(
                        new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName,
                        "capitals.txt")
                ).Batch(2).ToDictionary(
                    list => list.ElementAt(0).Trim(),
                    list => int.Parse(list.ElementAt(1))
                );
            }

            private static Lazy<SingletonDatabase> instance =
                new Lazy<SingletonDatabase>(() =>
                    {
                        instanceCount++;
                        return new SingletonDatabase();
                    }
                );

            public static SingletonDatabase Instance = instance.Value;

            public int GetPopulation(string name)
            {
                return capitals[name];
            }
        }

        public class OrdinaryDatabase : IDatabase
        {
            private Dictionary<string, int> capitals;

            private OrdinaryDatabase()
            {
                Console.WriteLine("Initializing Database!!!");
                capitals = File.ReadAllLines(
                    Path.Combine(
                        new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName,
                        "capitals.txt")
                ).Batch(2).ToDictionary(
                    list => list.ElementAt(0).Trim(),
                    list => int.Parse(list.ElementAt(1))
                );
            }

            public int GetPopulation(string name)
            {
                return capitals[name];
            }
        }

        public class DummyDatabase : IDatabase
        {
            public int GetPopulation(string name)
            {
                return new Dictionary<string, int>
                {
                    ["alpha"] = 1,
                    ["beta"] = 2,
                    ["gamma"] = 3
                }[name];
            }
        }

        public class SingletonRecordFinder
        {
            public int GetTotalPopulation(IEnumerable<string> names)
            {
                int result = 0;
                foreach (var name in names)
                {
                    result += SingletonDatabase.Instance.GetPopulation(name);
                }

                return result;
            }
        }

        public class ConfigurableRecordFinder
        {
            private IDatabase database;

            public ConfigurableRecordFinder(IDatabase database)
            {
                this.database = database;
            }

            public int GetTotalPopulation(IEnumerable<string> names)
            {
                int result = 0;

                foreach (var name in names)
                    result += database.GetPopulation(name);
                return result;
            }
        }

        [TestFixture]
        public class SingletonTests
        {
            [Test]
            public void IsSingletonTest()
            {
                var db = SingletonDatabase.Instance;
                var db2 = SingletonDatabase.Instance;
                Assert.That(db, Is.SameAs(db2));
                Assert.That(SingletonDatabase.Count, Is.EqualTo(1));
            }

            [Test]
            public void SingletonTotalPopulationTest()
            {
                // Singleton database is used as live (production) database test
                // data of live database may changed by user
                // so we need to create another database for test environment
                var rf = new SingletonRecordFinder();
                var names = new[] {"Seoul", "Mexico City"};
                var tp = rf.GetTotalPopulation(names);
                Assert.That(tp, Is.EqualTo(17500000 + 17400000));
            }

            [Test]
            public void ConfigurableTotalPopulationTest()
            {
                //database for test environment
                var rf = new ConfigurableRecordFinder(new DummyDatabase());
                var names = new[] {"alpha", "gamma"};
                var tp = rf.GetTotalPopulation(names);
                Assert.That(tp, Is.EqualTo(1 + 3));
            }

            [Test]
            public void DIPopulationTest()
            {
                var cb = new ContainerBuilder();
                cb.RegisterType<OrdinaryDatabase>().As<IDatabase>().SingleInstance();
                cb.RegisterType<ConfigurableRecordFinder>();
                using (var c = cb.Build())
                {
                    var rf = c.Resolve<ConfigurableRecordFinder>();
                }
            }
        }

        public static void Main(string[] args)
        {
            var db = SingletonDatabase.Instance;
            var db2 = SingletonDatabase.Instance;

            var city = "Tokyo";

            Console.WriteLine($"{city} has population {db.GetPopulation(city)}");
        }
    }
}