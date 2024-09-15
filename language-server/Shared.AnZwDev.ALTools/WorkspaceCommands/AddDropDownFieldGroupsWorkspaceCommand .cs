using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddDropDownFieldGroupsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<AddDropDownFieldGroupsRewriter>
    {

        public static string FieldNamesPatternsParameterName = "fieldsNamesPatterns";

        public AddDropDownFieldGroupsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addDropDownFieldGroups", true)
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            this.SyntaxRewriter.FieldNamesPatterns = parameters.GetStringListValue(FieldNamesPatternsParameterName);
        }

    }
}
