using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    internal class AddTableDataCaptionFieldsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<AddTableDataCaptionFieldsRewriter>
    {

        public static string FieldNamesPatternsParameterName = "fieldsNamesPatterns";
        public static string SortPropertiesParameterName = "sortProperties";

        public AddTableDataCaptionFieldsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addTableDataCaptionFields")
        {
        }

        protected override void SetParameters(string sourceCode, string projectPath, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, projectPath, filePath, span, parameters);
            this.SyntaxRewriter.SortProperties = parameters.GetBoolValue(SortPropertiesParameterName);
            this.SyntaxRewriter.FieldNamesPatterns = parameters.GetStringListValue(FieldNamesPatternsParameterName);
        }

    }
}
