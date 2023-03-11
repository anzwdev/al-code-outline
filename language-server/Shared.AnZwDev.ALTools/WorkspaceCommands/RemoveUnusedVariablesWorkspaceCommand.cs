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
    public class RemoveUnusedVariablesWorkspaceCommand : SemanticModelSyntaxRewriterWorkspaceCommand<RemoveUnusedVariablesSyntaxRewriter>
    {

        public static string RemoveGlobalVariablesParameterName = "removeGlobalVariables";
        public static string RemoveLocalVariablesParameterName = "removeLocalVariables";
        public static string RemoveLocalMethodParametersParameterName = "removeLocalMethodParameters";

        public RemoveUnusedVariablesWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "removeUnusedVariables")
        {
        }

        protected override void SetParameters(SyntaxTree syntaxTree, SyntaxNode node, SemanticModel semanticModel, ALProject project, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(syntaxTree, node, semanticModel, project, span, parameters);
            this.SyntaxRewriter.RemoveGlobalVariables = parameters.GetBoolValue(RemoveGlobalVariablesParameterName);
            this.SyntaxRewriter.RemoveLocalVariables = parameters.GetBoolValue(RemoveLocalVariablesParameterName);
            this.SyntaxRewriter.RemoveLocalMethodParameters = parameters.GetBoolValue(RemoveLocalMethodParametersParameterName);
        }

    }
#endif

}
