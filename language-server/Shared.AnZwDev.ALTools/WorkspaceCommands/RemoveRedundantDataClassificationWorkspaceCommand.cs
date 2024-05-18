using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    internal class RemoveRedundantDataClassificationWorkspaceCommand : SyntaxRewriterWorkspaceCommand<RemoveRedundantDataClassificationSyntaxRewriter>
    {

        public static string SortPropertiesParameterName = "sortProperties";

        public RemoveRedundantDataClassificationWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "removeRedundantDataClassification")
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            this.SyntaxRewriter.SortProperties = parameters.GetBoolValue(SortPropertiesParameterName);
        }

    }
}
