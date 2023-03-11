using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class MakeFlowFieldsReadOnlyWorkspaceCommand : SyntaxRewriterWorkspaceCommand<MakeFlowFieldsReadOnlySyntaxRewriter>
    {

        public MakeFlowFieldsReadOnlyWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "makeFlowFieldsReadOnly")
        {
        }

    }
}
