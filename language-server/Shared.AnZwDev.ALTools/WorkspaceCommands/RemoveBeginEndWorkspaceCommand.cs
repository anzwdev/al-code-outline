using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class RemoveBeginEndWorkspaceCommandd : SyntaxRewriterWorkspaceCommand<RemoveBeginEndSyntaxRewriter>
    {

        public RemoveBeginEndWorkspaceCommandd(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "removeBeginEnd")
        {
        }

    }
}
