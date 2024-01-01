using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetCodeunitsListRequestHandler : RequestHandler
    {

        public GetCodeunitsListRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getcodeunitslist", UseSingleObjectParameterDeserialization = true)]
        public GetCodeunitsListResponse GetCodeunitsList(GetCodeunitsListRequest parameters)
        {
            GetCodeunitsListResponse response = new GetCodeunitsListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                CodeunitInformationProvider provider = new CodeunitInformationProvider();
                response.symbols = provider.GetCodeunits(project);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }

    }
}
