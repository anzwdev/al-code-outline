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
    public class GetInterfaceMethodsListRequestHandler : BaseALRequestHandler<GetInterfaceMethodsListRequest, GetInterfaceMethodsListResponse>
    {

        public GetInterfaceMethodsListRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getinterfacemethodslist")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetInterfaceMethodsListResponse> HandleMessage(GetInterfaceMethodsListRequest parameters, RequestContext<GetInterfaceMethodsListResponse> context)
        {
            GetInterfaceMethodsListResponse response = new GetInterfaceMethodsListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                InterfaceInformationProvider provider = new InterfaceInformationProvider();
                response.symbols = provider.GetMethodsInformation(project, parameters.name);
            }

            return response;
        }
#pragma warning restore 1998

    }
}
