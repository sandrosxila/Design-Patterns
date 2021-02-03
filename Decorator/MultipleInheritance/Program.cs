using System;

namespace MultipleInheritance
{
    public interface IBird
    {
        int Weight { get; set; }
        void Fly();
    }

    public interface ILizard
    {
        int Weight { get; set; }
        void Crawl();
    }

    public class Bird : IBird
    {
        public int Weight { get; set; }

        public void Fly()
        {
            Console.WriteLine($"I can fly with weight: {Weight}");
        }
    }

    public class Lizard : ILizard
    {
        public int Weight { get; set; }

        public void Crawl()
        {
            Console.WriteLine($"I can crawl with weight: {Weight}");
        }
    }

    public class Dragon
    {
        private Bird bird = new Bird();
        private Lizard lizard = new Lizard();
        private int weight;

        public void Fly()
        {
            bird.Fly();
        }

        public void Crawl()
        {
            lizard.Crawl();
        }

        public int Weight
        {
            get => Weight;
            set
            {
                weight = value;
                bird.Weight = value;
                lizard.Weight = value;
            }
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var dragon = new Dragon();
            dragon.Weight = 225;
            dragon.Fly();
            dragon.Crawl();
        }
    }
}