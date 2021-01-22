using System;

namespace Monostate
{
    internal class Program
    {
        class CEO
        {
            private static string name;
            private static int age;
            
            public string Name
            {
                get => name;
                set => name = value;
            }

            public int Age
            {
                get => age;
                set => age = value;
            }

            public override string ToString()
            {
                return $"{nameof(Name)}: {Name}, {nameof(Age)}: {Age}";
            }
        }
        public static void Main(string[] args)
        {
            var ceo = new CEO();
            ceo.Name = "Sandro Skhirtladze";
            ceo.Age = 21;
            var ceo2 = new CEO();
            Console.WriteLine(ceo2);
        }
    }
}