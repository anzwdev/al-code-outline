using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{

#if BC
    public class ConvertObjectIdsToNamesWorkspaceCommand : SemanticModelSyntaxRewriterWorkspaceCommand<ConvertObjectIdsToNamesSyntaxRewriter>
#else
    public class ConvertObjectIdsToNamesWorkspaceCommand : SyntaxRewriterWorkspaceCommand<ConvertObjectIdsToNamesSyntaxRewriter>

#endif
    {

        public ConvertObjectIdsToNamesWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "convertObjectIdsToNames")
        {
        }

    }


}
