using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetPagesListRequestHandler : RequestHandler
    {

        public GetPagesListRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getpageslist", UseSingleObjectParameterDeserialization = true)]
        public GetPagesListResponse GetPagesList(GetPagesListRequest parameters)
        {
            RebuildModifiedSymbols();

            GetPagesListResponse response = new GetPagesListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                PageInformationProvider provider = new PageInformationProvider();
                response.symbols = provider.GetPages(project);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }

    }

}
