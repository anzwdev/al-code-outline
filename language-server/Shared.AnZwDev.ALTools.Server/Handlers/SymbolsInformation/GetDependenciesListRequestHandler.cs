using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetDependenciesListRequestHandler : RequestHandler
    {
        public GetDependenciesListRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getdependencieslist", UseSingleObjectParameterDeserialization = true)]
        public GetDependenciesListResponse GetDependenciesList(GetDependenciesListRequest parameters)
        {
            GetDependenciesListResponse response = new GetDependenciesListResponse();

            try
            {
                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    response.dependencies = new List<string>();
                    for (int i = 0; i < project.Dependencies.Count; i++)
                    {
                        if (project.Dependencies[i].Symbols != null)
                            response.dependencies.Add(project.Dependencies[i].Symbols.GetNameWithPublisher());
                    }
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
