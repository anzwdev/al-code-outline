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
    public class GetPagesListRequestHandler : BaseALRequestHandler<GetPagesListRequest, GetPagesListResponse>
    {

        public GetPagesListRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getpageslist")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetPagesListResponse> HandleMessage(GetPagesListRequest parameters, RequestContext<GetPagesListResponse> context)
        {
            GetPagesListResponse response = new GetPagesListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                PageInformationProvider provider = new PageInformationProvider();
                response.symbols = provider.GetPages(project);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }
#pragma warning restore 1998
    }

}
