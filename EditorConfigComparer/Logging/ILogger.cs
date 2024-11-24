using System.Runtime.CompilerServices;

namespace EditorConfigComparer.Logging
{
    internal interface ILogger
    {
        void LogInfo(string message, [CallerMemberName] string methodName = "");
        void LogWarning(string message, [CallerMemberName] string methodName = "");
        void LogError(string message, [CallerMemberName] string methodName = "");
    }
}