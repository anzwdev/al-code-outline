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
    public class GetCodeunitsListRequestHandler : BaseALRequestHandler<GetCodeunitsListRequest, GetCodeunitsListResponse>
    {

        public GetCodeunitsListRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getcodeunitslist")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetCodeunitsListResponse> HandleMessage(GetCodeunitsListRequest parameters, RequestContext<GetCodeunitsListResponse> context)
        {
            GetCodeunitsListResponse response = new GetCodeunitsListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                CodeunitInformationProvider provider = new CodeunitInformationProvider();
                response.symbols = provider.GetCodeunits(project);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }
#pragma warning restore 1998
    }
}
