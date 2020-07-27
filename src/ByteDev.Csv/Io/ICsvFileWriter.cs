using System.Collections.Generic;

namespace ByteDev.Csv.Io
{
    /// <summary>
    /// Represents a CSV file writer interface.
    /// </summary>
    public interface ICsvFileWriter
    {
        /// <summary>
        /// Writes a <paramref name="csvFile" /> to a file.
        /// </summary>
        /// <param name="file">Path to CSV file.</param>
        /// <param name="csvFile">CSV file object to write.</param>
        void Write(string file, CsvFile csvFile);

        /// <summary>
        /// Writes a header to a file.
        /// </summary>
        /// <param name="file">Path to CSV file.</param>
        /// <param name="header">CSV file header to write.</param>
        void WriteHeader(string file, CsvFileLine header);

        /// <summary>
        /// Write a body to a file.
        /// </summary>
        /// <param name="file">Path to CSV file.</param>
        /// <param name="body">CSV file body to write.</param>
        /// <param name="existingHasHeader">True the existing CSV file has a header; otherwise does not have a header.</param>
        void WriteBody(string file, CsvFileBody body, bool existingHasHeader = true);

        /// <summary>
        /// Write a line to a CSV file.
        /// </summary>
        /// <param name="file">Path to CSV file.</param>
        /// <param name="line">CSV line to write.</param>
        void AppendLine(string file, CsvFileLine line);

        /// <summary>
        /// Write lines to a CSV file.
        /// </summary>
        /// <param name="file">Path to CSV file.</param>
        /// <param name="lines">CSV lines to write.</param>
        void AppendLines(string file, IList<CsvFileLine> lines);
    }
}