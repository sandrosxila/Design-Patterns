using System;
using System.Collections.Generic;
using static System.Console;

namespace AbstractFactory
{
    public interface IHotDrink
    {
        void Consume();
    }

    public class Tea : IHotDrink
    {
        public void Consume()
        {
            WriteLine("This tea is nice, but I'd rather have it with some milk!!!");
        }
    }

    public class Coffee : IHotDrink
    {
        public void Consume()
        {
            WriteLine("This coffee is sensational!!!");
        }
    }

    public interface IHotDrinkFactory
    {
        IHotDrink Prepare(int amount);
    }

    public class TeaFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            WriteLine($"Put in a tea bag, boil water, pour {amount} ml, add lemon, enjoy!!!");
            return new Tea();
        }
    }

    public class CoffeeFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            WriteLine($"Grind some beans, boil water, pour {amount} ml, add cream and sugar, enjoy");
            return new Coffee();
        }
    }

    public class HotDrinkMachine
    {
        private List<Tuple<string, IHotDrinkFactory>> factories =
            new List<Tuple<string, IHotDrinkFactory>>();

        public HotDrinkMachine()
        {
            foreach (var t in typeof(HotDrinkMachine).Assembly.GetTypes())
            {
                if (typeof(IHotDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
                {
                    factories.Add(Tuple.Create(
                        t.Name.Replace("Factory", String.Empty),
                        (IHotDrinkFactory) Activator.CreateInstance(t)
                    ));
                }
            }
        }

        public IHotDrink MakeDrink()
        {
            WriteLine("Available drinks:");
            for (int index = 0; index < factories.Count; index++)
            {
                var tuple = factories[index];
                WriteLine($"{index}: {tuple.Item1}");
            }

            while (true)
            {
                string s;
                WriteLine("Please Enter Index of a Drink: ");
                s = ReadLine();
                if (
                    s != null && int.TryParse(s, out int i) && i >= 0 && i < factories.Count
                )
                {
                    WriteLine("Please Enter Amount: ");
                    s = ReadLine();
                    if (s != null && int.TryParse(s, out int amount) && amount > 0)
                    {
                        return factories[i].Item2.Prepare(amount);
                    }
                }
                
                WriteLine("Incorrect Input, Please Try Again!!!");
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var machine = new HotDrinkMachine();
            var drink = machine.MakeDrink();
            drink.Consume();
        }
    }
}