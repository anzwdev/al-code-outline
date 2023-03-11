using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class LockRemovedFieldsCaptionsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<LockRemovedFieldsCaptionsSyntaxRewriter>
    {

        public LockRemovedFieldsCaptionsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "lockRemovedFieldsCaptions")
        {
        }

    }
}
