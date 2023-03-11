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
    public class GetEnumsListRequestHandler : BaseALRequestHandler<GetEnumsListRequest, GetEnumsListResponse>
    {

        public GetEnumsListRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getenumslist")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetEnumsListResponse> HandleMessage(GetEnumsListRequest parameters, RequestContext<GetEnumsListResponse> context)
        {
            GetEnumsListResponse response = new GetEnumsListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                EnumInformationProvider provider = new EnumInformationProvider();
                response.symbols = provider.GetEnums(project);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }
#pragma warning restore 1998
    }
}
