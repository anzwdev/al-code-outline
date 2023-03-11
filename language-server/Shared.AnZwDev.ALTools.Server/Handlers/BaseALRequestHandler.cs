using AnZwDev.ALTools;
using AnZwDev.VSCodeLangServer.Utility;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.VSCodeLangServer.Protocol.Server;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class BaseALRequestHandler<TParameters, TResult> : RequestHandler<TParameters, TResult>
    {

        public ALDevToolsServer Server { get; }

        public BaseALRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost, string name) : base(languageServerHost, name)
        {
            this.Server = server;
        }

    }
}
