using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class RemoveRedundantAppAreasWorkspaceCommand : SyntaxRewriterWorkspaceCommand<RemoveRedundandAppAreasSyntaxRewriter>
    {

        public RemoveRedundantAppAreasWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "removeRedundantAppAreas")
        {
        }
    }
}
