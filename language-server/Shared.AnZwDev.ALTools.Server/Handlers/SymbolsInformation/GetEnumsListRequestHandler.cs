using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetEnumsListRequestHandler : RequestHandler
    {

        public GetEnumsListRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getenumslist", UseSingleObjectParameterDeserialization = true)]
        public GetEnumsListResponse GetEnumsList(GetEnumsListRequest parameters)
        {
            GetEnumsListResponse response = new GetEnumsListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                EnumInformationProvider provider = new EnumInformationProvider();
                response.symbols = provider.GetEnums(project);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }

    }
}
