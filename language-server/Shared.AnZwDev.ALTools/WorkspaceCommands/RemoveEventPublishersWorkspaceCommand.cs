using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{

#if BC

    public class RemoveEventPublishersWorkspaceCommand : SemanticModelSyntaxRewriterWorkspaceCommand<RemoveEventPublishersSyntaxRewriter>
    {

        public RemoveEventPublishersWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "removeEventPublishers")
        {
        }

    }

#endif
}
