using Solid.S.Models;

namespace Solid.S.C.Interfaces
{
    public interface IQuoteMapper
    {
        Quote Map(string[] fields);
    }
}