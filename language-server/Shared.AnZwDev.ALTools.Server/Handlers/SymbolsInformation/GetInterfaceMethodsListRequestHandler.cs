using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetInterfaceMethodsListRequestHandler : RequestHandler
    {

        public GetInterfaceMethodsListRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getinterfacemethodslist", UseSingleObjectParameterDeserialization = true)]
        public GetInterfaceMethodsListResponse GetInterfaceMethodsList(GetInterfaceMethodsListRequest parameters)
        {
            RebuildModifiedSymbols();

            GetInterfaceMethodsListResponse response = new GetInterfaceMethodsListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                InterfaceInformationProvider provider = new InterfaceInformationProvider();
                response.symbols = provider.GetMethodsInformation(project, parameters.symbolReference.ToALObjectReference());
            }

            return response;
        }

    }
}
