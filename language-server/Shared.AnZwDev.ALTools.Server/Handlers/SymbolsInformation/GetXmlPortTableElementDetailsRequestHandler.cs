using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{

    public class GetXmlPortTableElementDetailsRequestHandler : BaseALRequestHandler<GetXmlPortTableElementDetailsRequest, GetXmlPortTableElementDetailsResponse>
    {

        public GetXmlPortTableElementDetailsRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getxmlporttableelementdetails")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetXmlPortTableElementDetailsResponse> HandleMessage(GetXmlPortTableElementDetailsRequest parameters, RequestContext<GetXmlPortTableElementDetailsResponse> context)
        {
            GetXmlPortTableElementDetailsResponse response = new GetXmlPortTableElementDetailsResponse();

            try
            {

                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    XmlPortInformationProvider provider = new XmlPortInformationProvider();
                    response.symbol = provider.GetXmlPortTableElementDetails(project, parameters.objectName, parameters.name, parameters.getExistingFields, parameters.getAvailableFields);
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
#pragma warning restore 1998

    }

}
