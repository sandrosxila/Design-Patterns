using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Autofac;

namespace EventBroker
{
    public class Actor
    {
        protected EventBroker broker;

        public Actor(EventBroker broker)
        {
            this.broker = broker;
        }
    }

    public class FootballPlayer : Actor
    {
        public string Name { get; set; }
        public int GoalsScored = 0;
        
        
        public void Score()
        {
            GoalsScored++;
            broker.Publish(new PlayerScoredEvent {Name = Name, GoalsScored = GoalsScored});
        }

        public void AssaultReferee()
        {
            broker.Publish(new PlayerSentOffEvent {Name = Name, Reason = "violence"});
        }

        public FootballPlayer(EventBroker broker, string name) : base(broker)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));

            broker.OfType<PlayerScoredEvent>()
                .Where(ps => ps.Name != Name)
                .Subscribe(
                    ps => { Console.WriteLine($"{name}: Nicely done, {ps.Name}! it's your {ps.GoalsScored} Goal!"); }
                );

            broker.OfType<PlayerSentOffEvent>()
                .Where(ps => ps.Name != Name)
                .Subscribe(ps => { Console.WriteLine($"{name}: see you in the lockers {ps.Name}"); });
        }
    }

    public class FootballCoach : Actor
    {
        public FootballCoach(EventBroker broker) : base(broker)
        {
            broker.OfType<PlayerScoredEvent>().Subscribe(
                pe =>
                {
                    if (pe.GoalsScored < 3)
                        Console.WriteLine($"Coach: Well Done, {pe.Name}!");
                }
            );

            broker.OfType<PlayerSentOffEvent>().Subscribe(
                pe =>
                {
                    if (pe.Reason == "violence")
                        Console.WriteLine($"Coach: How could you {pe.Name}.");
                }
            );
        }
    }

    public class PlayerEvent
    {
        public string Name { get; set; }
    }

    public class PlayerScoredEvent : PlayerEvent
    {
        public int GoalsScored { get; set; }
    }

    public class PlayerSentOffEvent : PlayerEvent
    {
        public string Reason { get; set; }
    }

    public class EventBroker : IObservable<PlayerEvent>
    {
        private Subject<PlayerEvent> subscriptions = new Subject<PlayerEvent>();

        public IDisposable Subscribe(IObserver<PlayerEvent> observer)
        {
            return subscriptions.Subscribe(observer);
        }

        public void Publish(PlayerEvent pe)
        {
            subscriptions.OnNext(pe);
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var cb = new ContainerBuilder();
            cb.RegisterType<EventBroker>().SingleInstance();
            cb.RegisterType<FootballCoach>();
            cb.Register(
                (c, p) =>
                    new FootballPlayer(
                        c.Resolve<EventBroker>(),
                        p.Named<string>("name")
                    )
            );
            using (var c = cb.Build())
            {
                var coach = c.Resolve<FootballCoach>();
                var player1 = c.Resolve<FootballPlayer>(new NamedParameter("name", "John"));
                var player2 = c.Resolve<FootballPlayer>(new NamedParameter("name", "Chris"));
                player1.Score();
                player1.Score();
                player1.Score();
                player1.AssaultReferee();
                player2.Score();
            }
        }
    }
}