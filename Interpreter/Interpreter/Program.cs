using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    public interface IElement
    {
        int Value { get; }
    }

    public class Integer : IElement
    {
        public Integer(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class BinaryExpression : IElement, ICloneable
    {

        public enum Type
        {
            Empty,Addition, Subtraction
        }

        public Type MyType = Type.Empty;
        public IElement Left, Right;

        public int Value
        {
            get
            {
                switch (MyType)
                {
                    case Type.Addition:
                        return Left.Value + Right.Value;
                    case Type.Subtraction:
                        return Left.Value - Right.Value;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public object Clone()
        {
            var leftChild = new BinaryExpression();
            leftChild.Left = Left;
            leftChild.Right = Right;
            leftChild.MyType = MyType;
            return leftChild;
        }
    }
    public class Token
    {
        public enum Type
        {
            Integer,
            Plus,
            Minus,
            Lparen,
            Rparen
        }

        public Type MyType;
        public string Text;

        public Token(Type myType, string text)
        {
            MyType = myType;
            this.Text = text;
        }

        public override string ToString()
        {
            return $"{Text}";
        }
    }

    internal class Program
    {
        static IElement Parse(IReadOnlyList<Token> tokens)
        {
            var result = new BinaryExpression();
            bool haveLHS = false;
            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];

                switch (token.MyType)
                {
                    case Token.Type.Integer:
                        if(!haveLHS)
                        {
                            result.Left = new Integer(int.Parse(token.Text));
                            haveLHS = true;
                        }
                        else
                        {
                            result.Right= new Integer(int.Parse(token.Text));
                        }
                        break;
                    case Token.Type.Plus:
                        if(result.MyType == BinaryExpression.Type.Empty)
                            result.MyType = BinaryExpression.Type.Addition;
                        else
                        {
                            result.Left = (BinaryExpression)result.Clone();
                            result.MyType = BinaryExpression.Type.Addition;
                            result.Right = null;
                            haveLHS = true;
                        }
                        break;
                    case Token.Type.Minus:
                        if(result.MyType == BinaryExpression.Type.Empty)
                            result.MyType = BinaryExpression.Type.Subtraction;
                        else
                        {
                            result.Left = (BinaryExpression)result.Clone();
                            result.MyType = BinaryExpression.Type.Subtraction;
                            result.Right = null;
                            haveLHS = true;
                        }
                        break;
                    case Token.Type.Lparen:
                        int j = i + 1;
                        for (; j < tokens.Count && tokens[j].MyType != Token.Type.Rparen; j++);
                        var subexpression = tokens.Skip(i + 1).Take(j - i - 1).ToList();
                        var element = Parse(subexpression);
                        if (!haveLHS)
                        {
                            result.Left = element;
                            haveLHS = true;
                        }
                        else
                        {
                            result.Right = element;
                        }
                        i = j;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"{i}");
                }
            }

            return result;
        }
        static List<Token> Lex(string input)
        {
            var result = new List<Token>();
            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '+':
                        result.Add(new Token(Token.Type.Plus, "+"));
                        break;
                    case '-':
                        result.Add(new Token(Token.Type.Minus, "-"));
                        break;
                    case '(':
                        result.Add(new Token(Token.Type.Lparen, "("));
                        break;
                    case ')':
                        result.Add(new Token(Token.Type.Rparen, ")"));
                        break;
                    default:
                        var sb = new StringBuilder(input[i].ToString());
                        for (int j = i + 1; j < input.Length && char.IsDigit(input[j]); ++j)
                        {
                            sb.Append(input[j]);
                            i++;
                        }

                        result.Add(new Token(Token.Type.Integer, sb.ToString()));
                        break;
                }
            }

            return result;
        }

        public static void Main(string[] args)
        {
            string input = "(13+4-0)-(12+1)+9-5-1+(6-5)";
            var tokens = Lex(input);
            var parsed = Parse(tokens);
            Console.WriteLine($"{input} = {parsed.Value}");
        }
    }
}