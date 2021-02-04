namespace ApplicationProgrammingInterface
{
    internal interface ISubclass
    {
        string Output();
    }

    internal class SubclassA : ISubclass
    {
        public string Output()
        {
            return new string('A', 1);
        }
    }

    internal class SubclassB : ISubclass
    {
        public string Output()
        {
            return new string('B', 1);
            ;
        }
    }

    internal class SubclassC : ISubclass
    {
        public string Output()
        {
            return new string('C', 1);
            ;
        }
    }

    public class SubclassAPI
    {
        public string Result()
        {
            var a = new SubclassA();
            var b = new SubclassB();
            var c = new SubclassC();
            var result = a.Output() + b.Output() + c.Output();
            return result;
        }
    }
}