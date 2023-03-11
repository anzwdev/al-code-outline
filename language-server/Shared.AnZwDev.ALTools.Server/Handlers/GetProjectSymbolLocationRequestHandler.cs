using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.ALTools.Workspace;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class GetProjectSymbolLocationRequestHandler : BaseALRequestHandler<GetProjectSymbolLocationRequest, GetProjectSymbolLocationResponse>
    {

        public GetProjectSymbolLocationRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/projectsymbollocation")
        {
        }

#pragma warning disable 1998
        protected override async Task<GetProjectSymbolLocationResponse> HandleMessage(GetProjectSymbolLocationRequest parameters, RequestContext<GetProjectSymbolLocationResponse> context)
        {
            GetProjectSymbolLocationResponse response = new GetProjectSymbolLocationResponse();
            try
            {
                ALProject project = this.Server.Workspace.FindProject(parameters.projectPath);
                if (project != null)
                {
                    ALProjectSymbolsLibrarySource symbolsSource = new ALProjectSymbolsLibrarySource(project);
                    ALSymbol symbol = new ALSymbol(parameters.kind, parameters.name);
                    response.location = symbolsSource.GetSymbolSourceProjectLocation(symbol);
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
