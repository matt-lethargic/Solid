using System.Collections.Generic;
using Solid.S.Models;

namespace Solid.S.C.Interfaces
{
    public interface IQuoteParser
    {
        IEnumerable<Quote> Parse(IEnumerable<string> lines);
    }
}