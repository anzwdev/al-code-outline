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
    public class GetWarningDirectivesRequestHandler : BaseALRequestHandler<GetWarningDirectivesRequest, GetWarningDirectivesResponse>
    {

        public GetWarningDirectivesRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getwarningdirectives")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetWarningDirectivesResponse> HandleMessage(GetWarningDirectivesRequest parameters, RequestContext<GetWarningDirectivesResponse> context)
        {
            GetWarningDirectivesResponse response = new GetWarningDirectivesResponse();

            WarningDirectivesInformationProvider provider = new WarningDirectivesInformationProvider(this.Server);
            response.directives = provider.GetWarningDirectives(this.Server.Workspace);

            return response;
        }
#pragma warning restore 1998

    }
}
