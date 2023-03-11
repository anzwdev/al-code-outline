using AnZwDev.ALTools.CodeTransformations;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    internal class SortCustomizationsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<SortPageCustomizationsSyntaxRewriter>
    {

        public SortCustomizationsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "sortCustomizations")
        {
        }

        protected override SyntaxNode FormatSyntaxNode(SyntaxNode node)
        {
            return node;
        }

    }
}
