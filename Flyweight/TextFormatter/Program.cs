using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TextFormatter
{
    class FormattedText
    {
        private readonly string plainText;
        private bool[] capitalize;

        public FormattedText(string plainText)
        {
            this.plainText = plainText;
            capitalize = new bool[plainText.Length];
        }

        public void Capitalize(int start, int end)
        {
            for (int i = start; i <= end; i++)
                capitalize[i] = true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < plainText.Length; i++)
            {
                var c = plainText[i];
                sb.Append(capitalize[i] ? char.ToUpper(c) : c);
            }

            return sb.ToString();
        }
    }

    public static class ListExtensionMethod
    {
        public static int IndexInSorted<T>(this IList<T> list, T item) where T : IComparable<T>
        {
            if (list.Count == 0)
                return -1;
            if (item.CompareTo(list[list.Count - 1]) > 0)
                return list.Count;
            int l = 0;
            int r = list.Count - 1;
            while (l != r)
            {
                int mid = (l + r) >> 1;
                if (item.CompareTo(list[mid]) > 0)
                    l = mid + 1;
                else r = mid;
            }

            return r;
        }
    }

    class BetterFormattedText
    {
        private class Range : IComparable<Range>
        {
            public int start;
            public int end;

            public Range(int start, int end)
            {
                this.start = start;
                this.end = end;
            }

            public int CompareTo(Range other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;
                var startComparison = start.CompareTo(other.start);
                if (startComparison != 0) return startComparison;
                return end.CompareTo(other.end);
            }

            public override string ToString()
            {
                return $"({nameof(start)}: {start}, {nameof(end)}: {end})";
            }
        }

        private readonly string plainText;
        private List<Range> ranges = new List<Range>();

        public BetterFormattedText(string plainText)
        {
            this.plainText = plainText;
        }

        public void Capitalize(int start, int end)
        {
            int index = ranges.IndexInSorted(new Range(start, end));

            if (index == -1)
            {
                ranges.Add(new Range(start, end));
                return;
            }

            for (int i = index - 1; i >= 0; i--)
            {
                if (ranges[i].start > end || ranges[i].end < start)
                    break;
                start = Math.Min(start, ranges[i].start);
                end = Math.Max(end, ranges[i].end);

                ranges.RemoveAt(i);
            }

            index = ranges.IndexInSorted(new Range(start, end));

            for (int i = index; i < ranges.Count; i++)
            {
                if (ranges[i].start > end || ranges[i].end < start)
                    break;

                if (ranges[i].end < end)
                {
                    ranges.RemoveAt(i);
                    i--;
                    continue;
                }

                if (ranges[i].start <= end)
                {
                    start = Math.Min(start, ranges[i].start);
                    end = Math.Max(end, ranges[i].end);
                    ranges.RemoveAt(i);
                    i--;
                }
            }

            index = ranges.IndexInSorted(new Range(start, end));
            ranges.Insert(index, new Range(start, end));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ranges.Count; i++)
            {
                if (i > 0)
                {
                    for (int j = ranges[i - 1].end + 1; j < ranges[i].start; j++)
                        sb.Append(plainText[j]);
                }
                else
                {
                    for (int j = 0; j < ranges[i].start; j++)
                        sb.Append(plainText[j]);
                }

                for (int j = ranges[i].start; j <= ranges[i].end; j++)
                    sb.Append(char.ToUpper(plainText[j]));

                if (i == ranges.Count - 1)
                {
                    for (int j = ranges[i].end + 1; j < plainText.Length; j++)
                        sb.Append(plainText[j]);
                }
            }

            return sb.ToString();
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var ft = new FormattedText("hello world from new york!!! ha ha ha !!!");
            ft.Capitalize(2, 3);
            ft.Capitalize(6, 7);
            ft.Capitalize(9, 11);
            ft.Capitalize(14, 15);
            ft.Capitalize(5, 15);
            ft.Capitalize(20, 30);
            ft.Capitalize(18, 19);
            ft.Capitalize(19, 20);
            ft.Capitalize(12, 20);
            ft.Capitalize(6, 23);
            ft.Capitalize(6, 20);
            ft.Capitalize(0, 0);

            Console.WriteLine(ft);

            // using FlyWeight Design Pattern
            var bft = new BetterFormattedText("hello world from new york!!! ha ha ha !!!");
            bft.Capitalize(2, 3);
            bft.Capitalize(6, 7);
            bft.Capitalize(9, 11);
            bft.Capitalize(14, 15);
            bft.Capitalize(5, 15);
            bft.Capitalize(20, 30);
            bft.Capitalize(18, 19);
            bft.Capitalize(19, 20);
            bft.Capitalize(12, 20);
            bft.Capitalize(6, 23);
            bft.Capitalize(6, 20);
            bft.Capitalize(0, 0);

            Console.WriteLine(bft);
        }
    }
}