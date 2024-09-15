using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System.Threading.Tasks;


namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetQueriesListRequestHandler : RequestHandler
    {

        public GetQueriesListRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getquerieslist", UseSingleObjectParameterDeserialization = true)]
        public GetQueriesListResponse GetQueriesList(GetQueriesListRequest parameters)
        {
            RebuildModifiedSymbols();

            GetQueriesListResponse response = new GetQueriesListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                QueryInformationProvider provider = new QueryInformationProvider();
                response.symbols = provider.GetQueries(project);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }

    }
}
