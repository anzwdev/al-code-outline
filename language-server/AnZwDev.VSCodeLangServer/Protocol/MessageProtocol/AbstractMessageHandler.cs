using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.VSCodeLangServer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.VSCodeLangServer.Protocol.MessageProtocol
{
    public abstract class AbstractMessageHandler
    {

        public string Name { get; }
        public LanguageServerHost LanguageServerHost { get; }
        public ILogger Logger 
        { 
            get { return this.LanguageServerHost.Logger; }
        }

        public AbstractMessageHandler(LanguageServerHost languageServerHost, string name)
        {
            this.Name = name;
            this.LanguageServerHost = languageServerHost;
        }

        protected void LogError(Exception e)
        {
            if (this.Logger != null)
                this.Logger.Write(LogLevel.Error, "Error: " + e.Message + "\n" + e.StackTrace);
        }

        public abstract Task HandleRawMessage(Message message, MessageWriter messageWriter);

    }
}
