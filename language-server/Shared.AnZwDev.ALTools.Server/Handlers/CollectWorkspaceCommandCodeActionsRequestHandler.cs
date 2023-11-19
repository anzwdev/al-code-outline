using AnZwDev.ALTools.Server.Contracts;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class CollectWorkspaceCommandCodeActionsRequestHandler : BaseALRequestHandler<CollectWorkspaceCommandCodeActionsRequest, CollectWorkspaceCommandCodeActionsResponse>
    {

        public CollectWorkspaceCommandCodeActionsRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/collectworkspacecodeactions")
        {
        }

#pragma warning disable 1998
        protected override async Task<CollectWorkspaceCommandCodeActionsResponse> HandleMessage(CollectWorkspaceCommandCodeActionsRequest parameters, RequestContext<CollectWorkspaceCommandCodeActionsResponse> context)
        {
            CollectWorkspaceCommandCodeActionsResponse response = new CollectWorkspaceCommandCodeActionsResponse();
            try
            {
                response.CodeActions = this.Server.WorkspaceCommandsManager.GetCodeActions(parameters.Source, parameters.ProjectPath, parameters.FilePath, parameters.Range);
            }
            catch (Exception e)
            {
                response.Error = true;
                response.ErrorMessage = e.Message;
                this.LogError(e);
            }
            return response;
        }
#pragma warning restore 1998
    }

}
