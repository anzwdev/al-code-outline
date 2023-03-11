using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddDropDownFieldGroupsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<AddDropDownFieldGroupsRewriter>
    {

        public static string FieldNamesPatternsParameterName = "fieldsNamesPatterns";

        public AddDropDownFieldGroupsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addDropDownFieldGroups")
        {
        }

        protected override void SetParameters(string sourceCode, string projectPath, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, projectPath, filePath, span, parameters);
            this.SyntaxRewriter.FieldNamesPatterns = parameters.GetStringListValue(FieldNamesPatternsParameterName);
        }

    }
}
