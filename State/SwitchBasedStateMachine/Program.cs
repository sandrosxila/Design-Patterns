﻿using System;
using System.Text;

namespace SwitchBasedStateMachine
{
    internal class Program
    {
        enum State
        {
            Locked,
            Failed,
            Unlocked
        }
        public static void Main(string[] args)
        {
            string code = "1234";
            var state = State.Locked;
            var entry = new StringBuilder();

            while (true)
            {
                switch (state)
                {
                    case State.Locked:
                        entry.Append(Console.ReadKey().KeyChar);
                        if (entry.ToString() == code)
                        {
                            state = State.Unlocked;
                            break;
                        }

                        if (!code.StartsWith(entry.ToString()))
                        {
                            // state = State.Failed;
                            goto case State.Failed;
                        }
                        break;
                    case State.Failed:
                        // Console.CursorLeft = 0;
                        Console.WriteLine("Failed!!!");
                        entry.Clear();
                        state = State.Locked;
                        break;
                    case State.Unlocked:
                        Console.CursorLeft = 0;
                        Console.WriteLine("Unlocked!!!");
                        return;
                }
            }
        }
    }
}