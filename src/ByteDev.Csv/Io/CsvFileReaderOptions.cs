namespace ByteDev.Csv.Io
{
    /// <summary>
    /// Represents options for reading CSV files.
    /// </summary>
    public class CsvFileReaderOptions
    {
        /// <summary>
        /// Should the read in file be treated as though it has a header.
        /// </summary>
        public bool HasHeader { get; set; }

        /// <summary>
        /// If set to true all read in values will be trimmed.
        /// </summary>
        public bool EnableTrimValues { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Csv.Io.CsvFileReaderOptions" /> class.
        /// </summary>
        public CsvFileReaderOptions()
        {
            HasHeader = true;
            EnableTrimValues = false;
        }
    }
}