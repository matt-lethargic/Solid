using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Solid.S.Models;

namespace Solid.S.B
{
    /// <summary>
    /// 
    /// Refactored for clarity
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// Line:
    /// c676de6b-045a-b547-59d7-f98ab543dc87,1234,452.99
    /// 
    /// </remarks>
    public class QuoteProcessor
    {
        private const int MonthsInYear = 12;

        public void ProcessQuotes(Stream stream)
        {
            var lines = ReadQuoteLines(stream);
            var quotes = ParseQuotes(lines);

            StoreQuotes(quotes);
        }


        private List<string> ReadQuoteLines(Stream stream)
        {
            //read rows
            var lines = new List<string>();
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            return lines;
        }
        
        private IEnumerable<Quote> ParseQuotes(List<string> lines)
        {
            var quotes = new List<Quote>();

            //parse
            var lineCount = 1;
            foreach (var line in lines)
            {
                var fields = line.Split(new char[] { ',' });

                if (!ValidateQuoteData(fields, lineCount))
                {
                    continue;
                }

                var quote = MapDataToQuote(fields);

                quotes.Add(quote);

                lineCount++;
            }
            return quotes;
        }

        private bool ValidateQuoteData(string[] fields, int currentline)
        {
            if (fields.Length != 3)
            {
                LogMessage("WARN: Line {0} malformed. Only {1} field(s) found.", currentline, fields.Length);
                return false;
            }

            if (!Guid.TryParse(fields[0], out _))
            {
                LogMessage("WARN: Quote id on line {0} is not a valid Guid {1}", currentline, fields[0]);
                return false;
            }

            if (!int.TryParse(fields[1], out _))
            {
                LogMessage("WARN: Customer id on line {0} is not a valid integer {1}", currentline, fields[1]);
                return false;
            }

            if (!decimal.TryParse(fields[2], out _))
            {
                LogMessage("WARN: Monthly price on line {0} is not a valid decimal {1}", currentline, fields[2]);
                return false;
            }

            return true;
        }

        private Quote MapDataToQuote(string[] fields)
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

        private void StoreQuotes(IEnumerable<Quote> quotes)
        {
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

            LogMessage("INFO: {0} quotes processed.", quotes.Count());
        }

        private void LogMessage(string messsage, params object[] args)
        {
            Console.WriteLine(messsage, args);
        }
    }
}
