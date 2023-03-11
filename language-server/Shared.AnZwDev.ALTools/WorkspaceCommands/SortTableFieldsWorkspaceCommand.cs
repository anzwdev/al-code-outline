using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SortTableFieldsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<SortTableFieldsSyntaxRewriter>
    {

        public SortTableFieldsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "sortTableFields")
        {
        }

    }
}
