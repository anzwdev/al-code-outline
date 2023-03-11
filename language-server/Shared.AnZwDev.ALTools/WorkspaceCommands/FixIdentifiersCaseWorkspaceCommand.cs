using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
#if BC

    public class FixIdentifiersCaseWorkspaceCommand : SemanticModelWorkspaceCommand
    {

        public static string RemoveQuotesFromKeywordsParameterName = "removeQuotesFromKeywords";
        public static string RemoveQuotesFromDataTypeIdentifiersParameterName = "removeQuotesFromDataTypeIdentifiers";
        public static string RemoveQuotesFromNonDataTypeIdentifiersParameterName = "removeQuotesFromNonDataTypeIdentifiers";

        protected int _totalNoOfChanges = 0;
        protected int _noOfChangedFiles = 0;

        public FixIdentifiersCaseWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "fixIdentifiersCase")
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
                IdentifierCaseSyntaxRewriter identifierCaseSyntaxRewriter = new IdentifierCaseSyntaxRewriter();
                identifierCaseSyntaxRewriter.SemanticModel = semanticModel;
                identifierCaseSyntaxRewriter.Project = project;
                identifierCaseSyntaxRewriter.RemoveQuotesFromDataTypeIdentifiers = parameters.GetBoolValue(RemoveQuotesFromDataTypeIdentifiersParameterName);
                identifierCaseSyntaxRewriter.RemoveQuotesFromNonDataTypeIdentifiers = parameters.GetBoolValue(RemoveQuotesFromNonDataTypeIdentifiersParameterName);

                node = identifierCaseSyntaxRewriter.Visit(node);

                if (identifierCaseSyntaxRewriter.NoOfChanges > 0)
                {
                    this._noOfChangedFiles++;
                    this._totalNoOfChanges += identifierCaseSyntaxRewriter.NoOfChanges;
                }
            }

            return base.ProcessSyntaxNode(syntaxTree, node, semanticModel, project, span, parameters);
        }

    }

#endif
}
