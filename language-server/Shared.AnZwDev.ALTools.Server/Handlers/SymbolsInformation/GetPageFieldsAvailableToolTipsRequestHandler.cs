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
    internal class GetPageFieldsAvailableToolTipsRequestHandler : BaseALRequestHandler<GetPageFieldAvailableToolTipsRequest, GetPageFieldAvailableToolTipsResponse>
    {
        public GetPageFieldsAvailableToolTipsRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getpagefieldtooltips")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetPageFieldAvailableToolTipsResponse> HandleMessage(GetPageFieldAvailableToolTipsRequest parameters, RequestContext<GetPageFieldAvailableToolTipsResponse> context)
        {
            GetPageFieldAvailableToolTipsResponse response = new GetPageFieldAvailableToolTipsResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                PageInformationProvider provider = new PageInformationProvider();
                response.toolTips = provider.GetPageFieldAvailableToolTips(project, parameters.objectType, parameters.objectName, parameters.sourceTable, parameters.fieldExpression);
            }

            return response;
        }
#pragma warning restore 1998
    }
}
