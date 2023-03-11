using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{

#if BC

    public class SemanticModelSyntaxRewriterWorkspaceCommand<T> : SemanticModelWorkspaceCommand where T: ALSemanticModelSyntaxRewriter, new()
    {
        public T SyntaxRewriter { get; }

        public SemanticModelSyntaxRewriterWorkspaceCommand(ALDevToolsServer alDevToolsServer, string name) : base(alDevToolsServer, name)
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

        public override SyntaxNode ProcessSyntaxNode(SyntaxTree syntaxTree, SyntaxNode node, SemanticModel semanticModel, ALProject project, TextSpan span, Dictionary<string, string> parameters)
        {
            this.SetParameters(syntaxTree, node, semanticModel, project, span, parameters);
            node = this.SyntaxRewriter.ProcessNode(node);
            node = base.ProcessSyntaxNode(syntaxTree, node, semanticModel, project, span, parameters);
            this.ClearParameters();
            return node;
        }

        protected virtual void SetParameters(SyntaxTree syntaxTree, SyntaxNode node, SemanticModel semanticModel, ALProject project, TextSpan span, Dictionary<string, string> parameters)
        {
            this.SyntaxRewriter.Project = project;
            this.SyntaxRewriter.SemanticModel = semanticModel;
            this.SyntaxRewriter.Span = span;
        }

        protected virtual void ClearParameters()
        {
            this.SyntaxRewriter.Project = null;
            this.SyntaxRewriter.SemanticModel = null;
        }

    }

#endif

}
