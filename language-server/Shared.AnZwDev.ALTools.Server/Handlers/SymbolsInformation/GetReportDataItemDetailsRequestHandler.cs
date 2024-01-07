using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetReportDataItemDetailsRequestHandler : RequestHandler
    {

        public GetReportDataItemDetailsRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getreportdataitemdetails", UseSingleObjectParameterDeserialization = true)]
        public GetReportDataItemDetailsResponse GetReportDataItemDetails(GetReportDataItemDetailsRequest parameters)
        {
            GetReportDataItemDetailsResponse response = new GetReportDataItemDetailsResponse();

            try
            {

                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    ReportInformationProvider provider = new ReportInformationProvider();
                    response.symbol = provider.GetReportDataItemInformationDetails(project, 
                        parameters.symbolReference.ToALObjectReference(),
                        parameters.childSymbolName,
                        parameters.getExistingFields, parameters.getAvailableFields);
                    if (response.symbol != null)
                        response.symbol.Sort();
                }
            }
            catch (Exception e)
            {
                response.symbol = new ReportDataItemInformation
                {
                    Name = e.Message
                };
                this.LogError(e);
            }

            return response;
        }

    }
}
