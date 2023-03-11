using AnZwDev.ALTools;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public abstract class BaseALNotificationHandler<T> : NotificationHandler<T>
    {

        public ALDevToolsServer Server { get; }

        public BaseALNotificationHandler(ALDevToolsServer alDevToolsServer, LanguageServerHost languageServerHost, string name) : base(languageServerHost, name)
        {
            this.Server = alDevToolsServer;
        }

    }
}
