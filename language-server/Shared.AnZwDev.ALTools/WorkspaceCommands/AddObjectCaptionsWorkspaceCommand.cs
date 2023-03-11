using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;


namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddObjectCaptionsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<ObjectCaptionSyntaxRewriter>
    {

        public AddObjectCaptionsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addObjectCaptions")
        {
        }

    }
}
