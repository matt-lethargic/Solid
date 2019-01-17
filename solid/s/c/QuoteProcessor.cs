using Solid.S.C.Interfaces;

namespace Solid.S.C
{
    /// <summary>
    /// 
    /// Refactored for abstraction
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// Line:
    /// c676de6b-045a-b547-59d7-f98ab543dc87,1234,452.99
    /// 
    /// </remarks>
    public class QuoteProcessor
    {
        private readonly IQuoteDataProvider _quoteDataProvider;
        private readonly IQuoteParser _quoteParser;
        private readonly IQuoteStorage _quoteStorage;

        public QuoteProcessor(
            IQuoteDataProvider quoteDataProvider, 
            IQuoteParser quoteParser,
            IQuoteStorage quoteStorage)
        {
            _quoteDataProvider = quoteDataProvider;
            _quoteParser = quoteParser;
            _quoteStorage = quoteStorage;
        }

        
        public void ProcessQuotes()
        {
            var lines = _quoteDataProvider.GetQuoteData();
            var quotes = _quoteParser.Parse(lines);
            _quoteStorage.Save(quotes);
        }
    }
}