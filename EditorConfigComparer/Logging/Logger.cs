using System.Runtime.CompilerServices;

namespace EditorConfigComparer.Logging
{
    internal class Logger<T> : ILogger
    {
        private string _className;

        public Logger()
        {
            _className = typeof(T).Name;
        }

        public void LogInfo(string message, [CallerMemberName] string methodName = "")
        {
            LogFormattedMessage(message, methodName, Info);
        }

        public void LogError(string message, [CallerMemberName] string methodName = "")
        {
            LogFormattedMessage(message, methodName, Error);
        }

        public void LogWarning(string message, [CallerMemberName] string methodName = "")
        {
            LogFormattedMessage(message, methodName, Warning);
        }

        private const string Info    = "INFO   ";
        private const string Warning = "WARNING";
        private const string Error   = "ERROR  ";

        private void LogFormattedMessage(string message, string methodName, string messageType)
        {
            string formattedMessage =
                $"{DateTime.Now:HH:mm:ss.ffffff} {messageType} {_className} {methodName} {message}";
            CommonLogger.Write(formattedMessage);
        }
    }
}
