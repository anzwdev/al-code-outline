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
    public class CloseSymbolsLibraryNotificationHandler : BaseALNotificationHandler<CloseSymbolsLibraryRequest>
    {

        public CloseSymbolsLibraryNotificationHandler(ALDevToolsServer alDevToolsServer, LanguageServerHost languageServerHost) : base(alDevToolsServer, languageServerHost, "al/closesymbolslibrary")
        {
        }

#pragma warning disable 1998
        public override async Task HandleNotification(CloseSymbolsLibraryRequest parameters, NotificationContext context)
        {
            this.Server.SymbolsLibraries.RemoveLibrary(parameters.libraryId);
        }
#pragma warning restore 1998
    }
}
