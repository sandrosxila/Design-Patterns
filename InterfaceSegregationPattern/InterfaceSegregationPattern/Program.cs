namespace InterfaceSegregationPattern
{
    //Before

    public class Document
    {
        
    }
    
    public interface MultiFuncPrinter
    {
        void Print(Document d);
        void Scan(Document d);
        void Fax(Document d);
    }

    public class ModernPrinter : MultiFuncPrinter
    {
        public void Print(Document d)
        {
            // works
        }

        public void Scan(Document d)
        {
            //works
        }

        public void Fax(Document d)
        {
            //works
        }
    }

    public class OldPrinter : MultiFuncPrinter
    {
        public void Scan(Document d)
        {
            // works
        }

        public void Print(Document d)
        {
            throw new System.NotImplementedException();
        }

        public void Fax(Document d)
        {
            throw new System.NotImplementedException();
        }
    }
    
    // AFTER

    public interface IPrint
    {
        void Print(Document d);
    }
    
    public interface IScan
    {
        void Scan(Document d);
    }
    
    public interface IFax
    {
        void Fax(Document d);
    }

    public interface IMultiFunctionDevice : IPrint,IScan
    {
    }

    public struct MultiFunctionMachine : IMultiFunctionDevice
    {
        public void Print(Document d)
        {
            //works
        }

        public void Scan(Document d)
        {
            //works
        }
    }

    public class PrintOnlyMachine : IPrint
    {
        public void Print(Document d)
        {
            //works
        }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
        }
    }
}