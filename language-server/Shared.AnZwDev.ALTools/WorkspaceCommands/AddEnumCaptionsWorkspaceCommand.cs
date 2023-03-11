using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{

#if BC

    public class AddEnumCaptionsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<EnumCaptionSyntaxRewriter>
    {

        public static string SortPropertiesParameterName = "sortProperties";

        public AddEnumCaptionsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addEnumCaptions")
        {
        }

        protected override void SetParameters(string sourceCode, string projectPath, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, projectPath, filePath, span, parameters);
            this.SyntaxRewriter.SortProperties = parameters.GetBoolValue(SortPropertiesParameterName);
        }

    }

#endif

}
