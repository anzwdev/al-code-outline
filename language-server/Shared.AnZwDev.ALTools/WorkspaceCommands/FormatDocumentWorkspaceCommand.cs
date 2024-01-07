using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Workspace;
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

        public override WorkspaceCommandResult Run(string sourceCode, ALProject project, string filePath, TextRange range, Dictionary<string, string> parameters, List<string> excludeFiles)
        {
            WorkspaceCommandResult result = base.Run(sourceCode, project, filePath, range, parameters, excludeFiles);
            result.SetParameter(NoOfChangesParameterName, "");
            result.SetParameter(NoOfChangedFilesParameterName, "");
            return result;
        }

        public override SyntaxNode ProcessSyntaxNode(SyntaxNode node, string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            return FormatSyntaxNode(node);
        }

    }
}
