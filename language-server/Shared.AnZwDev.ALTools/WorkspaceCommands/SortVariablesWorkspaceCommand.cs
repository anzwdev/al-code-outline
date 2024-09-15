using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SortVariablesWorkspaceCommand : SyntaxRewriterWorkspaceCommand<SortVariablesSyntaxRewriter>
    {

        public static string SortModeParameterName = "variablesSortMode";
        public static string SortSingleNodeRegionsParameterName = "sortSingleNodeRegions";

        public SortVariablesWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "sortVariables", false)
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            this.SyntaxRewriter.SortMode = VariableSortModeExtensions.FromString(parameters.GetStringValue(SortModeParameterName));
            this.SyntaxRewriter.SortSingleNodeRegions = parameters.GetBoolValue(SortSingleNodeRegionsParameterName);
        }


    }
}
