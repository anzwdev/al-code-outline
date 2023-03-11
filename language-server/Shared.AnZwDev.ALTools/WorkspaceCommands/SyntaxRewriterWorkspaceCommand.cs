using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SyntaxRewriterWorkspaceCommand<T> : SyntaxTreeWorkspaceCommand where T: ALSyntaxRewriter, new()
    {

        public T SyntaxRewriter { get; }

        public SyntaxRewriterWorkspaceCommand(ALDevToolsServer alDevToolsServer, string name): base(alDevToolsServer, name)
        {
            this.SyntaxRewriter = new T();
        }

        public override WorkspaceCommandResult Run(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters, List<string> excludeFiles)
        {
            this.SyntaxRewriter.TotalNoOfChanges = 0;
            this.SyntaxRewriter.NoOfChangedFiles = 0;
            this.SyntaxRewriter.NoOfChanges = 0;

            WorkspaceCommandResult result = base.Run(sourceCode, projectPath, filePath, range, parameters, excludeFiles);

            result.SetParameter(NoOfChangesParameterName, this.SyntaxRewriter.TotalNoOfChanges.ToString());
            result.SetParameter(NoOfChangedFilesParameterName, this.SyntaxRewriter.NoOfChangedFiles.ToString());
            return result;
        }

        public override SyntaxNode ProcessSyntaxNode(SyntaxNode node, string sourceCode, string projectPath, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            this.SetParameters(sourceCode, projectPath, filePath, span, parameters);
            node = this.SyntaxRewriter.ProcessNode(node);
            node = base.ProcessSyntaxNode(node, sourceCode, projectPath, filePath, span, parameters);
            this.ClearParameters();
            return node;
        }

        protected virtual void SetParameters(string sourceCode, string projectPath, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            this.SyntaxRewriter.Project = this.ALDevToolsServer.Workspace.FindProject(projectPath, true);
            this.SyntaxRewriter.Span = span;
        }

        protected virtual void ClearParameters()
        {
            this.SyntaxRewriter.Project = null;
        }


    }
}
