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
    public class GetCodeunitMethodsListRequestHandler : BaseALRequestHandler<GetCodeunitMethodsListRequest, GetCodeunitMethodsListResponse>
    {

        public GetCodeunitMethodsListRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getcodeunitmethodslist")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetCodeunitMethodsListResponse> HandleMessage(GetCodeunitMethodsListRequest parameters, RequestContext<GetCodeunitMethodsListResponse> context)
        {
            GetCodeunitMethodsListResponse response = new GetCodeunitMethodsListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                CodeunitInformationProvider provider = new CodeunitInformationProvider();
                response.symbols = provider.GetMethodsInformation(project, parameters.name);
            }

            return response;
        }
#pragma warning restore 1998

    }
}
