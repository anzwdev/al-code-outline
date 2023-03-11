using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddAppAreasWorkspaceCommand: SyntaxRewriterWorkspaceCommand<AppAreaSyntaxRewriter>
    {

        public static string AppAreaParameterName = "appArea";
        public static string AppAreaModeParameterName = "appAreaMode";

        public AddAppAreasWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addAppAreas")
        {
        }

        protected override void SetParameters(string sourceCode, string projectPath, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, projectPath, filePath, span, parameters);
            if (parameters.ContainsKey(AppAreaParameterName))
                this.SyntaxRewriter.ApplicationAreaName = parameters[AppAreaParameterName];
            if (String.IsNullOrWhiteSpace(this.SyntaxRewriter.ApplicationAreaName))
                this.SyntaxRewriter.ApplicationAreaName = "All";
            if (parameters.ContainsKey(AppAreaModeParameterName))
                this.SyntaxRewriter.ApplicationAreaMode = parameters[AppAreaModeParameterName].ToEnum<AppAreaMode>();
        }

    }
}
