using System;

namespace Proxy
{
    public interface ICar
    {
        void Drive();
    }
    public class Car : ICar
    {
        public void Drive()
        {
            Console.WriteLine("The car is being driven...");
        }
    }

    public class Driver
    {
        public int Age { get; set; }

        public Driver(int age)
        {
            Age = age;
        }
    }

    public class CarProxy : ICar
    {
        private Driver driver;
        private Car car = new Car();

        public CarProxy(Driver driver)
        {
            this.driver = driver;
        }
        
        public void Drive()
        {
            if(driver.Age >= 16)
                car.Drive();
            else
                Console.WriteLine("Too young");
        }
    }
    internal class Program
    {
        public static void Main(string[] args)
        {
            ICar carProxy = new CarProxy(new Driver(22));
            carProxy.Drive();
        }
    }
}