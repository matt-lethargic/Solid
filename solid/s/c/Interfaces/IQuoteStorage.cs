using System.Collections.Generic;
using Solid.S.Models;

namespace Solid.S.C.Interfaces
{
    public interface IQuoteStorage
    {
        void Save(IEnumerable<Quote> quotes);
    }
}