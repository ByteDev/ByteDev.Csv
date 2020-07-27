using System;
using System.Linq;

namespace ByteDev.Csv
{
    /// <summary>
    /// Represents a CSV file.
    /// </summary>
    public class CsvFile
    {
        /// <summary>
        /// Header line.
        /// </summary>
        public CsvFileLine Header { get; }

        /// <summary>
        /// Body.
        /// </summary>
        public CsvFileBody Body { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Csv.CsvFile" /> class.
        /// </summary>
        /// <param name="header">Header line.</param>
        public CsvFile(CsvFileLine header) : this(header, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Csv.CsvFile" /> class.
        /// </summary>
        /// <param name="body">Body.</param>
        public CsvFile(CsvFileBody body) : this(null, body)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Csv.CsvFile" /> class.
        /// </summary>
        /// <param name="header">Header line.</param>
        /// <param name="body">Body.</param>
        public CsvFile(CsvFileLine header, CsvFileBody body)
        {
            Header = header;
            Body = body;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Csv.CsvFile" /> class.
        /// </summary>
        public CsvFile() : this(null, null)
        {
        }

        /// <summary>
        /// Indicates if a header is present.
        /// </summary>
        public bool HasHeader => Header != null;

        /// <summary>
        /// Indicates if a body is present.
        /// </summary>
        public bool HasBody => Body != null;

        /// <summary>
        /// Body line count.
        /// </summary>
        public int BodyLineCount => HasBody ? Body.Lines.Count : 0;

        /// <summary>
        /// Performs a simple verification of the body data.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">Has no body.</exception>
        /// <exception cref="T:System.InvalidOperationException">Header values count does not match the body.</exception>
        public void VerifyBody()
        {
            if (!HasBody)
                throw new InvalidOperationException("No body exists");

            if (HasHeader)
            {
                if (Header.ValuesCount != Body.Lines.First().ValuesCount)
                    throw new InvalidOperationException($"Header values count ({Header.ValuesCount}) does not match body line value count ({BodyLineCount}");
            }
        }
    }
}