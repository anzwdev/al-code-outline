using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{

#if BC

    public class AddUsingRegionWorkspaceCommand : SyntaxRewriterWorkspaceCommand<AddUsingRegionSyntaxRewriter>
    {

        public static string TitleParameterName = "title";

        public AddUsingRegionWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addUsingRegion")
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            this.SyntaxRewriter.Title = parameters.GetStringValue(TitleParameterName);
        }

    }

#endif

}
