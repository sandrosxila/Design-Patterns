using System;
using static System.Console;
namespace ChainOfResponsibility
{

    public class Creature
    {
        public string Name;
        public int Attack, Defense;

        public Creature(string name, int attack, int defense)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defense)}: {Defense}";
        }
    }

    public class CreatureModifier
    {
        protected Creature creature;
        protected CreatureModifier next;

        public CreatureModifier(Creature creature)
        {
            this.creature = creature;
        }

        public void Add(CreatureModifier cm)
        {
            if (next != null)
                next.Add(cm);
            else next = cm;
        }

        public virtual void Handle() => next?.Handle();
    }

    public class NoBonusesModifier : CreatureModifier
    {
        public NoBonusesModifier(Creature creature) : base(creature) 
        {
            
        }
        public override void Handle()
        {
        }
    }
    
    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Creature creature) : base(creature)
        {
            
        }

        public override void Handle()
        {
            WriteLine($"Doubling {creature.Name}'s Attack");
            creature.Attack *= 2;
            base.Handle();
        }
    }

    public class IncreasedDefenseModifier : CreatureModifier
    {
        public IncreasedDefenseModifier(Creature creature) : base(creature) 
        {
            
        }

        public override void Handle()
        {
            WriteLine($"Increasing {creature.Name}'s defence");
            creature.Defense += 3;
            base.Handle();
        }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            var goblin = new Creature("Goblin", 2, 2);
            var root = new CreatureModifier(goblin);
            WriteLine(goblin);

            WriteLine($"Let's Double goblin's attack!!!");
            root.Add(new DoubleAttackModifier(goblin));
            
            root.Add(new NoBonusesModifier(goblin));

            WriteLine($"Let's Increase goblins defense!!!");
            root.Add(new IncreasedDefenseModifier(goblin));

            root.Handle();
            
            WriteLine(goblin);
        }
    }
}