using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{

#if BC

    public class RemoveStrSubstNoFromErrorWorkspaceCommand : SemanticModelSyntaxRewriterWorkspaceCommand<RemoveStrSubstNoFromErrorSyntaxRewriter>
    {

        public RemoveStrSubstNoFromErrorWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "removeStrSubstNoFromError")
        {
        }

    }

#endif

}
