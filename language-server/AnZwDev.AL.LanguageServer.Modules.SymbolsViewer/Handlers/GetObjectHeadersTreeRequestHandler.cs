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
    internal class GetObjectHeadersTreeRequestHandler : RequestHandler
    {
        public GetObjectHeadersTreeRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/symbolsviewer/getobjectheaderstree", UseSingleObjectParameterDeserialization = true)]
        public GetObjectHeadersTreeResponse GetObjectHeadersTree(GetObjectHeadersTreeRequest parameters)
        {
            var response = new GetObjectHeadersTreeResponse()
            {
                DocumentUid = parameters.DocumentUid,
                Root = this.Services
                    .GetService<SymbolsViewerDocumentStore>()?
                    .Get(parameters.DocumentUid)?
                    .ObjectHeadersViewRootNode
            };

            return response;
        }

    }
}
