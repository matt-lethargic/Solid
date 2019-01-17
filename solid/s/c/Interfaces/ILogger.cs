namespace Solid.S.C.Interfaces
{
    public interface ILogger
    {
        void LogMessage(string message, params object[] args);
    }
}
