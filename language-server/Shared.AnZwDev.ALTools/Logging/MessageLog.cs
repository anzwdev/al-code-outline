using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Logging
{
    public static class MessageLog
    {

        public static IMessageLogWriter Writer { get; set; } = null;

        public static void LogError(Exception e)
        {
            try
            {
                if (MessageLog.Writer != null)
                    MessageLog.Writer.WriteError(e);
            }
            catch (Exception)
            {
            }
        }

        public static void LogError(Exception e, string messageStartPart)
        {
            try
            {
                if (MessageLog.Writer != null)
                    MessageLog.Writer.WriteError(e, messageStartPart);
            }
            catch (Exception)
            {
            }
        }

    }
}
