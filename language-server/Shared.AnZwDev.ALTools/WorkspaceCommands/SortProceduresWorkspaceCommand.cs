using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SortProceduresWorkspaceCommand : SyntaxRewriterWorkspaceCommand<SortProceduresSyntaxRewriter>
    {

        public SortProceduresWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "sortProcedures")
        {
        }

    }
}
