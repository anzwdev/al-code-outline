using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddDataClassificationWorkspaceCommand : SyntaxRewriterWorkspaceCommand<DataClassificationSyntaxRewriter>
    {
        public static string SortPropertiesParameterName = "sortProperties";
        public static string DataClassificationParameterName = "dataClassification";
        
        public AddDataClassificationWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addDataClassification", false)
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            if (parameters.ContainsKey(DataClassificationParameterName))
                this.SyntaxRewriter.DataClassification = parameters[DataClassificationParameterName];
            if (String.IsNullOrWhiteSpace(this.SyntaxRewriter.DataClassification))
                this.SyntaxRewriter.DataClassification = "CustomerContent";
            this.SyntaxRewriter.SortProperties = parameters.GetBoolValue(SortPropertiesParameterName);
        }

    }
}
