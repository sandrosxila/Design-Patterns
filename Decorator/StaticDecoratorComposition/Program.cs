using System;

namespace StaticDecoratorComposition
{
    
    public abstract class Shape
    {
        public abstract string AsString();
    }
    
    public class Circle : Shape
    {
        private float radius;


        public Circle() : this(0)
        {
            
        }
        public Circle(float radius)
        {
            this.radius = radius;
        }

        public void Resize(float factor)
        {
            radius *= factor;
        }

        public override string AsString() => $"A circle with Radius: {radius}";
    }

    public class Square : Shape
    {
        private float side;

        public Square() : this(0)
        {
            
        }
        public Square(float side)
        {
            this.side = side;
        }


        public override string AsString() => $"A square with Side: {side}";
    }

    public class ColoredShape<T> : Shape where T : Shape, new()
    {
        private string color;
        private T shape = new T();

        public ColoredShape() : this("black")
        {
            
        }
        
        public ColoredShape(string color)
        {
            this.color = color;
        }
        public override string AsString() => $"{shape.AsString()} has the color: {color}";

    }
    
    public class TransparentShape<T> : Shape where T : Shape, new()
    {
        private float transparency;
        private T shape = new T();

        public TransparentShape() : this(0)
        {
            
        }
        
        public TransparentShape(float transparency)
        {
            this.transparency = transparency;
        }
        public override string AsString() => $"{shape.AsString()} has the transparency: {transparency * 100.0f}%";

    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            var redSquare = new ColoredShape<Square>("red");
            Console.WriteLine(redSquare.AsString());

            var circle = new TransparentShape<ColoredShape<Circle>>(0.4f);
            Console.WriteLine(circle.AsString());
        }
    }

}