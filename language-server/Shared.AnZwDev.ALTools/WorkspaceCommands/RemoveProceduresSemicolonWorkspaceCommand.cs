using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class RemoveProceduresSemicolonWorkspaceCommand : SyntaxRewriterWorkspaceCommand<RemoveProceduresSemicolonSyntaxRewriter>
    {

        public static string IncludeInterfacesParameterName = "includeInterfaces";

        public RemoveProceduresSemicolonWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "removeProceduresSemicolon")
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            this.SyntaxRewriter.IncludeInterfaces = parameters.GetBoolValue(IncludeInterfacesParameterName);
        }

    }
}
