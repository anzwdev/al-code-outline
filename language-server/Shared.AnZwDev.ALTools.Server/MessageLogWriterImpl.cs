using AnZwDev.ALTools.Logging;
using AnZwDev.VSCodeLangServer.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.Server
{
    public class MessageLogWriterImpl: IMessageLogWriter
    {

        private ALDevToolsServerHost _aLDevToolsServerHost;

        public MessageLogWriterImpl(ALDevToolsServerHost serverHost)
        {
            this._aLDevToolsServerHost = serverHost;
        }

        public void WriteError(Exception e)
        {
            this.WriteError(e, "Error: ");
        }

        public void WriteError(Exception e, string messageStartPart)
        {
            if (String.IsNullOrEmpty(messageStartPart))
                messageStartPart = "Error: ";

            if ((this._aLDevToolsServerHost != null) && (this._aLDevToolsServerHost.Logger != null))
                this._aLDevToolsServerHost.Logger.Write(LogLevel.Error, messageStartPart + e.Message + "\n" + e.StackTrace);
        }


    }
}
