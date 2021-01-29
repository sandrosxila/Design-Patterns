using static System.Console;
namespace Bridge
{
    public interface IRenderer
    {
        void RenderCircle(float radius);
    }

    public class VectorRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            WriteLine($"Drawing a circle of radius: {radius}");
        }
    }

    public class RasterRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            WriteLine($"Drawing pixels for a circle with radius: {radius}");
        }
    }

    public abstract class Shape
    {
        protected IRenderer renderer;

        protected Shape(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        public abstract void Draw();
        public abstract void Resize(float factor);
    }

    public class Circle : Shape
    {
        private float radius;
        public Circle(IRenderer renderer,float radius) : base(renderer)
        {
            this.radius = radius;
        }

        public override void Draw()
        {
            renderer.RenderCircle(radius);
        }

        public override void Resize(float factor)
        {
            radius *= factor;
        }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            // without engaging renderer in inheritance tree
            // it's passed as a parameter 
            // so we don't need to create
            // CircleVectorRenderer and CircleRasterRenderer
            // as a child classes of Circle
            
            // IRenderer renderer = new RasterRenderer();
            IRenderer renderer = new VectorRenderer();
            var circle = new Circle(renderer, 5);
            circle.Draw();
        }
    }
}