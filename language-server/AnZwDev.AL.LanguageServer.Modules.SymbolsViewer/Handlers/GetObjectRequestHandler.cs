using AnZwDev.AL.LanguageServer.Modules.SymbolsViewer.Contracts;
using AnZwDev.AL.Workspaces.Documents.SymbolsViewers;
using AnZwDev.LanguageServer;
using AnZwDev.System.ServiceModel;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.LanguageServer.Modules.SymbolsViewer.Handlers
{
    internal class GetObjectRequestHandler : RequestHandler
    {
        public GetObjectRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/symbolsviewer/getobject", UseSingleObjectParameterDeserialization = true)]
        public GetObjectResponse GetObjectHeadersTree(GetObjectRequest parameters)
        {
            var response = new GetObjectResponse()
            {
                DocumentUid = parameters.DocumentUid,
                ObjectUid = parameters.ObjectUid,
                Root = this.Services
                    .GetService<SymbolsViewerDocumentStore>()?
                    .Get(parameters.DocumentUid)?
                    .GetNode(parameters.ObjectUid)
            };

            return response;
        }
    }
}
