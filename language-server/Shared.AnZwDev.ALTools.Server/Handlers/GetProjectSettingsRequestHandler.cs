using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.ALTools;
using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.ALSymbols.SymbolReaders;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.ALTools.Workspace;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class GetProjectSettingsRequestHandler : BaseALRequestHandler<GetProjectSettingsRequest, GetProjectSettingsResponse>
    {

        public GetProjectSettingsRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/getprojectsettings")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetProjectSettingsResponse> HandleMessage(GetProjectSettingsRequest parameters, RequestContext<GetProjectSettingsResponse> context)
        {
            GetProjectSettingsResponse response = new GetProjectSettingsResponse();
            try
            {
                ALProject project = this.Server.Workspace.FindProject(parameters.path, true);
                if (project != null)
                {
                    response.mandatoryPrefixes = project.MandatoryPrefixes;
                    response.mandatorySuffixes = project.MandatorySuffixes;
                    response.mandatoryAffixes = project.MandatoryAffixes;
                }
            }
            catch (Exception e)
            {
                this.LogError(e);
            }

            return response;
        }
#pragma warning restore 1998

    }
}
