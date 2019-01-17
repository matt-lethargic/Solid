using System;

namespace Solid.S.D
{
    public class LoggingDecorator : IQuoteProcessor
    {
        private readonly IQuoteProcessor _quoteProcessor;
        private readonly ISomeLogger _logger;

        public LoggingDecorator(IQuoteProcessor quoteProcessor, ISomeLogger logger)
        {
            _quoteProcessor = quoteProcessor;
            _logger = logger;
        }
        public void ProcessQuotes()
        {
            _logger.Log("Some message");

            try
            {
                _quoteProcessor.ProcessQuotes();
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message);
                throw;
            }
        }
    }
}
