using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Workspace;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
#if BC
using Microsoft.Dynamics.Nav.CodeAnalysis.Workspaces.Formatting;
#endif
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class WorkspaceCommand
    {

        public static string NoOfChangesParameterName = "noOfChanges";
        public static string NoOfChangedFilesParameterName = "noOfChangedFiles";
        
        public ALDevToolsServer ALDevToolsServer { get; }
        public string Name { get; }
        public bool ModifiedSymbolsRebuildRequired { get; }

        public WorkspaceCommand(ALDevToolsServer alDevToolsServer, string newName, bool modifiedSymbolsRebuildRequired)
        {
            this.ALDevToolsServer = alDevToolsServer;
            this.Name = newName;
            this.ModifiedSymbolsRebuildRequired = modifiedSymbolsRebuildRequired;
        }

        public virtual (WorkspaceCommandResult, bool) CanRun(string sourceCode, ALProject alProject, string filePath, TextRange range, Dictionary<string, string> parameters, List<string> excludeFiles, List<string> includeFiles)
        {
            return (null, true);
        }

        public virtual WorkspaceCommandResult Run(string sourceCode, ALProject alProject, string filePath, TextRange range, Dictionary<string, string> parameters, List<string> excludeFiles, List<string> includeFiles)
        {
            return WorkspaceCommandResult.Empty;
        }

        protected virtual SyntaxNode FormatSyntaxNode(SyntaxNode node)
        {
#if BC
            if (node == null)
                return node;
            return Formatter.Format(node, this.GetWorkspace());
#else
            return node;
#endif
        }

#if BC
        private Microsoft.Dynamics.Nav.EditorServices.Protocol.VsCodeWorkspace _workspace = null;
        protected Microsoft.Dynamics.Nav.EditorServices.Protocol.VsCodeWorkspace GetWorkspace()
        {
            if (this._workspace == null)
                this._workspace =
                    new Microsoft.Dynamics.Nav.EditorServices.Protocol.VsCodeWorkspace();
            return this._workspace;
        }
#endif


        protected bool GetSkipFormattingValue(Dictionary<string, string> parameters)
        {
            return (
                (parameters != null) && 
                (parameters.ContainsKey("skipFormatting")) && 
                (parameters["skipFormatting"] != null) &&
                (parameters["skipFormatting"].Equals("true", StringComparison.OrdinalIgnoreCase)));
        }

        public virtual void CollectCodeActions(SyntaxTree syntaxTree, SyntaxNode node, TextRange range, List<WorkspaceCommandCodeAction> actions)
        {
        }

    }
}
