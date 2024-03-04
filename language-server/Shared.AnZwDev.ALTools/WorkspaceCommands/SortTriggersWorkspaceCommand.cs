using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    internal class SortTriggersWorkspaceCommand : SortProceduresOrTriggersWorkspaceCommand
    {

        public SortTriggersWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "sortTriggers")
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            this.SyntaxRewriter.SortProcedures = false;
            if (this.SyntaxRewriter.TriggerSortMode == CodeTransformations.SortProceduresTriggerSortMode.None)
                this.SyntaxRewriter.TriggerSortMode = CodeTransformations.SortProceduresTriggerSortMode.NaturalOrder;
        }

    }
}
