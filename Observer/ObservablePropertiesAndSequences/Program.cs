using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ObservablePropertiesAndSequences.Annotations;

namespace ObservablePropertiesAndSequences
{
    public class Market
    {
        public BindingList<float> Prices = new BindingList<float>();

        public void AddPrice(float price)
        {
            Prices.Add(price);
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var market = new Market();
            market.Prices.ListChanged += ((sender, eventArgs) =>
            {
                if (eventArgs.ListChangedType == ListChangedType.ItemAdded)
                {
                    float price = ((BindingList<float>) sender)[eventArgs.NewIndex];
                    Console.WriteLine($"Binding List got a price of {price}");
                }
            });
            market.AddPrice(123);
        }
    }
}