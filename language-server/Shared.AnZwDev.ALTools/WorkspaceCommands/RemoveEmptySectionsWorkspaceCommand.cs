using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class RemoveEmptySectionsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<RemoveEmptySectionsSyntaxRewriter>
    {

        //public static string RemovePageFieldGroupsParameterName = "removePageFieldGroups";
        public static string RemoveActionGroupsParameterName = "removeActionGroups";
        public static string RemoveActionsParameterName = "removeActions";
        public static string IgnoreCommentsParameterName = "ignoreComments";
        public static string IncludeObsoleteParameterName = "includeObsolete";

        public RemoveEmptySectionsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "removeEmptySections")
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            //this.SyntaxRewriter.RemovePageFieldGroups = parameters.GetBoolValue(RemovePageFieldGroupsParameterName);
            this.SyntaxRewriter.RemovePageFieldGroups = false;
            this.SyntaxRewriter.RemoveActionGroups = parameters.GetBoolValue(RemoveActionGroupsParameterName);
            this.SyntaxRewriter.RemoveActions = parameters.GetBoolValue(RemoveActionsParameterName);
            this.SyntaxRewriter.IgnoreComments = parameters.GetBoolValue(IgnoreCommentsParameterName);
            this.SyntaxRewriter.IncludeObsolete = parameters.GetBoolValue(IncludeObsoleteParameterName);
        }

    }
}
