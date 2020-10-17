using System;

namespace Factory
{
    internal class Program
    {

        public class Point
        {
            private double x, y;

            private Point(double x, double y)
            {
                this.x = x;
                this.y = y;
            }

            public override string ToString()
            {
                return $"{nameof(x)}: {x}, {nameof(y)}: {y}";
            }

            public static Point origin = new Point(0, 0);
            
            internal static class Factory
            {
                public static Point NewCartesianPoint(double x, double y)
                {
                    return new Point(x, y);
                }

                public static Point NewPolarPoint(double rho, double theta)
                {
                    return new Point(rho * Math.Sin(theta), rho * Math.Cos(theta));
                }
            }
        }

        class MyClass
        {
            private Point x = Point.Factory.NewCartesianPoint(0,0);
            
        }
        
        public static void Main(string[] args)
        {
            var point = Point.Factory.NewPolarPoint(1, Math.PI / 2);
            Console.WriteLine(point.ToString());
        }
    }
}