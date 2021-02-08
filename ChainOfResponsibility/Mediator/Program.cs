using System;

namespace Mediator
{
    public class Query
    {
        public string CreatureName;

        public enum Argument
        {
            Attack,
            Defence
        }

        public Argument WhatToQuery;

        public int Value;

        public Query(string creatureName, Argument whatToQuery, int value)
        {
            CreatureName = creatureName;
            WhatToQuery = whatToQuery;
            Value = value;
        }
    }

    public class Game
    {
        public event EventHandler<Query> Queries;

        public void PerformQuery(object sender, Query q)
        {
            Queries?.Invoke(sender, q);
        }
    }

    public class Creature
    {
        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defence)}: {Defence}";
        }

        private Game game;
        public string Name;
        private int attack, defence;

        public Creature(Game game, string name, int attack, int defence)
        {
            this.game = game;
            Name = name;
            this.attack = attack;
            this.defence = defence;
        }

        public int Attack
        {
            get
            {
                var q = new Query(Name, Query.Argument.Attack, attack);
                game.PerformQuery(this, q);
                return q.Value;
            }
        }
        
        public int Defence
        {
            get
            {
                var q = new Query(Name, Query.Argument.Defence, defence);
                game.PerformQuery(this, q);
                return q.Value;
            }
        }
    }

    public abstract class CreatureModifier : IDisposable
    {
        protected Game game;
        protected Creature creature;

        protected CreatureModifier(Game game, Creature creature)
        {
            this.game = game;
            this.creature = creature;
            game.Queries += Handler;
        }

        protected abstract void Handler(object sender, Query q);


        public void Dispose()
        {
            game.Queries -= Handler;
        }
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Game game, Creature creature) : base(game, creature)
        {
            
        }

        protected override void Handler(object sender, Query q)
        {
            if (q.CreatureName == creature.Name && q.WhatToQuery == Query.Argument.Attack)
                q.Value *= 2;
        }
    }

    public class IncreaseDefenceModifier : CreatureModifier
    {
        public IncreaseDefenceModifier(Game game, Creature creature) : base(game, creature)
        {
        }

        protected override void Handler(object sender, Query q)
        {
            if (q.CreatureName == creature.Name && q.WhatToQuery == Query.Argument.Defence)
                q.Value += 2;
        }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            var game = new Game();
            var goblin = new Creature(game,"Goblin",3,3);
            Console.WriteLine(goblin);
            using (new DoubleAttackModifier(game, goblin))
            {
                Console.WriteLine(goblin);
                using (new DoubleAttackModifier(game,goblin))
                {
                    Console.WriteLine(goblin);
                    using (new IncreaseDefenceModifier(game,goblin))
                    {
                        Console.WriteLine(goblin);
                    }
                }
            }
            Console.WriteLine(goblin);
        }
    }
}