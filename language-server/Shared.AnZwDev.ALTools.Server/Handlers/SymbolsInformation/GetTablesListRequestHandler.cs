using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetTablesListRequestHandler : RequestHandler
    {

        public GetTablesListRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/gettableslist", UseSingleObjectParameterDeserialization = true)]
        public GetTablesListResponse GetTablesList(GetTablesListRequest parameters)
        {
            GetTablesListResponse response = new GetTablesListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                TableInformationProvider provider = new TableInformationProvider();
                response.symbols = provider.GetTables(project);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }

    }
}
