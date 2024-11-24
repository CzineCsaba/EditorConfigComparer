using System.Diagnostics;
using System.IO;

namespace EditorConfigComparer.Logging
{
    internal static class CommonLogger
    {
        private static readonly int proecessId = Process.GetCurrentProcess().Id;
        private static readonly string _logDirectoryPath = @"C:\Temp\_EditorConfigComparer";
        private static readonly string _logFilePath = $"{_logDirectoryPath}\\EditorConfigComparer_{DateTime.Now:yyyyMMdd_HHmmss}_{proecessId}.txt";
        private static readonly object _fileLock = new object();

        public static void Write(string text)
        {
            lock (_fileLock)
            {
                if (!Directory.Exists(_logDirectoryPath))
                {
                    Directory.CreateDirectory(_logDirectoryPath);
                }

                if (!File.Exists(_logFilePath))
                {
                    File.Create(_logFilePath);
                }

                File.AppendAllLines(_logFilePath, [text]);
            }
        }
    }
}
