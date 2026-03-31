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
    internal class OpenAppFileDocumentRequestHandler : RequestHandler
    {

        public OpenAppFileDocumentRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/symbolsviewer/openappfile", UseSingleObjectParameterDeserialization = true)]
        public OpenAppFileDocumentResponse OpenAppFileDocument(OpenAppFileDocumentRequest parameters)
        {
            var response = new OpenAppFileDocumentResponse()
            {
                Path = parameters.Path
            };

            try
            {
                if (!String.IsNullOrEmpty(parameters.Path))
                {

                    var workspace = this.Services.GetService<Workspace>();
                    var documentsStore = this.Services.GetService<SymbolsViewerDocumentStore>();
                    if ((workspace != null) && (documentsStore != null))
                    {
                        var document = documentsStore.Open(workspace, parameters.Path);
                        response.DocumentUid = document.Uid;
                        response.Root = document.ObjectHeadersViewRootNode;
                    }
                }
            }
            catch (Exception e)
            {
                Services.GetService<ILogger>()?.Log(e);
            }

            return response;
        }

    }
}
