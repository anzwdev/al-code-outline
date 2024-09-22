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
    public class AddObjectCaptionsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<ObjectCaptionSyntaxRewriter>
    {

        public static string SortPropertiesParameterName = "sortProperties";

        public AddObjectCaptionsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addObjectCaptions", true)
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            this.SyntaxRewriter.SortProperties = parameters.GetBoolValue(SortPropertiesParameterName);
        }

    }
}
