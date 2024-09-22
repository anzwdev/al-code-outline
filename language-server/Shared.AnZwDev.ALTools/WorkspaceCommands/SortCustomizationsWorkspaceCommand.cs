using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    internal class SortCustomizationsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<SortPageCustomizationsSyntaxRewriter>
    {

        public static string SortSingleNodeRegionsParameterName = "sortSingleNodeRegions";

        public SortCustomizationsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "sortCustomizations", false)
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            this.SyntaxRewriter.SortSingleNodeRegions = parameters.GetBoolValue(SortSingleNodeRegionsParameterName);
        }

        protected override SyntaxNode FormatSyntaxNode(SyntaxNode node)
        {
            return node;
        }

    }
}
