using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SortTableFieldsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<SortTableFieldsSyntaxRewriter>
    {

        public static string SortSingleNodeRegionsParameterName = "sortSingleNodeRegions";

        public SortTableFieldsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "sortTableFields", false)
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            this.SyntaxRewriter.SortSingleNodeRegions = parameters.GetBoolValue(SortSingleNodeRegionsParameterName);
        }

    }
}
