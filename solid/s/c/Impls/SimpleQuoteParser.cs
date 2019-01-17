using System.Collections.Generic;
using Solid.S.C.Interfaces;
using Solid.S.Models;

namespace Solid.S.C.Impls
{
    public class SimpleQuoteParser : IQuoteParser
    {
        private readonly IQuoteValidator _quoteValidator;
        private readonly IQuoteMapper _quoteMapper;

        public SimpleQuoteParser(IQuoteValidator quoteValidator, IQuoteMapper quoteMapper)
        {
            _quoteValidator = quoteValidator;
            _quoteMapper = quoteMapper;
        }


        public IEnumerable<Quote> Parse(IEnumerable<string> lines)
        {
            var quotes = new List<Quote>();

            //parse
            foreach (var line in lines)
            {
                var fields = line.Split(new char[] { ',' });

                if (!_quoteValidator.Validate(fields))
                {
                    continue;
                }

                var quote = _quoteMapper.Map(fields);

                quotes.Add(quote);
            }
            return quotes;
        }
    }
}
