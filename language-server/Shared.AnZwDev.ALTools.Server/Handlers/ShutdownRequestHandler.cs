using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{

    public class ShutdownRequestHandler : RequestHandler<object, object>
    {

        public ShutdownRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost, "shutdown")
        {
        }

#pragma warning disable 1998
        protected override async Task<object> HandleMessage(object parameters, RequestContext<object> context)
        {
            return new object();
        }
#pragma warning restore 1998
    }

}
