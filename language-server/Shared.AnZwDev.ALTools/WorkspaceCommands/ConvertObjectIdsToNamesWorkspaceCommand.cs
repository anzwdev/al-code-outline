using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class ConvertObjectIdsToNamesWorkspaceCommand : SemanticModelSyntaxRewriterWorkspaceCommand<ConvertObjectIdsToNamesSyntaxRewriter>
    {

        public ConvertObjectIdsToNamesWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "convertObjectIdsToNames")
        {
        }

    }
}
