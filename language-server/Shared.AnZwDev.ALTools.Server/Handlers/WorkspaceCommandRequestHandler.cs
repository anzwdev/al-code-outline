using AnZwDev.ALTools.WorkspaceCommands;
using AnZwDev.ALTools.Server.Contracts;
using System;
using System.Threading.Tasks;
using StreamJsonRpc;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class WorkspaceCommandRequestHandler : RequestHandler
    {

        public WorkspaceCommandRequestHandler(LanguageServerHost languageServerHost) : base(languageServerHost)
        {
        }

        [JsonRpcMethod("al/workspacecommand", UseSingleObjectParameterDeserialization = true)]
        public WorkspaceCommandResponse WorkspaceCommand(WorkspaceCommandRequest parameters)
        {
            WorkspaceCommandResponse response = new WorkspaceCommandResponse();
            try
            {
                WorkspaceCommandResult commandResult = this.Server.WorkspaceCommandsManager.RunCommand(parameters.command, parameters.source, parameters.projectPath, parameters.filePath, parameters.range, parameters.parameters, parameters.excludeFiles);

                response.source = commandResult.Source;
                response.parameters = commandResult.Parameters;
                response.error = commandResult.Error;
                response.errorMessage = commandResult.ErrorMessage;
            }
            catch (Exception e)
            {
                response.error = true;
                response.errorMessage = e.Message;
                this.LogError(e);
            }
            return response;
        }


    }
}
