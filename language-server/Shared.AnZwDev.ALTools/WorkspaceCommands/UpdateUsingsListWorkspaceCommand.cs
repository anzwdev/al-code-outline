using AnZwDev.ALTools.CodeTransformations.Namespaces;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{

#if BC
    public class UpdateUsingsListWorkspaceCommand : SyntaxRewriterWorkspaceCommand<UodateUsingsListSyntaxRewriter>
    {

        public static string RemoveUnusedUsingsParameterName = "removeUnusedUsings";
        public static string AddMissingUsingsParameterName = "addMissingUsings";

        public UpdateUsingsListWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "updateUsingsList")
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            this.SyntaxRewriter.RemoveUnusedUsings = parameters.GetBoolValue(RemoveUnusedUsingsParameterName);
            this.SyntaxRewriter.AddMissingUsings = parameters.GetBoolValue(AddMissingUsingsParameterName);
        }

        protected override SyntaxNode FormatSyntaxNode(SyntaxNode node)
        {
            //disable format because default formatter keeps all nodes in a single line
            return node;
        }
    }
#endif
}
