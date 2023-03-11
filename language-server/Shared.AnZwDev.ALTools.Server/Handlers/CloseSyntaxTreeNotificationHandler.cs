using AnZwDev.ALTools;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class CloseSyntaxTreeNotificationHandler : BaseALNotificationHandler<CloseSyntaxTreeRequest>
    {

        public CloseSyntaxTreeNotificationHandler(ALDevToolsServer alDevToolsServer, LanguageServerHost languageServerHost) : base(alDevToolsServer, languageServerHost, "al/closesyntaxtree")
        {
        }

#pragma warning disable 1998
        public override async Task HandleNotification(CloseSyntaxTreeRequest parameters, NotificationContext context)
        {
            this.Server.SyntaxTrees.Close(parameters.path);
        }
#pragma warning restore 1998

    }
}
