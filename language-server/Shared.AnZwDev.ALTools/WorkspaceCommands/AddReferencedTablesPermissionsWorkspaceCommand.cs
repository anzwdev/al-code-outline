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

    public class AddReferencedTablesPermissionsWorkspaceCommand : SemanticModelSyntaxRewriterWorkspaceCommand<AddReferencedTablesPermissionsSyntaxRewriter>
    {

        public static string SortPropertiesParameterName = "sortProperties";

        public AddReferencedTablesPermissionsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "addReferencedTablesPermissions")
        {
        }

        protected override SyntaxNode FormatSyntaxNode(SyntaxNode node)
        {
            //disable format because default formatter keeps all permissions in a single line
            return node;
        }

        protected override void SetParameters(SyntaxTree syntaxTree, SyntaxNode node, SemanticModel semanticModel, ALProject project, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(syntaxTree, node, semanticModel, project, span, parameters);
            this.SyntaxRewriter.SortProperties = parameters.GetBoolValue(SortPropertiesParameterName);
        }


    }

#endif

}
