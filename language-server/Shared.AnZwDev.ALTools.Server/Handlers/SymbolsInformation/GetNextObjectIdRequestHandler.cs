using AnZwDev.ALTools.Server.Contracts.SymbolsInformation;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers.SymbolsInformation
{
    public class GetNextObjectIdRequestHandler : BaseALRequestHandler<GetNextObjectIdRequest, GetNextObjectIdResponse>
    {

        public GetNextObjectIdRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getnextobjectid")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetNextObjectIdResponse> HandleMessage(GetNextObjectIdRequest parameters, RequestContext<GetNextObjectIdResponse> context)
        {
            GetNextObjectIdResponse response = new GetNextObjectIdResponse
            {
                id = 0
            };

            try
            {

                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    ObjectIdInformationProvider objectIdInformationProvider = new ObjectIdInformationProvider();
                    response.id = objectIdInformationProvider.GetNextObjectId(project, parameters.objectType);
                }
            }
            catch (Exception e)
            {
                response.id = 0;
                this.LogError(e);
            }

            return response;

        }
#pragma warning restore 1998


    }
}
