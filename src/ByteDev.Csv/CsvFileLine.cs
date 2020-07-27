using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ByteDev.Csv
{
    /// <summary>
    /// Represents a CSV file line.
    /// </summary>
    public class CsvFileLine : IEquatable<CsvFileLine>
    {
        private const string Delimiter = ",";

        private static readonly Regex RegExCsvSplitter = new Regex(@",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))");

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Csv.CsvFileLine" /> class.
        /// </summary>
        /// <param name="line">Line.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="line" /> is null.</exception>
        public CsvFileLine(string line)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));                

            Values = ConvertLineToValues(line);
            Line = line;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Csv.CsvFileLine" /> class.
        /// </summary>
        /// <param name="values">Array of values that represents a line.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="values" /> is null.</exception>
        public CsvFileLine(string[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            Line = ConvertValuesToLine(values);
            Values = values;
        }

        /// <summary>
        /// Line as an array of values.
        /// </summary>
        public string[] Values { get; }

        /// <summary>
        /// Line as a string.
        /// </summary>
        public string Line { get; }
        
        /// <summary>
        /// Indicates if the line is empty.
        /// </summary>
        public bool IsEmpty => Values == null || Values.Length < 1;

        /// <summary>
        /// Value count.
        /// </summary>
        public int ValuesCount => Values.Length;

        public override string ToString()
        {
            return Line;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if (ReferenceEquals(obj, this))
                return true;

            if (GetType() != obj.GetType())
                return false;

            return Equals(obj as CsvFileLine);
        }

        public bool Equals(CsvFileLine other)
        {
            if (other == null)
                return false;

            return Line == other.Line;
        }

        public override int GetHashCode()
        {
            return Line.GetHashCode();
        }
        
        private string ConvertValuesToLine(string[] values)
        {
            if (values == null || values.Length < 1)
            {
                return string.Empty;
            }

            return string.Join(Delimiter, CsvHelper.Escape(values));
        }

        private string[] ConvertLineToValues(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return new string[0];
            }

            var values = RegExCsvSplitter.Split(line);

            return CsvHelper.Unescape(values).ToArray();
        }
    }
}