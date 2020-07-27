using System;
using System.Collections.Generic;
using System.IO;

namespace ByteDev.Csv.Io
{
    /// <summary>
    /// Represents a writer of CSV content.
    /// </summary>
    public class CsvFileWriter : ICsvFileWriter
    {
        private readonly ICsvFileReader _csvReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Csv.Io.CsvFileWriter" /> class.
        /// </summary>
        public CsvFileWriter()
        {
            _csvReader = new CsvFileReader();
        }

        /// <summary>
        /// Writes a <paramref name="csvFile" /> to a file.
        /// </summary>
        /// <param name="file">Path to CSV file.</param>
        /// <param name="csvFile">CSV file object to write.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="csvFile" /> is null.</exception>
        public void Write(string file, CsvFile csvFile)
        {
            if (csvFile == null)
                throw new ArgumentNullException(nameof(csvFile));

            if (csvFile.HasHeader)
                WriteHeader(file, csvFile.Header);

            if (csvFile.HasBody)
                WriteBody(file, csvFile.Body);
        }

        /// <summary>
        /// Writes a header to a file.
        /// </summary>
        /// <param name="file">Path to CSV file.</param>
        /// <param name="header">CSV file header to write.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="header" /> is null.</exception>
        public void WriteHeader(string file, CsvFileLine header)
        {
            if(header == null)
                throw new ArgumentNullException(nameof(header));    

            var body = ReadBody(file);

            using (var writer = new StreamWriter(file))
            {
                writer.WriteLine(header);

                foreach (var line in body)
                {
                    writer.WriteLine(line.Line);    
                }
            }
        }

        /// <summary>
        /// Write a body to a file.
        /// </summary>
        /// <param name="file">Path to CSV file.</param>
        /// <param name="body">CSV file body to write.</param>
        /// <param name="existingHasHeader">True the existing CSV file has a header; otherwise does not have a header.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="body" /> is null.</exception>
        public void WriteBody(string file, CsvFileBody body, bool existingHasHeader = true)
        {
            if (body == null)
                throw new ArgumentNullException(nameof(body));

            if (!body.HasLines)
                return;

            CsvFileLine header = null;

            if (existingHasHeader)
                header = ReadHeader(file);

            using (var writer = new StreamWriter(file))
            {
                if (header != null)
                {
                    writer.WriteLine(header);
                }

                foreach (var line in body.Lines)
                {
                    writer.WriteLine(line.Line);
                }
            }
        }

        /// <summary>
        /// Write a line to a CSV file.
        /// </summary>
        /// <param name="file">Path to CSV file.</param>
        /// <param name="line">CSV line to write.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="line" /> is null.</exception>
        public void AppendLine(string file, CsvFileLine line)
        {
            if(line == null)
                throw new ArgumentNullException(nameof(line));

            using (var writer = new StreamWriter(file, true))
            {
                writer.WriteLine(line);
            }
        }

        /// <summary>
        /// Write lines to a CSV file.
        /// </summary>
        /// <param name="file">Path to CSV file.</param>
        /// <param name="lines">CSV lines to write.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="file" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="lines" /> is null.</exception>
        public void AppendLines(string file, IList<CsvFileLine> lines)
        {
            if(file == null)
                throw new ArgumentNullException(nameof(file));

            if(lines == null)
                throw new ArgumentNullException(nameof(lines));

            if (lines.Count < 1)
                return;

            using (var writer = new StreamWriter(file, true))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }   
            }
        }

        private CsvFileLine ReadHeader(string file)
        {
            try
            {
                return _csvReader.ReadHeader(file);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        private IList<CsvFileLine> ReadBody(string file)
        {
            try
            {
                return _csvReader.ReadBody(file, new CsvFileReaderOptions());
            }
            catch (FileNotFoundException)
            {
                return new List<CsvFileLine>();
            }
        }
    }
}