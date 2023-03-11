using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.CommandLine;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{

#if BC
    public class RemoveWithWorkspaceCommand: SemanticModelWorkspaceCommand
    {

        protected int _totalNoOfChanges = 0;
        protected int _noOfChangedFiles = 0;

        public RemoveWithWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "removeWith")
        {
        }

        public override WorkspaceCommandResult Run(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters, List<string> excludeFiles)
        {
            this._totalNoOfChanges = 0;
            this._noOfChangedFiles = 0;

            WorkspaceCommandResult result = base.Run(sourceCode, projectPath, filePath, range, parameters, excludeFiles);

            result.SetParameter(NoOfChangesParameterName, this._totalNoOfChanges.ToString());
            result.SetParameter(NoOfChangedFilesParameterName, this._noOfChangedFiles.ToString());
            return result;
        }

        public override SyntaxNode ProcessSyntaxNode(SyntaxTree syntaxTree, SyntaxNode node, SemanticModel semanticModel, ALProject project, TextSpan span, Dictionary<string, string> parameters)
        {
            if (node != null)
            {                
                //stage 1 - update calls
                WithIdentifiersSyntaxRewriter identifiersRewriter = new WithIdentifiersSyntaxRewriter();
                identifiersRewriter.SemanticModel = semanticModel;
                node = identifiersRewriter.Visit(node);

                int noOfChanges = identifiersRewriter.NoOfChanges;

                //stage 2 - remove "with" statements
                if (node != null)
                {
                    WithRemoveSyntaxRewriter withRemoveSyntaxRewriter = new WithRemoveSyntaxRewriter();
                    node = withRemoveSyntaxRewriter.Visit(node);

                    noOfChanges += withRemoveSyntaxRewriter.NoOfChanges;
                    if (noOfChanges > 0)
                    {
                        this._noOfChangedFiles++;
                        this._totalNoOfChanges += noOfChanges;
                    }
                }
            }

            return base.ProcessSyntaxNode(syntaxTree, node, semanticModel, project, span, parameters);
        }

        protected override CompilerFeatures GetCompilerFeatures(ProjectManifest manifest)
        {
            var features = base.GetCompilerFeatures(manifest);
            features &= (~CompilerFeatures.NoImplicitWith);
            return features;
        }

    }
#endif
}
