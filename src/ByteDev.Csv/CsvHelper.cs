using System.Collections.Generic;

namespace ByteDev.Csv
{
    internal static class CsvHelper
    {
        private static readonly char[] CharsThatMustBeQuoted = { ',', '"', '\n' };

        private const string Quote = "\"";
        private const string EscapedQuote = "\"\"";

        public static IList<string> Escape(string[] lineValues)
        {
            var values = new List<string>();

            foreach (var value in lineValues)
            {
                values.Add(Escape(value));
            }
            return values;
        }

        public static string Escape(string value)
        {
            if (value.Contains(Quote))
            {
                value = value.Replace(Quote, EscapedQuote);
            }

            if (value.IndexOfAny(CharsThatMustBeQuoted) > -1)
            {
                value = Quote + value + Quote;
            }

            return value;
        }

        public static IList<string> Unescape(string[] lineValues)
        {
            var values = new List<string>();

            foreach (var value in lineValues)
            {
                values.Add(Unescape(value));
            }
            return values;
        }

        public static string Unescape(string value)
        {
            if (value.StartsWith(Quote) && value.EndsWith(Quote))
            {
                value = value.Substring(1, value.Length - 2);

                if (value.Contains(EscapedQuote))
                {
                    value = value.Replace(EscapedQuote, Quote);
                }
            }

            return value;
        }
    }
}