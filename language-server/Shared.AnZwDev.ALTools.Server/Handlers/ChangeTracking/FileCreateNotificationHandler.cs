using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class FileCreateNotificationHandler : BaseALNotificationHandler<FilesNotificationRequest>
    {

        public FileCreateNotificationHandler(ALDevToolsServer alDevToolsServer, LanguageServerHost languageServerHost) : base(alDevToolsServer, languageServerHost, "ws/fileCreate")
        {
        }

#pragma warning disable 1998
        public override async Task HandleNotification(FilesNotificationRequest parameters, NotificationContext context)
        {
            this.Server.Workspace.OnFilesCreate(parameters.files);

        }
#pragma warning restore 1998

    }
}