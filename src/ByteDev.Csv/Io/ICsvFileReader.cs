using System.Collections.Generic;

namespace ByteDev.Csv.Io
{
    /// <summary>
    /// Represents a CSV file reader interface.
    /// </summary>
    public interface ICsvFileReader
    {
        /// <summary>
        /// Reads the entire file into a <see cref="CsvFile"/> object.
        /// </summary>
        /// <param name="file">Path to CSV file.</param>
        /// <param name="options">File reading options.</param>
        /// <returns><see cref="CsvFile"/> object.</returns>
        CsvFile ReadFile(string file, CsvFileReaderOptions options);

        /// <summary>
        /// Reads the header (first line) of csv file.
        /// </summary>
        /// <param name="file">Path to CSV file.</param>
        /// <returns><see cref="CsvFileLine"/> object.</returns>
        CsvFileLine ReadHeader(string file);

        /// <summary>
        /// Reads the body values from a CSV file.
        /// </summary>
        /// <param name="file">The csv file name.</param>
        /// <param name="options">File reading options.</param>
        /// <returns>List of <see cref="CsvFileLine"/> objects.</returns>
        IList<CsvFileLine> ReadBody(string file, CsvFileReaderOptions options);
    }
}