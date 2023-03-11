using AnZwDev.ALTools.CodeTransformations;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;


namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SortPermissionSetListWorkspaceCommand : SyntaxRewriterWorkspaceCommand<SortPermissionSetListSyntaxRewriter>
    {

        public SortPermissionSetListWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "sortPermissionSetList")
        {
        }

        protected override SyntaxNode FormatSyntaxNode(SyntaxNode node)
        {
            //disable format because default formatter keeps all nodes in a single line
            return node;
        }

    }
}
