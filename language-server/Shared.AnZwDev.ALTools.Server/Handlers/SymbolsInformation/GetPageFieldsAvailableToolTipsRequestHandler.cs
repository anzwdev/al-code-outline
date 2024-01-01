using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    internal class GetPageFieldsAvailableToolTipsRequestHandler : RequestHandler
    {
        public GetPageFieldsAvailableToolTipsRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getpagefieldtooltips", UseSingleObjectParameterDeserialization = true)]
        public GetPageFieldAvailableToolTipsResponse GetPageFieldAvailableToolTips(GetPageFieldAvailableToolTipsRequest parameters)
        {
            GetPageFieldAvailableToolTipsResponse response = new GetPageFieldAvailableToolTipsResponse();

            ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
            if (project != null)
            {
                PageInformationProvider provider = new PageInformationProvider();
                response.toolTips = provider.GetPageFieldAvailableToolTips(project, 
                    parameters.objectType,
                    parameters.objectReference.ToALObjectIdentifier(),
                    parameters.sourceTableReference.ToALObjectReference(), 
                    parameters.fieldExpression);
            }

            return response;
        }

    }
}
