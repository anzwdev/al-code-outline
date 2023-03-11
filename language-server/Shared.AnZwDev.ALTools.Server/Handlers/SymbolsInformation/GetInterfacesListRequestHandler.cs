using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetInterfacesListRequestHandler : BaseALRequestHandler<GetInterfacesListRequest, GetInterfacesListResponse>
    {

        public GetInterfacesListRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getinterfaceslist")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetInterfacesListResponse> HandleMessage(GetInterfacesListRequest parameters, RequestContext<GetInterfacesListResponse> context)
        {
            GetInterfacesListResponse response = new GetInterfacesListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                InterfaceInformationProvider provider = new InterfaceInformationProvider();
                response.symbols = provider.GetInterfaces(project);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }
#pragma warning restore 1998

    }
}
