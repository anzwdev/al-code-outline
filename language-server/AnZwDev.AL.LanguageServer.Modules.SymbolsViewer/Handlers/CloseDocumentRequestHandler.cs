using AnZwDev.AL.LanguageServer.Modules.SymbolsViewer.Contracts;
using AnZwDev.AL.Workspaces;
using AnZwDev.AL.Workspaces.Documents.SymbolsViewers;
using AnZwDev.LanguageServer;
using AnZwDev.System.Logging;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsViewer.Handlers
{
    internal class CloseDocumentRequestHandler : RequestHandler
    {

        public CloseDocumentRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/symbolsviewer/closedocument", UseSingleObjectParameterDeserialization = true)]
        public CloseDocumentResponse GetSyntaxTreeTreeView(CloseDocumentRequest parameters)
        {
            var response = new CloseDocumentResponse()
            {
                DocumentUid = parameters.DocumentUid,
            };

            try
            {
                this.Services.GetService<SymbolsViewerDocumentStore>()?.Remove(parameters.DocumentUid);
            }
            catch (Exception e)
            {
                Services.GetService<ILogger>()?.Log(e);
            }

            return response;
        }


    }
}
