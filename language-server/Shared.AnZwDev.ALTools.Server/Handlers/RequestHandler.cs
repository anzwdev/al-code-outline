using AnZwDev.ALTools.Logging;
using System;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class RequestHandler
    {

        public LanguageServerHost LanguageServerHost { get; }
        public ALDevToolsServer Server { get => LanguageServerHost.ALDevToolsServer; }

        public RequestHandler(LanguageServerHost languageServerHost)
        {
            LanguageServerHost = languageServerHost;
        }

        protected void LogError(Exception exception)
        {
            MessageLog.LogError(exception);   
        }

    }
}
