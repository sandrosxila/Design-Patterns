using System;

namespace Observer
{
    public class FallsIllEventArgs
    {
        public string Address;
    }
    public class Person
    {
        public void CatchACold()
        {
            FallsIll?.Invoke(this,new FallsIllEventArgs{Address = "123 London Road"});
        }
        
        public event EventHandler<FallsIllEventArgs> FallsIll;
        
    }
    
    internal class Program
    {
        private static void CallDoctor(object sender,FallsIllEventArgs args)
        {
            Console.WriteLine($"A doctor has been called to {args.Address}");
        }
        public static void Main(string[] args)
        {
            var person = new Person();
            person.FallsIll += CallDoctor;
            person.CatchACold();
        }
        
    }
}