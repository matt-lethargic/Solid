using System.Collections.Generic;
using System.IO;
using Solid.S.C.Interfaces;

namespace Solid.S.C.Impls
{
    public class StreamQuoteDataProvider : IQuoteDataProvider
    {
        private readonly Stream _stream;

        public StreamQuoteDataProvider(Stream stream)
        {
            _stream = stream;
        }

        public IEnumerable<string> GetQuoteData()
        {
            //read rows
            var lines = new List<string>();
            using (var reader = new StreamReader(_stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            return lines;
        }
    }
}
