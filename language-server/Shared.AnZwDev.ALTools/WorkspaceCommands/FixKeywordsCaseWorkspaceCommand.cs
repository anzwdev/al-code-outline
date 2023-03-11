using AnZwDev.ALTools.CodeTransformations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class FixKeywordsCaseWorkspaceCommand : SyntaxRewriterWorkspaceCommand<KeywordCaseSyntaxRewriter>
    {

        public FixKeywordsCaseWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "fixKeywordsCase")
        {
        }

    }
}
