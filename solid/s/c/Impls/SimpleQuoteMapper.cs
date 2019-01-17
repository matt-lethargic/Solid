using System;
using Solid.S.C.Interfaces;
using Solid.S.Models;

namespace Solid.S.C.Impls
{
    public class SimpleQuoteMapper : IQuoteMapper
    {
        private const int MonthsInYear = 12;

        public Quote Map(string[] fields)
        {
            //cal values and create
            Guid quoteId = Guid.Parse(fields[0]);
            int customerId = int.Parse(fields[1]);
            decimal monthlyPrice = decimal.Parse(fields[2]);
            decimal yearlyPrice = monthlyPrice * MonthsInYear;

            var quote = new Quote
            {
                QuoteId = quoteId,
                CustomerId = customerId,
                MonthlyPrice = monthlyPrice,
                YearlyPrice = yearlyPrice
            };

            return quote;

        }
    }
}
