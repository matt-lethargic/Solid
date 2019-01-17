using System;

namespace Solid.S.Models
{
    public class Quote
    {
        public Guid QuoteId { get; set; }
        public int CustomerId { get; set; }
        public decimal MonthlyPrice { get; set; }
        public decimal YearlyPrice { get; set; }
    }
}
