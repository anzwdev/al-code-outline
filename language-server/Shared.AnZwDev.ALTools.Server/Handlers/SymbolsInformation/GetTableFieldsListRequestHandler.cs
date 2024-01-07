using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetTableFieldsListRequestHandler : RequestHandler
    {

        public GetTableFieldsListRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/gettablefieldslist", UseSingleObjectParameterDeserialization = true)]
        public GetTableFieldsListResponse GetTableFieldsList(GetTableFieldsListRequest parameters)
        {
            GetTableFieldsListResponse response = new GetTableFieldsListResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                TableInformationProvider provider = new TableInformationProvider();
                response.symbols = provider.GetTableFields(project, 
                    parameters.tableReference.ToALObjectReference(),                  
                    parameters.includeDisabled, parameters.includeObsolete, parameters.includeNormal, 
                    parameters.includeFlowFields, parameters.includeFlowFilters, parameters.includeToolTips, 
                    parameters.toolTipsSourceDependencies);
                response.symbols.Sort(new SymbolInformationComparer());
            }

            return response;
        }

    }
}
