using System;
using System.Collections.Generic;
using System.IO;

namespace ByteDev.Csv.Io
{
    /// <summary>
    /// Represents a reader of CSV content.
    /// </summary>
    public class CsvFileReader : ICsvFileReader
    {
        /// <summary>
        /// Reads the entire file into a <see cref="CsvFile"/> object.
        /// </summary>
        /// <param name="file">Path to CSV file.</param>
        /// <param name="options">File reading options.</param>
        /// <returns><see cref="ByteDev.Csv.CsvFile"/> object.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="options" /> is null.</exception>
        public CsvFile ReadFile(string file, CsvFileReaderOptions options)
        {
            if(options == null)
                throw new ArgumentNullException(nameof(options));
            
            var header = options.HasHeader ? ReadHeader(file) : null;
            var body = new CsvFileBody { Lines = ReadBody(file, options) };

            return new CsvFile(header, body);
        }

        /// <summary>
        /// Reads the header (first line) of csv file.
        /// </summary>
        /// <param name="file">Path to CSV file.</param>
        /// <returns><see cref="ByteDev.Csv.CsvFileLine"/> object.</returns>
        public CsvFileLine ReadHeader(string file)
        {
            using (var reader = new StreamReader(File.OpenRead(file)))
            {
                var line = reader.ReadLine();

                if (line == null)
                {
                    return null;
                }

                return new CsvFileLine(line);
            }
        }

        /// <summary>
        /// Reads the body values from a CSV file.
        /// </summary>
        /// <param name="file">The csv file name.</param>
        /// <param name="options">File reading options.</param>
        /// <returns>List of <see cref="ByteDev.Csv.CsvFileLine"/> objects.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="options" /> is null.</exception>
        public IList<CsvFileLine> ReadBody(string file, CsvFileReaderOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var values = ReadCsvFile(file, options);

            if (options.HasHeader)
            {
                RemoveHeader(values);
            }

            return values;
        }

        private static IList<CsvFileLine> ReadCsvFile(string file, CsvFileReaderOptions options)
        {
            var fileLines = new List<CsvFileLine>();

            using (var reader = new StreamReader(File.OpenRead(file)))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (line == null)
                        continue;
                    
                    var csvFileLine = new CsvFileLine(line);

                    if (options.EnableTrimValues)
                    {
                        TrimValues(csvFileLine);
                    }

                    fileLines.Add(csvFileLine);                    
                }
            }

            return fileLines;
        }

        private static void TrimValues(CsvFileLine csvFileLine)
        {
            for (var i = 0; i < csvFileLine.Values.Length; i++)
            {
                csvFileLine.Values[i] = csvFileLine.Values[i].Trim();
            }
        }

        private static void RemoveHeader(IList<CsvFileLine> values)
        {
            if (values.Count > 0)
            {
                values.RemoveAt(0);
            }
        }
    }
}
