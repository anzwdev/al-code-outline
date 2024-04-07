using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
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
        public static string CommentParameterName = "comment";
        public static string SortPropertiesParameterName = "sortProperties";

        public SetPageFieldToolTipWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "setPageFieldToolTip")
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            this.SyntaxRewriter.ToolTip = parameters.GetStringValue(ToolTipParameterName);
            this.SyntaxRewriter.Comment = parameters.GetStringValue(CommentParameterName);
            this.SyntaxRewriter.SortProperties = parameters.GetBoolValue(SortPropertiesParameterName);
        }

    }
}
