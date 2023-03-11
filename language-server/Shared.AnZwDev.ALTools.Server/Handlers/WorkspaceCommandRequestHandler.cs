using AnZwDev.ALTools;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.WorkspaceCommands;
using AnZwDev.VSCodeLangServer.Protocol.Server;
using AnZwDev.VSCodeLangServer.Protocol.MessageProtocol;
using AnZwDev.ALTools.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AnZwDev.VSCodeLangServer.Utility;

namespace AnZwDev.ALTools.Server.Handlers
{
    public class WorkspaceCommandRequestHandler : BaseALRequestHandler<WorkspaceCommandRequest, WorkspaceCommandResponse>
    {

        public WorkspaceCommandRequestHandler(ALDevToolsServer server, LanguageServerHost languageServerHost) : base(server, languageServerHost, "al/workspacecommand")
        {
        }

#pragma warning disable 1998
        protected override async Task<WorkspaceCommandResponse> HandleMessage(WorkspaceCommandRequest parameters, RequestContext<WorkspaceCommandResponse> context)
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
#pragma warning restore 1998


    }
}
