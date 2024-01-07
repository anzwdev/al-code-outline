using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetPermissionSetsRequestHandler : RequestHandler
    {

        public GetPermissionSetsRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getpermissionsetslist", UseSingleObjectParameterDeserialization = true)]
        public GetPermissionSetsResponse GetPermissionSets(GetPermissionSetsRequest parameters)
        {
            GetPermissionSetsResponse response = new GetPermissionSetsResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                PermissionSetInformationProvider provider = new PermissionSetInformationProvider();
                response.symbols = provider.GetPermissionSets(project, parameters.includeNonAccessible);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }

    }
}
