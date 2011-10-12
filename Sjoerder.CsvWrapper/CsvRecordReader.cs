using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sjoerder.CsvWrapper
{
    using System.Collections;
    using System.IO;
    using LumenWorks.Framework.IO.Csv;

    public class CsvRecordReader : IDisposable, IEnumerable<CsvRecord>
    {
        private CsvReader csvReader;
        private readonly CsvHeaders headers;
        public bool SkipEmptyLines { get { return csvReader.SkipEmptyLines; } set { csvReader.SkipEmptyLines = value; } }
        public string DefaultHeaderName { get { return csvReader.DefaultHeaderName; } set { csvReader.DefaultHeaderName = value; } }
        public bool SupportsMultiline { get { return csvReader.SupportsMultiline; } set { csvReader.SupportsMultiline = value; } }
        public char Delimiter { get { return csvReader.Delimiter;  } }
        public int FieldCount { get { return csvReader.FieldCount;  } }
        public long CurrentRecordIndex { get { return csvReader.CurrentRecordIndex; } }
        public bool HasHeaders { get { return headers != null; } }

        public CsvHeaders GetFieldHeaders()
        {
            return headers;
        }

        /// <summary>
		/// Initializes a new instance of the CsvRecordReader class.
		/// </summary>
		/// <param name="reader">A <see cref="T:TextReader"/> pointing to the CSV file.</param>
		/// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
		/// <exception cref="T:ArgumentNullException">
		///		<paramref name="reader"/> is a <see langword="null"/>.
		/// </exception>
		/// <exception cref="T:ArgumentException">
		///		Cannot read from <paramref name="reader"/>.
		/// </exception>
		public CsvRecordReader(TextReader reader, bool hasHeaders)
			: this(reader, hasHeaders, CsvReader.DefaultDelimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultComment, ValueTrimmingOptions.UnquotedOnly, CsvReader.DefaultBufferSize)
		{
		}

		/// <summary>
		/// Initializes a new instance of the CsvRecordReader class.
		/// </summary>
		/// <param name="reader">A <see cref="T:TextReader"/> pointing to the CSV file.</param>
		/// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
		/// <param name="bufferSize">The buffer size in bytes.</param>
		/// <exception cref="T:ArgumentNullException">
		///		<paramref name="reader"/> is a <see langword="null"/>.
		/// </exception>
		/// <exception cref="T:ArgumentException">
		///		Cannot read from <paramref name="reader"/>.
		/// </exception>
		public CsvRecordReader(TextReader reader, bool hasHeaders, int bufferSize)
			: this(reader, hasHeaders, CsvReader.DefaultDelimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultComment, ValueTrimmingOptions.UnquotedOnly, bufferSize)
		{
		}

		/// <summary>
		/// Initializes a new instance of the CsvRecordReader class.
		/// </summary>
		/// <param name="reader">A <see cref="T:TextReader"/> pointing to the CSV file.</param>
		/// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
		/// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
		/// <exception cref="T:ArgumentNullException">
		///		<paramref name="reader"/> is a <see langword="null"/>.
		/// </exception>
		/// <exception cref="T:ArgumentException">
		///		Cannot read from <paramref name="reader"/>.
		/// </exception>
		public CsvRecordReader(TextReader reader, bool hasHeaders, char delimiter)
			: this(reader, hasHeaders, delimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultComment, ValueTrimmingOptions.UnquotedOnly, CsvReader.DefaultBufferSize)
		{
		}

		/// <summary>
		/// Initializes a new instance of the CsvRecordReader class.
		/// </summary>
		/// <param name="reader">A <see cref="T:TextReader"/> pointing to the CSV file.</param>
		/// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
		/// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
		/// <param name="bufferSize">The buffer size in bytes.</param>
		/// <exception cref="T:ArgumentNullException">
		///		<paramref name="reader"/> is a <see langword="null"/>.
		/// </exception>
		/// <exception cref="T:ArgumentException">
		///		Cannot read from <paramref name="reader"/>.
		/// </exception>
		public CsvRecordReader(TextReader reader, bool hasHeaders, char delimiter, int bufferSize)
			: this(reader, hasHeaders, delimiter, CsvReader.DefaultQuote, CsvReader.DefaultEscape, CsvReader.DefaultComment, ValueTrimmingOptions.UnquotedOnly, bufferSize)
		{
		}

		/// <summary>
		/// Initializes a new instance of the CsvRecordReader class.
		/// </summary>
		/// <param name="reader">A <see cref="T:TextReader"/> pointing to the CSV file.</param>
		/// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
		/// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
		/// <param name="quote">The quotation character wrapping every field (default is ''').</param>
		/// <param name="escape">
		/// The escape character letting insert quotation characters inside a quoted field (default is '\').
		/// If no escape character, set to '\0' to gain some performance.
		/// </param>
		/// <param name="comment">The comment character indicating that a line is commented out (default is '#').</param>
		/// <param name="trimmingOptions">Determines which values should be trimmed.</param>
		/// <exception cref="T:ArgumentNullException">
		///		<paramref name="reader"/> is a <see langword="null"/>.
		/// </exception>
		/// <exception cref="T:ArgumentException">
		///		Cannot read from <paramref name="reader"/>.
		/// </exception>
		public CsvRecordReader(TextReader reader, bool hasHeaders, char delimiter, char quote, char escape, char comment, ValueTrimmingOptions trimmingOptions)
			: this(reader, hasHeaders, delimiter, quote, escape, comment, trimmingOptions, CsvReader.DefaultBufferSize)
		{
		}

		/// <summary>
		/// Initializes a new instance of the CsvRecordReader class.
		/// </summary>
		/// <param name="reader">A <see cref="T:TextReader"/> pointing to the CSV file.</param>
		/// <param name="hasHeaders"><see langword="true"/> if field names are located on the first non commented line, otherwise, <see langword="false"/>.</param>
		/// <param name="delimiter">The delimiter character separating each field (default is ',').</param>
		/// <param name="quote">The quotation character wrapping every field (default is ''').</param>
		/// <param name="escape">
		/// The escape character letting insert quotation characters inside a quoted field (default is '\').
		/// If no escape character, set to '\0' to gain some performance.
		/// </param>
		/// <param name="comment">The comment character indicating that a line is commented out (default is '#').</param>
		/// <param name="trimmingOptions">Determines which values should be trimmed.</param>
		/// <param name="bufferSize">The buffer size in bytes.</param>
		/// <exception cref="T:ArgumentNullException">
		///		<paramref name="reader"/> is a <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="bufferSize"/> must be 1 or more.
		/// </exception>
        public CsvRecordReader(TextReader reader, bool hasHeaders, char delimiter, char quote, char escape, char comment, ValueTrimmingOptions trimmingOptions, int bufferSize)
		{
            this.csvReader = new CsvReader(reader, hasHeaders, delimiter, quote, escape, comment, trimmingOptions, bufferSize);
            if (hasHeaders)
            {
                var headersArray = csvReader.GetFieldHeaders();
                this.headers = new CsvHeaders(headersArray);
            }
		}

        public CsvRecord Read()
        {
            if (!csvReader.ReadNextRecord())
                return null;

            var fieldCount = csvReader.FieldCount;
            var values = new string[fieldCount];
            csvReader.CopyCurrentRecordTo(values);
            return new CsvRecord(headers, values);
        }

        public void Dispose()
        {
            csvReader.Dispose();
            csvReader = null;
        }

        public IEnumerator<CsvRecord> GetEnumerator()
        {
            return new CsvRecordReader.RecordEnumerator(this);
        }

        public class RecordEnumerator : IEnumerator<CsvRecord>
        {
            private CsvRecordReader csvRecordReader;
            private CsvRecord _current;

            public RecordEnumerator(CsvRecordReader csvRecordReader)
            {
                this.csvRecordReader = csvRecordReader;
            }

            public void Dispose()
            {
                _current = null;
                csvRecordReader = null;
            }

            public bool MoveNext()
            {
                _current = csvRecordReader.Read();
                return (_current != null);
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public CsvRecord Current
            {
                get { return _current; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
