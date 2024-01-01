using System;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.ALTools.Workspace;
using StreamJsonRpc;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class DocumentSymbolsRequestHandler : RequestHandler
    {

        public DocumentSymbolsRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/documentsymbols", UseSingleObjectParameterDeserialization = true)]
        public DocumentSymbolsResponse GetDocumentSymbols(DocumentSymbolsRequest parameters)
        {
            DocumentSymbolsResponse response = new DocumentSymbolsResponse();
            try
            {
                ALProject project = Server.Workspace.FindProject(parameters.path, parameters.projectPath, true);

                ALSymbolInfoSyntaxTreeReader symbolTreeBuilder = new ALSymbolInfoSyntaxTreeReader(
                    parameters.includeProperties);
                if (String.IsNullOrWhiteSpace(parameters.source))
                    response.root = symbolTreeBuilder.ProcessSourceFile(parameters.path, project);
                else
                {
                    response.root = symbolTreeBuilder.ProcessSourceCodeAndUpdateActiveDocument(
                        parameters.path, parameters.source, Server.Workspace, project, parameters.isActiveDocument);
                }
            }
            catch (Exception e)
            {
                response.root = new ALSymbol
                {
                    fullName = e.Message
                };
                this.LogError(e);
            }

            return response;
        }


    }
}
