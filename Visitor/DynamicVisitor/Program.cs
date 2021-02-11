using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Visitor
{
    public abstract class Expression
    {
    }

    public class DoubleExpression : Expression
    {
        public double Value { get; set; }

        public DoubleExpression(double value)
        {
            this.Value = value;
        }
    }

    public class AdditionExpression : Expression
    {
        public Expression Left, Right;

        public AdditionExpression(Expression left, Expression right)
        {
            this.Left = left ?? throw new ArgumentNullException(nameof(left));
            this.Right = right ?? throw new ArgumentNullException(nameof(right));
        }
    }

    public class ExpressionPrinter
    {
        public void Print(DoubleExpression de, StringBuilder sb)
        {
            sb.Append(de.Value);
        }
        public void Print(AdditionExpression ae, StringBuilder sb)
        {
            sb.Append("(");
            Print((dynamic)ae.Left,sb);
            sb.Append("+");
            Print((dynamic)ae.Right,sb);
            sb.Append(")");
        }
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            var e = new AdditionExpression(
                new DoubleExpression(1),
                new AdditionExpression(
                    new DoubleExpression(2),
                    new DoubleExpression(3)
                )
            );
            var ep = new ExpressionPrinter();
            var sb = new StringBuilder();
            ep.Print(e, sb);
            Console.WriteLine(sb);
        }
    }
}