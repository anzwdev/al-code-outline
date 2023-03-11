using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetPermissionSetsRequestHandler : BaseALRequestHandler<GetPermissionSetsRequest, GetPermissionSetsResponse>
    {

        public GetPermissionSetsRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getpermissionsetslist")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetPermissionSetsResponse> HandleMessage(GetPermissionSetsRequest parameters, RequestContext<GetPermissionSetsResponse> context)
        {
            GetPermissionSetsResponse response = new GetPermissionSetsResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                PermissionSetInformationProvider provider = new PermissionSetInformationProvider();
                response.symbols = provider.GetPermissionSets(project, parameters.includeNonAccessible);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }
#pragma warning restore 1998
    }
}
