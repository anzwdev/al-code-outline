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
    internal class OpenProjectDocumentRequestHandler : RequestHandler
    {
        public OpenProjectDocumentRequestHandler(IServiceProvider services) : base(services)
        {
        }

        [JsonRpcMethod("al/symbolsviewer/openproject", UseSingleObjectParameterDeserialization = true)]
        public OpenProjectDocumentResponse OpenProjectSymbolsDocument(OpenProjectDocumentRequest parameters)
        {
            var response = new OpenProjectDocumentResponse()
            {
                Path = parameters.Path,
                IncludeDependencies = parameters.IncludeDependencies
            };

            try
            {
                if (!String.IsNullOrEmpty(parameters.Path))
                {

                    var workspace = this.Services.GetService<Workspace>();
                    var documentsStore = this.Services.GetService<SymbolsViewerDocumentStore>();
                    var project = workspace?.Projects.FindByPath(parameters.Path);

                    if ((workspace != null) && (documentsStore != null) && (project != null))
                    {
                        var document = documentsStore.Open(project, parameters.IncludeDependencies);
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
