using AnZwDev.ALTools.Server.Contracts;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class CollectWorkspaceCommandCodeActionsRequestHandler : RequestHandler
    {

        public CollectWorkspaceCommandCodeActionsRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/collectworkspacecodeactions", UseSingleObjectParameterDeserialization = true)]
        public CollectWorkspaceCommandCodeActionsResponse CollectWorkspaceCodeActions(CollectWorkspaceCommandCodeActionsRequest parameters)
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

    }

}
