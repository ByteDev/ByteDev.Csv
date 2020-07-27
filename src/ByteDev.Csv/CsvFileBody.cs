using System.Collections.Generic;

namespace ByteDev.Csv
{
    /// <summary>
    /// Represents a CSV file body.
    /// </summary>
    public class CsvFileBody
    {
        private IList<CsvFileLine> _lines;

        /// <summary>
        /// Lines.
        /// </summary>
        public IList<CsvFileLine> Lines
        {
            get => _lines ?? (_lines = new List<CsvFileLine>());
            set => _lines = value;
        }

        /// <summary>
        /// Indicates if lines are present.
        /// </summary>
        public bool HasLines => Lines.Count > 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Csv.CsvFileBody" /> class.
        /// </summary>
        public CsvFileBody()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Csv.CsvFileBody" /> class.
        /// </summary>
        /// <param name="lines">Lines.</param>
        public CsvFileBody(IList<CsvFileLine> lines)
        {
            _lines = lines;
        }

        /// <summary>
        /// Indicates if a line of values already exists in the body.
        /// </summary>
        /// <param name="line">Line.</param>
        /// <returns>True the line exists; otherwise false.</returns>
        public bool Contains(CsvFileLine line)
        {
            if (!HasLines)
                return false;

            return Lines.Contains(line);
        }
    }
}