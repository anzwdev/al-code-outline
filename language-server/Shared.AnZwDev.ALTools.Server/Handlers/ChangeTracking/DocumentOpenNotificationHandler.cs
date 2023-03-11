﻿using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class DocumentOpenNotificationHandler : BaseALNotificationHandler<DocumentChangeNotificationRequest>
    {

        public DocumentOpenNotificationHandler(ALDevToolsServer alDevToolsServer, LanguageServerHost languageServerHost) : base(alDevToolsServer, languageServerHost, "ws/documentOpen")
        {
        }

#pragma warning disable 1998
        public override async Task HandleNotification(DocumentChangeNotificationRequest parameters, NotificationContext context)
        {
            this.Server.Workspace.OnDocumentOpen(parameters.path);
        }
#pragma warning restore 1998

    }
}
