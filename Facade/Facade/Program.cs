using System;
using ApplicationProgrammingInterface;

namespace Facade
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var r = new SubclassAPI().Result();
            Console.WriteLine(r);
        }
    }
}