using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{

#if BC

    public class AddParenthesesWorkspaceCommand : SemanticModelSyntaxRewriterWorkspaceCommand<AddParenthesesSyntaxRewriter>
    {

        public AddParenthesesWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addParentheses")
        {
        }

    }

#endif

}
