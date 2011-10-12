namespace Sjoerder.CsvWrapper
{
    public class CsvHeaders
    {
        private string[] headers;

        public CsvHeaders(string[] headers)
        {
            this.headers = headers;
        }

        /// <summary>
        /// Gets the field index for the provided header.
        /// </summary>
        /// <param name="header">The header to look for.</param>
        /// <returns>The field index for the provided header. -1 if not found.</returns>
        /// <exception cref="T:System.ComponentModel.ObjectDisposedException">
        ///	The instance has been disposed of.
        /// </exception>
        public int GetFieldIndex(string header)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                if (header == headers[i]) return i;
            }
            return -1;
        }

        public string this[int i]
        {
            get { return headers[i]; }
        }

        public int Length { get { return headers.Length; } }
    }
}