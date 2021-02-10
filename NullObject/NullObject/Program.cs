using System;
using System.Dynamic;
using ImpromptuInterface;

namespace NullObject
{
    public interface ILog
    {
        void Info(string msg);
        void Warn(string msg);
    }

    class ConsoleLog : ILog
    {
        public void Info(string msg)
        {
            Console.WriteLine(msg);
        }

        public void Warn(string msg)
        {
            Console.WriteLine("WARNING!!! " + msg);
        }
    }

    public class NullLog : ILog
    {
        public void Info(string msg)
        {
        }

        public void Warn(string msg)
        {
        }
    }

    public class BankAccount
    {
        private ILog log;
        private int balance;

        public BankAccount(ILog log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public void Deposit(int amount)
        {
            balance += amount;
            log.Info($"Deposited amount : {amount}, balance is now {balance}");
        }
    }


    public class Null<TInterface> : DynamicObject where TInterface : class
    {
        public static TInterface Instance => new Null<TInterface>().ActLike<TInterface>();

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = Activator.CreateInstance(binder.ReturnType);
            return true;
        }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            var ba = new BankAccount(new NullLog());
            ba.Deposit(100);

            var log = Null<ILog>.Instance;
            log.Info("nothing will happen!!!");
            var ba1 = new BankAccount(log);
            ba1.Deposit(500);
        }
    }
}