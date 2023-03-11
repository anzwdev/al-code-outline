using AnZwDev.ALTools.CodeTransformations;
using AnZwDev.ALTools.Workspace.SymbolsInformation;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class RefreshToolTipsWorkspaceCommand : SyntaxRewriterWorkspaceCommand<RefreshToolTipsSyntaxRewriter>
    {

        public RefreshToolTipsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "refreshToolTips")
        {
        }

        protected override void SetParameters(string sourceCode, string projectPath, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            base.SetParameters(sourceCode, projectPath, filePath, span, parameters);
            if (this.SyntaxRewriter.Project?.Symbols?.Tables != null)
            {
                //collect list of dependencies
                List<string> dependencies = new List<string>();
                foreach (string parameterName in parameters.Keys)
                    if (parameterName.StartsWith("dependencyName"))
                        dependencies.Add(parameters[parameterName]);
                if (dependencies.Count == 0)
                    dependencies = null;
                //collect tooltips
                PageInformationProvider provider = new PageInformationProvider();
                this.SyntaxRewriter.ToolTipsCache = provider.CollectProjectTableFieldsToolTips(this.SyntaxRewriter.Project, dependencies);
            }
        }

        protected override void ClearParameters()
        {
            this.SyntaxRewriter.ToolTipsCache = null;
            base.ClearParameters();
        }

    }
}
