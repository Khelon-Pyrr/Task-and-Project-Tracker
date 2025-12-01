using System;
using System.IO;

namespace Task___Project_Tracker.Services
{
    public class LoggerService
    {
        private readonly string _logFilePath;

        public LoggerService(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void Log(string message)
        {
            try
            {
                string logEntry = $"{DateTime.Now}: {message}{Environment.NewLine}";
                File.AppendAllText(_logFilePath, logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }
    }
}
