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
    public class GetPageDetailsRequestHandler : BaseALRequestHandler<GetPageDetailsRequest, GetPageDetailsResponse>
    {

        public GetPageDetailsRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getpagedetails")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetPageDetailsResponse> HandleMessage(GetPageDetailsRequest parameters, RequestContext<GetPageDetailsResponse> context)
        {
                GetPageDetailsResponse response = new GetPageDetailsResponse();

            try
            {

                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    PageInformationProvider provider = new PageInformationProvider();
                    response.symbol = provider.GetPageDetails(project, parameters.name, parameters.getExistingFields, parameters.getAvailableFields, parameters.getToolTips, parameters.toolTipsSourceDependencies);
                    if (response.symbol != null)
                        response.symbol.Sort();
                }
            }
            catch (Exception e)
            {
                response.symbol = new PageInformation
                {
                    Name = e.Message
                };
                this.LogError(e);
            }

            return response;

        }
#pragma warning restore 1998

    }
}
