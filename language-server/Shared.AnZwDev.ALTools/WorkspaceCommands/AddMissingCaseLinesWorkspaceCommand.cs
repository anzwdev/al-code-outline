using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using AnZwDev.ALTools.Workspace;
using AnZwDev.ALTools.ALSymbols;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{

#if BC

    public class AddMissingCaseLinesWorkspaceCommand : SemanticModelSyntaxRewriterWorkspaceCommand<AddMissingCaseLinesSyntaxRewriter>
    {

        public static string IgnoreElseParameterName = "ignoreElse";

        public AddMissingCaseLinesWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addMissingCaseLines", true)
        {
        }

        protected override void SetParameters(SyntaxTree syntaxTree, SyntaxNode node, SemanticModel semanticModel, ALProject project, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(syntaxTree, node, semanticModel, project, span, parameters);
            this.SyntaxRewriter.IgnoreElseStatement = parameters.GetBoolValue(IgnoreElseParameterName);
        }

        public override void CollectCodeActions(SyntaxTree syntaxTree, SyntaxNode node, TextRange range, List<WorkspaceCommandCodeAction> actions)
        {
            if (node != null)
            {
                var caseStatement = node as CaseStatementSyntax;
                if (caseStatement == null)
                {
                    caseStatement = node.Parent as CaseStatementSyntax;
                }

                if (caseStatement != null)
                {
                    var nodeLineSpan = syntaxTree.GetLineSpan(caseStatement.FullSpan);
                    var nodeRange = new TextRange(nodeLineSpan);
                    actions.Add(new WorkspaceCommandCodeAction(this.Name, nodeRange, "Add missing case lines"));
                }
            }
        }

    }

#endif

}
