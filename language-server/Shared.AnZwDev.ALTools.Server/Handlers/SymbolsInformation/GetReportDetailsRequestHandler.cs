using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetReportDetailsRequestHandler : RequestHandler
    {

        public GetReportDetailsRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getreportdetails", UseSingleObjectParameterDeserialization = true)]
        public GetReportDetailsResponse GetReportDetails(GetReportDetailsRequest parameters)
        {
            RebuildModifiedSymbols();

            GetReportDetailsResponse response = new GetReportDetailsResponse();

            try
            {

                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    ReportInformationProvider provider = new ReportInformationProvider();
                    response.symbol = provider.GetFullReportInformation(project, parameters.symbolReference.ToALObjectReference());
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

    }
}
