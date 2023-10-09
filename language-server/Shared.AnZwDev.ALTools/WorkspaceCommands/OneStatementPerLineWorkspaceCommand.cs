using AnZwDev.ALTools.CodeTransformations;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class OneStatementPerLineWorkspaceCommand : SyntaxRewriterWorkspaceCommand<OneStatementPerLineSyntaxRewriter>
    {

        public OneStatementPerLineWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "oneStatementPerLine")
        {
        }

    }
}
