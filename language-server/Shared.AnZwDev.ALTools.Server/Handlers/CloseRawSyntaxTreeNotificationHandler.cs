using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class CloseRawSyntaxTreeNotificationHandler : BaseALNotificationHandler<CloseSyntaxTreeRequest>
    {

        public CloseRawSyntaxTreeNotificationHandler(ALDevToolsServer alDevToolsServer, LanguageServerHost languageServerHost) : base(alDevToolsServer, languageServerHost, "al/closerawsyntaxtree")
        {
        }

#pragma warning disable 1998
        public override async Task HandleNotification(CloseSyntaxTreeRequest parameters, NotificationContext context)
        {
            this.Server.RawSyntaxTrees.Close(parameters.path);
        }
#pragma warning restore 1998

    }
}
