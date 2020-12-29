using System;

namespace LiskovSubstitution
{
    //Before

    public class BadRectangle
    {
        public int Width
        {
            get;
            set;
        }
        public int Height
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }

    public class BadSquare : BadRectangle
    {
        public new int Height
        {
            set
            {
                base.Height = value;
                base.Width = value;
            }
        }

        public new int Width
        {
            set
            {
                base.Height = value;
                base.Width = value;
            }
        }
    }
    
    //AFTER
    
    public class Rectangle
    {
        public virtual int Width
        {
            get;
            set;
        }
        public virtual int Height
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }

    public class Square : Rectangle
    {
        public override int Height
        {
            set
            {
                base.Height = value;
                base.Width = value;
            }
        }

        public override int Width
        {
            set
            {
                base.Height = value;
                base.Width = value;
            }
        }
    }
    
    internal class Program
    {
        public static int BadArea(BadRectangle r) => r.Width * r.Height;
        public static int Area(Rectangle r) => r.Width * r.Height;
        public static void Main(string[] args)
        {
            //Before
            BadRectangle badSquare = new BadSquare();
            badSquare.Width = 4;
            Console.WriteLine($"{badSquare} , area {BadArea(badSquare)}");
            
            //After
            Rectangle square = new Square();
            square.Width = 4;
            Console.WriteLine($"{square} , area {Area(square)}");

        }
    }
}