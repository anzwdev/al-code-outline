using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System.Threading.Tasks;


namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetReportsListRequestHandler : RequestHandler
    {

        public GetReportsListRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getreportslist", UseSingleObjectParameterDeserialization = true)]
        public GetReportsListResponse GetReportsList(GetReportsListRequest parameters)
        {
            GetReportsListResponse response = new GetReportsListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                ReportInformationProvider provider = new ReportInformationProvider();
                response.symbols = provider.GetReports(project);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }

    }
}
