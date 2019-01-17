using System;
using Solid.S.C.Interfaces;

namespace Solid.S.C.Impls
{
    public class ConsoleLogger : ILogger
    {
        public void LogMessage(string message, params object[] args)
        {
            Console.Write(message, args);
        }
    }
}
