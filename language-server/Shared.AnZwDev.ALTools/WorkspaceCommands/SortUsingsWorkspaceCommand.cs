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
    public class SortUsingsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<SortUsingsSyntaxRewriter>
    {

        public static string SortSingleNodeRegionsParameterName = "sortSingleNodeRegions";
        public static string NamespacesOrderParameterName = "namespacesOrder";

        public SortUsingsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "sortUsings", false)
        {
        }

        protected override void SetParameters(string sourceCode, ALProject project, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, project, filePath, span, parameters);
            this.SyntaxRewriter.SortSingleNodeRegions = parameters.GetBoolValue(SortSingleNodeRegionsParameterName);
            this.SyntaxRewriter.PrefixesSortOrder = parameters.GetSeparatedStringArray(NamespacesOrderParameterName, ';');
        }

    }
#endif
}
