using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class AddPageControlCaptionWorkspaceCommand : SyntaxRewriterWorkspaceCommand<PageControlCaptionSyntaxRewriter>
    {

        public static string SetActionsCaptionsParameterName = "setActionsCaptions";
        public static string SetGroupsCaptionsParameterName = "setGroupsCaptions";
        public static string SetRepeatersCaptionsParameterName = "setRepeatersCaptions";
        public static string SetActionGroupsCaptionsParameterName = "setActionGroupsCaptions";
        public static string SetPartsCaptionsParameterName = "setPartsCaptions";
        public static string SetFieldsCaptionsParameterName = "setFieldsCaptions";
        public static string SetLabelsCaptionsParameterName = "setLabelsCaptions";
        public static string SortPropertiesParameterName = "sortProperties";

        public AddPageControlCaptionWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addPageControlCaptions")
        {
        }

        protected override void SetParameters(string sourceCode, string projectPath, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, projectPath, filePath, span, parameters);
            this.SyntaxRewriter.SetActionsCaptions = parameters.GetBoolValue(SetActionsCaptionsParameterName);
            this.SyntaxRewriter.SetGroupsCaptions = parameters.GetBoolValue(SetGroupsCaptionsParameterName);
            this.SyntaxRewriter.SetRepeatersCaptions = parameters.GetBoolValue(SetRepeatersCaptionsParameterName);
            this.SyntaxRewriter.SetActionGroupsCaptions = parameters.GetBoolValue(SetActionGroupsCaptionsParameterName);
            this.SyntaxRewriter.SetPartsCaptions = parameters.GetBoolValue(SetPartsCaptionsParameterName);
            this.SyntaxRewriter.SetFieldsCaptions = parameters.GetBoolValue(SetFieldsCaptionsParameterName);
            this.SyntaxRewriter.SetLabelCaptions = parameters.GetBoolValue(SetLabelsCaptionsParameterName);
            this.SyntaxRewriter.SortProperties = parameters.GetBoolValue(SortPropertiesParameterName);
        }

    }
}
