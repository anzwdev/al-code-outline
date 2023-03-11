using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class ExitNotificationHandler : NotificationHandler<object>
    {

        public ExitNotificationHandler(LanguageServerHost languageServerHost) : base(languageServerHost, "exit")
        {
        }

#pragma warning disable 1998
        public override async Task HandleNotification(object parameters, NotificationContext context)
        {
            this.LanguageServerHost.Stop();
        }
#pragma warning restore 1998

    }
}
