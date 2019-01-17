using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Solid.S.Models;

namespace Solid.S.A
{

    /// <remarks>
    /// Line:
    /// c676de6b-045a-b547-59d7-f98ab543dc87,1234,452.99
    /// </remarks>
    public class QuoteProcessor
    {
        private const int MonthsInYear = 12;

        public void ProcessQuotes(System.IO.Stream stream)
        {
            //read rows
            var lines = new List<string>();
            using (var reader = new System.IO.StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            var quotes = new List<Quote>();

            //parse
            var lineCount = 1;
            foreach (var line in lines)
            {
                var fields = line.Split(new char[] {','});

                if (fields.Length != 3)
                {
                    Console.WriteLine("WARN: Line {0} malformed. Only {1} field(s) found.", lineCount, fields.Length);
                    continue;
                }

                if (!Guid.TryParse(fields[0], out var quoteId))
                {
                    Console.WriteLine("WARN: Quote id on line {0} is not a valid Guid {1}", lineCount, fields[0]);
                    continue;
                }

                if (!int.TryParse(fields[1], out var customerId))
                {
                    Console.WriteLine("WARN: Customer id on line {0} is not a valid integer {1}", lineCount, fields[1]);
                    continue;
                }

                if (!decimal.TryParse(fields[2], out var monthlyPrice))
                {
                    Console.WriteLine("WARN: Monthly price on line {0} is not a valid decimal {1}", lineCount, fields[2]);
                    continue;
                }


                //cal values and create
                decimal yearlyPrice = monthlyPrice * MonthsInYear;

                var quote = new Quote
                {
                    QuoteId = quoteId,
                    CustomerId = customerId,
                    MonthlyPrice = monthlyPrice,
                    YearlyPrice = yearlyPrice
                };

                quotes.Add(quote);

                lineCount++;
            }

            // store
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

            Console.WriteLine("INFO: {0} quotes processed.", quotes.Count);
        }
    }
}
