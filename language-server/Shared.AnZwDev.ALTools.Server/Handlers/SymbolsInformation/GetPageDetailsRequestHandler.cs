using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetPageDetailsRequestHandler : RequestHandler
    {

        public GetPageDetailsRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getpagedetails", UseSingleObjectParameterDeserialization = true)]
        public GetPageDetailsResponse GetPageDetails(GetPageDetailsRequest parameters)
        {
            RebuildModifiedSymbols();

            GetPageDetailsResponse response = new GetPageDetailsResponse();

            try
            {

                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    PageInformationProvider provider = new PageInformationProvider();
                    response.symbol = provider.GetPageDetails(
                        project,
                        parameters.symbolReference.ToALObjectReference(),
                        parameters.getExistingFields, parameters.getAvailableFields, parameters.getToolTips, parameters.toolTipsSourceDependencies); ;
                    if (response.symbol != null)
                        response.symbol.Sort();
                }
            }
            catch (Exception e)
            {
                response.symbol = new PageInformation
                {
                    Name = e.Message
                };
                this.LogError(e);
            }

            return response;

        }

    }
}
