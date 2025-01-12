using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.ALTools.Workspace;
using StreamJsonRpc;
using System;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class GetProjectSymbolLocationRequestHandler : RequestHandler
    {

        public GetProjectSymbolLocationRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/projectsymbollocation", UseSingleObjectParameterDeserialization = true)]
        public GetProjectSymbolLocationResponse GetProjectSymbolLocation(GetProjectSymbolLocationRequest parameters)
        {
            RebuildModifiedSymbols();

            GetProjectSymbolLocationResponse response = new GetProjectSymbolLocationResponse();
            try
            {
                ALSymbol symbol = new ALSymbol(parameters.kind, parameters.name);
                ALProject project = this.Server.Workspace.FindProject(parameters.projectPath);
                if (project != null)
                {
                    ALProjectSymbolsLibrarySource symbolsSource = new ALProjectSymbolsLibrarySource(project);
                    response.location = symbolsSource.GetSymbolSourceProjectLocation(symbol);
                }

                for (int i = 0; (String.IsNullOrWhiteSpace(response.location?.sourcePath)) && (i < this.Server.Workspace.Projects.Count); i++)
                {
                    project = this.Server.Workspace.Projects[i];
                    ALProjectSymbolsLibrarySource symbolsSource = new ALProjectSymbolsLibrarySource(project);
                    response.location = symbolsSource.GetSymbolSourceProjectLocation(symbol);
                }
            }
            catch (Exception e)
            {
                this.LogError(e);
            }
            return response;
        }

    }
}
