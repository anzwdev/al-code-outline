using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using StreamJsonRpc;
using System;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{

    public class GetXmlPortTableElementDetailsRequestHandler : RequestHandler
    {

        public GetXmlPortTableElementDetailsRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/getxmlporttableelementdetails", UseSingleObjectParameterDeserialization = true)]
        public GetXmlPortTableElementDetailsResponse GetXmlPortTableElementDetails(GetXmlPortTableElementDetailsRequest parameters)
        {
            GetXmlPortTableElementDetailsResponse response = new GetXmlPortTableElementDetailsResponse();

            try
            {

                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    XmlPortInformationProvider provider = new XmlPortInformationProvider();
                    response.symbol = provider.GetXmlPortTableElementDetails(project, 
                        parameters.symbolReference.ToALObjectReference(),
                        parameters.childSymbolName,
                        parameters.getExistingFields, parameters.getAvailableFields);
                    if (response.symbol != null)
                        response.symbol.Sort();
                }
            }
            catch (Exception e)
            {
                response.symbol = new XmlPortTableElementInformation
                {
                    Name = e.Message
                };
                this.LogError(e);
            }

            return response;

        }

    }

}
