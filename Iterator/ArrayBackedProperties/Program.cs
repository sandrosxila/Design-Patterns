using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ArrayBackedProperties
{
    public class Creature : IEnumerable<int>
    {
        private int[] stats = new int[3];

        private const int strength = 0;
        private const int agility = 1;
        private const int intelligence = 2;
        
        public int Strength
        {
            get => stats[strength];
            set => stats[strength] = value;
        }
        public int Agility
        {
            get => stats[agility];
            set => stats[agility] = value;
        }
        public int Intelligence
        {
            get => stats[intelligence];
            set => stats[intelligence] = value;
        }

        public double AverageStat => stats.Average();


        public IEnumerator<int> GetEnumerator()
        {
            return stats.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int this[int index]
        {
            get => stats[index];
            set => stats[index] = value;
        }
    }
    internal class Program
    {
        public static void Main(string[] args)
        {
            var creature = new Creature();
            creature.Strength = 9;
            creature[1] = 12;
            creature.Intelligence = 15;
            Console.WriteLine($"Creature's Intelligence: {creature[2]}");
            Console.WriteLine(creature.AverageStat);
        }
    }
}