using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetXmlPortsListRequestHandler : RequestHandler
    {

        public GetXmlPortsListRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getxmlportslist", UseSingleObjectParameterDeserialization = true)]
        public GetXmlPortsListResponse GetXmlPortsList(GetXmlPortsListRequest parameters)
        {
            RebuildModifiedSymbols();

            GetXmlPortsListResponse response = new GetXmlPortsListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                XmlPortInformationProvider provider = new XmlPortInformationProvider();
                response.symbols = provider.GetXmlPorts(project);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }

    }

}