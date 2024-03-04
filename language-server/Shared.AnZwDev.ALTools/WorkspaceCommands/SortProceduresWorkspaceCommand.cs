using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.WorkspaceCommands.Parameters;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    internal class SortProceduresWorkspaceCommand : SortProceduresOrTriggersWorkspaceCommand
    {

        public SortProceduresWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "sortProcedures")
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            this.SyntaxRewriter.SortProcedures = true;
        }

    }
}
