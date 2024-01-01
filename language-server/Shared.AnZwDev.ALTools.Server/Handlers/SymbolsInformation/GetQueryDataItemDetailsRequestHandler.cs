using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetQueryDataItemDetailsRequestHandler : RequestHandler
    {

        public GetQueryDataItemDetailsRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getquerydataitemdetails", UseSingleObjectParameterDeserialization = true)]
        public GetQueryDataItemDetailsResponse GetQueryDataItemDetails(GetQueryDataItemDetailsRequest parameters)
        {
            GetQueryDataItemDetailsResponse response = new GetQueryDataItemDetailsResponse();

            try
            {
                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    QueryInformationProvider provider = new QueryInformationProvider();
                    response.symbol = provider.GetQueryDataItemInformationDetails(project, 
                        parameters.symbolReference.ToALObjectReference(),
                        parameters.childSymbolName, 
                        parameters.getExistingFields, parameters.getAvailableFields);

                    if (response.symbol != null)
                        response.symbol.Sort();
                }
            }
            catch (Exception e)
            {
                response.symbol = new QueryDataItemInformation
                {
                    Name = e.Message
                };
                this.LogError(e);
            }

            return response;
        }

    }
}
