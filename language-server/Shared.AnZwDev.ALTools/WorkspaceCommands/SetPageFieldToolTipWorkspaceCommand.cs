using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SetPageFieldToolTipWorkspaceCommand : SyntaxRewriterWorkspaceCommand<SetPageFieldToolTipSyntaxRewriter>
    {
        public static string ToolTipParameterName = "toolTip";

        public SetPageFieldToolTipWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "setPageFieldToolTip")
        {
        }

        protected override void SetParameters(string sourceCode, string projectPath, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, projectPath, filePath, span, parameters);
            this.SyntaxRewriter.ToolTip = parameters.GetStringValue(ToolTipParameterName);
        }

    }
}
