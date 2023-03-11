using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.SourceControl;
using Microsoft.Dynamics.Nav.CodeAnalysis;
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
        public string Name { get; set; }

        protected string[] ModifiedFilesNamesList { get; set; }
        protected HashSet<string> ModifiedFilesNamesHashSet { get; set; }

        public WorkspaceCommand(ALDevToolsServer alDevToolsServer, string newName)
        {
            this.ALDevToolsServer = alDevToolsServer;
            this.Name = newName;
            this.ModifiedFilesNamesList = null;
            this.ModifiedFilesNamesHashSet = null;
        }

        public virtual (WorkspaceCommandResult, bool) CanRun(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters, List<string> excludeFiles)
        {
            bool modifiedFilesOnly = this.GetModifiedFilesOnlyValue(parameters);
            if (modifiedFilesOnly)
            {
                try
                {
                    this.ModifiedFilesNamesList = GitClient.GetModifiedFiles(projectPath, ".al");
                    this.ModifiedFilesNamesHashSet = new HashSet<string>();
                    foreach (string name in this.ModifiedFilesNamesList)
                        this.ModifiedFilesNamesHashSet.Add(name);
                }
                catch (Exception ex)
                {
                    return (
                        new WorkspaceCommandResult("SourceControl", true, ex.Message),
                        false);
                }
            }
            else
            {
                this.ModifiedFilesNamesList = null;
                this.ModifiedFilesNamesHashSet = null;
            }

            return (null, true);
        }

        public virtual WorkspaceCommandResult Run(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters, List<string> excludeFiles)
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
                (parameters["skipFormatting"].Equals("true", StringComparison.CurrentCultureIgnoreCase)));
        }

        protected bool GetModifiedFilesOnlyValue(Dictionary<string, string> parameters)
        {
            return (
                (parameters != null) &&
                (parameters.ContainsKey("modifiedFilesOnly")) &&
                (parameters["modifiedFilesOnly"] != null) &&
                (parameters["modifiedFilesOnly"].Equals("true", StringComparison.CurrentCultureIgnoreCase)));
        }

    }
}
