using System;
using System.Collections.Generic;
using System.Text;

namespace Builder
{

    public class HTMLElement
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public List<HTMLElement> Elements = new List<HTMLElement> ();
        
        private int IndentSize = 2;

        public HTMLElement()
        {
        }

        public HTMLElement(string name, string text)
        {
            Name = name;
            Text = text;
        }

        private string ToStringImpl(int indent)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{new string(' ', indent * IndentSize)}<{Name}>");
            if (!string.IsNullOrWhiteSpace(Text))
            {
                sb.AppendLine($"{new string(' ', indent * (IndentSize + 1))}{Text}");
            }

            foreach (var element in Elements)
            {
                var childElement = element.ToStringImpl(indent + 1);
                sb.Append(childElement);
            }
            
            sb.AppendLine($"{new string(' ', indent * IndentSize)}</{Name}>");
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringImpl(0);
        }
    }

    public class HTMLBuilder
    {
        HTMLElement root = new HTMLElement();

        public HTMLBuilder(string rootName)
        {
            root.Name = rootName;
        }

        public HTMLBuilder AddChild(string name, string text)
        {
            var child = new HTMLElement(name, text);
            root.Elements.Add(child);
            return this;
        }

        public override string ToString()
        {
            return root.ToString();
        }

        public void Clear()
        {
            root = new HTMLElement();
        }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = new HTMLBuilder("ul");
            builder.AddChild("li","hello").AddChild("li","world");
            Console.WriteLine(builder.ToString());
        }
    }
}