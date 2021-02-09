using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Command
{
    public class BankAccount
    {
        private int balance;
        private const int overdaraftLimit = -500;

        public void Deposit(int amount)
        {
            balance += amount;
            Console.WriteLine($"Deposited ${amount}, balance is now {balance}");
        }

        public bool Withdraw(int amount)
        {
            if (balance - amount >= overdaraftLimit)
            {
                balance -= amount;
                Console.WriteLine($"Withdrew ${amount}, balance is now {balance}");
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"{nameof(balance)}: {balance}";
        }
    }

    public interface ICommand
    {
        void Call();
        void Undo();
    }

    public class Command : ICommand
    {
        private BankAccount bankAccount;

        public enum Action
        {
            Deposit,
            Withdraw
        }

        private Action action;
        private int amount;
        private bool succeded;

        public Command(BankAccount bankAccount, Action action, int amount)
        {
            this.bankAccount = bankAccount;
            this.action = action;
            this.amount = amount;
        }

        public void Call()
        {
            switch (action)
            {
                case Action.Deposit:
                    bankAccount.Deposit(amount);
                    succeded = true;
                    break;
                case Action.Withdraw:
                    succeded = bankAccount.Withdraw(amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Undo()
        {
            if (succeded)
            {
                switch (action)
                {
                    case Action.Withdraw:
                        bankAccount.Deposit(amount);
                        break;
                    case Action.Deposit:
                        bankAccount.Withdraw(amount);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var ba = new BankAccount();
            var commands = new List<Command>()
            {
                new Command(ba, Command.Action.Deposit, 100),
                new Command(ba, Command.Action.Withdraw, 1200),
                new Command(ba, Command.Action.Withdraw, 200)
            };
            Console.WriteLine(ba);

            foreach (var command in commands)
            {
                command.Call();
            }

            foreach (var command in Enumerable.Reverse(commands))
            {
                command.Undo();
            }
            
            Console.WriteLine(ba);
        }
    }
}