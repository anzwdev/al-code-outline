using AnZwDev.ALTools.ALSymbolReferences;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetDependenciesListRequestHandler : BaseALRequestHandler<GetDependenciesListRequest, GetDependenciesListResponse>
    {
        public GetDependenciesListRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getdependencieslist")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetDependenciesListResponse> HandleMessage(GetDependenciesListRequest parameters, RequestContext<GetDependenciesListResponse> context)
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
#pragma warning restore 1998

    }
}
