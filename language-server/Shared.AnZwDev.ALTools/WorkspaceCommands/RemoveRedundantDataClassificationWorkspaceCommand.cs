using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    internal class RemoveRedundantDataClassificationWorkspaceCommand : SyntaxRewriterWorkspaceCommand<RemoveRedundantDataClassificationSyntaxRewriter>
    {

        public RemoveRedundantDataClassificationWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "removeRedundantDataClassification")
        {
        }

    }
}
