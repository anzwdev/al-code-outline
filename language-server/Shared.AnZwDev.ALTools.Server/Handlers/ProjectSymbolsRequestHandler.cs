using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Server.Contracts;
using System;
using System.Threading.Tasks;
using AnZwDev.ALTools.Workspace;
using StreamJsonRpc;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class ProjectSymbolsRequestHandler : RequestHandler
    {

        public ProjectSymbolsRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/projectsymbols", UseSingleObjectParameterDeserialization = true)]
        public ProjectSymbolsResponse GetProjectSymbols(ProjectSymbolsRequest parameters)
        {
            ProjectSymbolsResponse response = new ProjectSymbolsResponse();
            try
            {
                //find project
                ALProject project = this.Server.Workspace.FindProject(parameters.projectPath);
                if (project != null)
                {
                    ALSymbolsLibrary library = project.CreateSymbolsLibrary(parameters.includeDependencies);
                    response.libraryId = this.Server.SymbolsLibraries.AddLibrary(library);
                    response.root = library.GetObjectsTree();
                }
            }
            catch (Exception e)
            {
                response.root = new ALSymbol
                {
                    fullName = e.Message                   
                };
                response.SetError(e.Message);
                this.LogError(e);
            }

            return response;
        }

    }
}
