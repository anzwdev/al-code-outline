using AnZwDev.ALTools.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class FormatDocumentWorkspaceCommand : SyntaxTreeWorkspaceCommand
    {

        public FormatDocumentWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "formatDocument")
        {
        }

        public override WorkspaceCommandResult Run(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters, List<string> excludeFiles)
        {
            WorkspaceCommandResult result = base.Run(sourceCode, projectPath, filePath, range, parameters, excludeFiles);
            result.SetParameter(NoOfChangesParameterName, "");
            result.SetParameter(NoOfChangedFilesParameterName, "");
            return result;
        }

        public override SyntaxNode ProcessSyntaxNode(SyntaxNode node, string sourceCode, string projectPath, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            return FormatSyntaxNode(node);
        }

    }
}
