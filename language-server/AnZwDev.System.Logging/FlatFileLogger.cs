using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AnZwDev.System.Logging
{
    public class FlatFileLogger : Logger
    {
        public string LogFilePath { get; }

        public string FieldSeparator { get; set; } = "\t";
        public string NewLine { get; set; } = "\n";
        public string FieldDelimiter { get; set; } = "\"";
        public string FieldDelimiterEscape { get; set; } = "\"\"";

        public FlatFileLogger(string? logFilePath)
        {
            this.LogFilePath = ValidateLogFilePath(logFilePath);
        }

        private string ValidateLogFilePath(string? logFilePath)
        {
            if (!String.IsNullOrWhiteSpace(logFilePath))
                return logFilePath;
            var logFileFolderPath = Path.GetDirectoryName(this.GetType().Assembly.Location) ?? Directory.GetCurrentDirectory();
            return Path.Combine(logFileFolderPath, "log.txt");
        }

        protected override void WriteLogMessage(LogLevel level, string message, string? stackTrace)
        {
            var line = NewLine + EncodeField(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            line = line + FieldSeparator + EncodeField(level.ToString());
            line = line + FieldSeparator + EncodeField(message);
            if (stackTrace != null)
                line = line + FieldSeparator + EncodeField(stackTrace);
            line = line + NewLine;

            try
            {
                File.AppendAllText(this.LogFilePath, line);
            }
            catch (Exception)
            {
            }
        }

        private string EncodeField(string? field)
        {
            if (field == null)
                field = String.Empty;
            if (field.Contains(this.FieldDelimiter))
                field = field.Replace(this.FieldDelimiter, this.FieldDelimiterEscape);
            return this.FieldDelimiter + field + this.FieldDelimiter;
        }

    }
}
