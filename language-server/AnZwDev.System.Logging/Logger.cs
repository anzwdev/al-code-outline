using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.System.Logging
{
    public abstract class Logger : ILogger
    {

        public LogLevel LogLevel { get; set; } = LogLevel.Information;

        public void Log(string message, LogLevel level)
        {
            if (level < LogLevel)
                return;

            WriteLogMessage(level, message, null);
        }
              
        public void Log(Exception e, string? messagePrefix = null, LogLevel level = LogLevel.Error)
        {
            if (level < LogLevel)
                return;

            var message = e.Message;
            if (messagePrefix != null)
                message = messagePrefix + message;

            WriteLogMessage(level, message, e.StackTrace);
        }

        protected abstract void WriteLogMessage(LogLevel level, string message, string? stackTrace);

    }
}
