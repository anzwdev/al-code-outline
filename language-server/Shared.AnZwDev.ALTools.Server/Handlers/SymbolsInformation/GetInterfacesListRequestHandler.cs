using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetInterfacesListRequestHandler : RequestHandler
    {

        public GetInterfacesListRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getinterfaceslist", UseSingleObjectParameterDeserialization = true)]
        public GetInterfacesListResponse GetInterfacesList(GetInterfacesListRequest parameters)
        {
            RebuildModifiedSymbols();

            GetInterfacesListResponse response = new GetInterfacesListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                InterfaceInformationProvider provider = new InterfaceInformationProvider();
                response.symbols = provider.GetInterfaces(project);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }

    }
}
