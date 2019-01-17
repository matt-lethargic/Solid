using System;
using Solid.S.C.Interfaces;

namespace Solid.S.C.Impls
{
    public class SimpleQuoteValidator : IQuoteValidator
    {
        private readonly ILogger _logger;

        public SimpleQuoteValidator(ILogger logger)
        {
            _logger = logger;
        }


        public bool Validate(string[] fields)
        {
            if (fields.Length != 3)
            {
                _logger.LogMessage("WARN: Line malformed. Only {1} field(s) found.", fields.Length);
                return false;
            }

            if (!Guid.TryParse(fields[0], out _))
            {
                _logger.LogMessage("WARN: Quote id is not a valid Guid {1}", fields[0]);
                return false;
            }
            
            if (!int.TryParse(fields[1], out _))
            {
                _logger.LogMessage("WARN: Customer id on is not a valid integer {1}", fields[1]);
                return false;
            }
            
            if (!decimal.TryParse(fields[2], out _))
            {
                _logger.LogMessage("WARN: Monthly price is not a valid decimal {1}", fields[2]);
                return false;
            }

            return true;
        }
    }
}