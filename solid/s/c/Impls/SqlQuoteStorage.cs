using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Solid.S.C.Interfaces;
using Solid.S.Models;

namespace Solid.S.C.Impls
{
    public class SqlQuoteStorage : IQuoteStorage
    {
        private readonly ILogger _logger;

        public SqlQuoteStorage(ILogger logger)
        {
            _logger = logger;
        }

        public void Save(IEnumerable<Quote> quotes)
        {
            using (var connection = new SqlConnection("Data Source=localhost;Initial Catalog=Quotes;"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var quote in quotes)
                    {
                        var command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "usp_SomeRandomProc";
                        command.Parameters.AddWithValue("@quoteId", quote.QuoteId);
                        command.Parameters.AddWithValue("@customerId", quote.CustomerId);
                        command.Parameters.AddWithValue("@monthlyPrice", quote.MonthlyPrice);
                        command.Parameters.AddWithValue("@yearlyPrice", quote.YearlyPrice);
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                connection.Close();
            }

            _logger.LogMessage("INFO: {0} quotes processed.", quotes.Count());
        }
    }
}
