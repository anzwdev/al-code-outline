using AnZwDev.ALTools.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server
{
    public class MessageLogWriterImpl: IMessageLogWriter
    {
        public string LogFilePath { get; }

        public MessageLogWriterImpl(string logFilePath = null)
        {
            LogFilePath = ValidateLogFilePath(logFilePath);
        }

        private string ValidateLogFilePath(string logFilePath)
        {
            if (!String.IsNullOrWhiteSpace(logFilePath))
                return logFilePath;
            string logFileFolderPath = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
            return System.IO.Path.Combine(logFileFolderPath, "log.txt");
        }

        public void WriteError(Exception e)
        {
            this.WriteError(e, "Error: ");
        }

        public void WriteError(Exception e, string messageStartPart)
        {
            if (String.IsNullOrEmpty(messageStartPart))
                messageStartPart = "";

            string message = 
                "\n" +
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") +
                " [ERROR] " +
                messageStartPart + 
                e.Message + 
                "\n" + 
                e.StackTrace + 
                "\n";

            try
            {
                System.IO.File.AppendAllText(LogFilePath, message);
            }
            catch (Exception ex)
            {
            }
        }

    }
}
