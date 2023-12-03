using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddDotToToolTipsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<AddDotToToolTipsSyntaxRewriter>
    {

        public AddDotToToolTipsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addTooTipsEndingDots")
        {
        }

    }
}
