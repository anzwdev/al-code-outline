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
    public class GetReportDetailsRequestHandler : BaseALRequestHandler<GetReportDetailsRequest, GetReportDetailsResponse>
    {

        public GetReportDetailsRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getreportdetails")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetReportDetailsResponse> HandleMessage(GetReportDetailsRequest parameters, RequestContext<GetReportDetailsResponse> context)
        {
            GetReportDetailsResponse response = new GetReportDetailsResponse();

            try
            {

                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    ReportInformationProvider provider = new ReportInformationProvider();
                    response.symbol = provider.GetFullReportInformation(project, parameters.name);
                    if (response.symbol != null)
                        response.symbol.Sort();
                }
            }
            catch (Exception e)
            {
                response.symbol = new ReportInformation
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
