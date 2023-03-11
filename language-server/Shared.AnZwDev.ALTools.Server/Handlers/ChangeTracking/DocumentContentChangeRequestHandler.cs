using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using AnZwDev.ALTools.Server.Contracts.ChangeTracking;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.ChangeTracking
{
    public class DocumentContentChangeRequestHandler : BaseALRequestHandler<DocumentContentChangeRequest, DocumentContentChangeResponse>
    {

        public DocumentContentChangeRequestHandler(ALDevToolsServer alDevToolsServer, LanguageServerHost languageServerHost) : base(alDevToolsServer, languageServerHost, "ws/documentContentChange")
        {
        }

#pragma warning disable 1998
        protected override async Task<DocumentContentChangeResponse> HandleMessage(DocumentContentChangeRequest parameters, RequestContext<DocumentContentChangeResponse> context)
        {
            DocumentContentChangeResponse response = new DocumentContentChangeResponse
            {
                root = this.Server.Workspace.OnDocumentChange(parameters.path, parameters.content, parameters.returnSymbols)
            };
            return response;
        }
#pragma warning restore 1998
    }
}
