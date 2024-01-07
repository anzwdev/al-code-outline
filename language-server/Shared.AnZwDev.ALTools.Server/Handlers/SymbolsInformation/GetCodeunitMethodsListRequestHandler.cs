using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetCodeunitMethodsListRequestHandler : RequestHandler
    {

        public GetCodeunitMethodsListRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getcodeunitmethodslist", UseSingleObjectParameterDeserialization = true)]
        public GetCodeunitMethodsListResponse GetCodeunitMethodsList(GetCodeunitMethodsListRequest parameters)
        {
            GetCodeunitMethodsListResponse response = new GetCodeunitMethodsListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                CodeunitInformationProvider provider = new CodeunitInformationProvider();
                response.symbols = provider.GetMethodsInformation(project, parameters.symbolReference.ToALObjectReference());
            }

            return response;
        }

    }
}
