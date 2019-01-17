using System.Collections.Generic;

namespace Solid.S.C.Interfaces
{
    public interface IQuoteDataProvider
    {
        IEnumerable<string> GetQuoteData();
    }
}
