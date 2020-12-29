using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SOLID
{
    public enum Color
    {
        Red,
        Green,
        Blue
    }

    public enum Size
    {
        Small,
        Big,
        Huge
    }

    public class Product
    {
        public string Name;
        public Color Color;
        public Size Size;

        public Product(string name, Color color, Size size)
        {
            Name = name;
            Color = color;
            Size = size;
        }
    }

    //BEFORE
    public class ProductFilter
    {
        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (var product in products)
            {
                if (product.Color == color)
                {
                    yield return product;
                }
            }
        }

        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var product in products)
            {
                if (product.Size == size)
                {
                    yield return product;
                }
            }
        }

        public IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
        {
            foreach (var product in products)
            {
                if (product.Size == size && product.Color == color)
                {
                    yield return product;
                }
            }
        }
    }

    //AFTER
    public interface ISpecification<T>
    {
        bool IsSatisfied(T item);
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> specification);
    }

    public class ColorSpecification : ISpecification<Product>
    {
        private Color color;

        public ColorSpecification(Color color)
        {
            this.color = color;
        }

        public bool IsSatisfied(Product item)
        {
            return item.Color == color;
        }
    }

    public class SizeSpecification : ISpecification<Product>
    {
        private Size size;

        public SizeSpecification(Size size)
        {
            this.size = size;
        }

        public bool IsSatisfied(Product item)
        {
            return item.Size == size;
        }
    }

    public class AndSpecification<T> : ISpecification<T>
    {
        private ISpecification<T> first, second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first;
            this.second = second;
        }

        public bool IsSatisfied(T item)
        {
            return first.IsSatisfied(item) && second.IsSatisfied(item);
        }
    }

    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> specification)
        {
            foreach (var item in items)
            {
                if (specification.IsSatisfied(item))
                {
                    yield return item;
                }
            }
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            //BEFORE
            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Big);
            var house = new Product("House", Color.Blue, Size.Big);

            Product[] products = {apple, tree, house};

            var pf = new ProductFilter();

            foreach (var p in pf.FilterByColor(products, Color.Green))
            {
                Console.WriteLine($"{p.Name} is Green");
            }

            //AFTER

            var bf = new BetterFilter();

            foreach (var product in bf.Filter(
                    products,
                    new AndSpecification<Product>(new ColorSpecification(Color.Green), new SizeSpecification(Size.Big)))
                )
            {
                Console.WriteLine($"{product.Name} is Green and Big");
            }
        }
    }
}