using System;
using System.Windows;

namespace WeakEvent
{
    public class Button
    {
        public event EventHandler Clicked;

        public void Fire()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Window
    {
        public Window(Button button)
        {
            // button.Clicked += ButtonOnClicked;
            WeakEventManager<Button, EventArgs>.AddHandler(button, "Clicked", ButtonOnClicked);
        }

        private void ButtonOnClicked(object sender, EventArgs args)
        {
            Console.WriteLine("Button clicked (window handler)");
        }

        ~Window()
        {
            Console.WriteLine("Window Finalize!!!");
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var button = new Button();
            var window = new Window(button);
            var windowRef = new WeakReference(window);
            button.Fire();
            Console.WriteLine($"Setting Window to null");
            window = null;

            FireGC();
            Console.WriteLine($"Is window weak reference alive: {windowRef.IsAlive}");
        }

        private static void FireGC()
        {
            Console.WriteLine("Starting GC");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Console.WriteLine("GC is done");
        }
    }
}