using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SortPropertiesWorkspaceCommand : SyntaxRewriterWorkspaceCommand<SortPropertiesSyntaxRewriter>
    {

        public SortPropertiesWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "sortProperties")
        {
        }

    }
}
