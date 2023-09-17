using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Dynamics.Nav.CodeAnalysis;

namespace AnZwDev.ALTools.WorkspaceCommands
{

    public class RemoveEmptyLinesWorkspaceCommand : SyntaxRewriterWorkspaceCommand<RemoveEmptyLinesSyntaxRewriter>
    {

        public RemoveEmptyLinesWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "removeEmptyLines")
        {
        }

        protected override (string, bool, string) ProcessSourceCode(string sourceCode, ALProject project, string filePath, Range range, Dictionary<string, string> parameters)
        {
            string newSourceCode = sourceCode.RemoveDuplicateEmptyLines();
            int prevNoOfChangedFiles = this.SyntaxRewriter.NoOfChangedFiles;
            bool fileChanged = (newSourceCode != sourceCode);

            (string processedSource, bool isError, string errorMessage) = base.ProcessSourceCode(newSourceCode, project, filePath, range, parameters);

            if ((fileChanged) && (prevNoOfChangedFiles == this.SyntaxRewriter.NoOfChangedFiles))
                this.SyntaxRewriter.NoOfChangedFiles++;

            return (processedSource, isError, errorMessage);
        }

    }

}
