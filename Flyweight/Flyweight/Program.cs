using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.dotMemoryUnit;
using NUnit.Framework;

namespace Flyweight
{
    public class User1
    {
        private string fullname;

        public User1(string fullname)
        {
            this.fullname = fullname;
        }
    }


    public class User2
    {
        private static List<string> strings = new List<string>();
        private int[] names;

        public User2(string fullname)
        {
            int getOrAdd(string s)
            {
                int idx = strings.IndexOf(s);
                if (idx != -1)
                    return idx;
                strings.Add(s);
                return strings.Count - 1;
            }

            names = fullname.Split(' ').Select(getOrAdd).ToArray();
        }

        public string Fullname => string.Join(" ", names.Select(i => names[i]));
    }

    [TestFixture]
    public class Program
    {
        public static void Main(string[] args)
        {
        }

        public void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public string RandomString()
        {
            Random rand = new Random();
            return new string(
                Enumerable.Range(0, 10)
                    .Select(
                        i => (char) ('a' + rand.Next(26))
                    ).ToArray());
        }

        [Test]
        public void TestUser1() // prints 4329102
        {
            var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

            var users = new List<User1>();

            foreach (var firstName in firstNames)
            {
                foreach (var lastName in lastNames)
                {
                    users.Add(new User1($"{firstName} {lastName}"));
                }
            }

            ForceGC();

            dotMemory.Check(memory => { Console.WriteLine(memory.SizeInBytes); });
        }

        [Test]
        public void TestUser2() // prints 3923456
        {
            var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

            var users = new List<User2>();

            foreach (var firstName in firstNames)
            {
                foreach (var lastName in lastNames)
                {
                    users.Add(new User2($"{firstName} {lastName}"));
                }
            }

            ForceGC();

            dotMemory.Check(memory => { Console.WriteLine(memory.SizeInBytes); });
        }
    }
}