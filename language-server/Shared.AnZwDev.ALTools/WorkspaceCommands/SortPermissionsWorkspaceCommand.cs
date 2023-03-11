using AnZwDev.ALTools.CodeTransformations;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SortPermissionsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<SortPermissionsSyntaxRewriter>
    {

        public SortPermissionsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "sortPermissions")
        {
        }

        protected override SyntaxNode FormatSyntaxNode(SyntaxNode node)
        {
            //disable format because default formatter keeps all permissions in a single line
            return node;
        }

    }
}
