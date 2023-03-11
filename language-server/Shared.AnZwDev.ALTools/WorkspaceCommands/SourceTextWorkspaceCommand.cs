using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Core;
using AnZwDev.ALTools.SourceControl;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SourceTextWorkspaceCommand : WorkspaceCommand
    {

        public SourceTextWorkspaceCommand(ALDevToolsServer alDevToolsServer, string name) : base(alDevToolsServer, name)
        {
        }

        public override WorkspaceCommandResult Run(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters, List<string> excludeFiles)
        {
            string newSourceCode = null;
            bool success = true;
            string errorMessage = null;

            if (!String.IsNullOrWhiteSpace(filePath))
            {
                if (!String.IsNullOrEmpty(sourceCode))
                {
                    (newSourceCode, success, errorMessage) = this.ProcessSourceCode(sourceCode, projectPath, filePath, range, parameters);
                    if (!success)
                        return new WorkspaceCommandResult(newSourceCode, true, errorMessage);
                }
            }
            else if (!String.IsNullOrWhiteSpace(projectPath))
                (success, errorMessage) = this.ProcessDirectory(projectPath, parameters, excludeFiles);

            if (success)
                return new WorkspaceCommandResult(newSourceCode);
            return new WorkspaceCommandResult(newSourceCode, true, errorMessage);
        }

        protected virtual (string, bool, string) ProcessSourceCode(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters)
        {
            return (sourceCode, true, null);
        }

        protected virtual (bool, string) ProcessDirectory(string projectPath, Dictionary<string, string> parameters, List<string> excludeFiles)
        {
            string[] filePathsList;

            bool modifiedFilesOnly = this.GetModifiedFilesOnlyValue(parameters);
            if (modifiedFilesOnly)
                filePathsList = this.ModifiedFilesNamesList;
            else
                filePathsList = System.IO.Directory.GetFiles(projectPath, "*.al", System.IO.SearchOption.AllDirectories);

            var matcher = new ExcludedFilesMatcher(excludeFiles);

            for (int i = 0; i < filePathsList.Length; i++)
            {
                if (matcher.ValidFile(projectPath, filePathsList[i]))
                {
                    (bool success, string errorMessage) = this.ProcessFile(projectPath, filePathsList[i], parameters);
                    if (!success)
                        return (false, errorMessage);
                }
            }
            return (true, null);
        }

        protected virtual (bool, string) ProcessFile(string projectPath, string filePath, Dictionary<string, string> parameters)
        {
            string source = FileUtils.SafeReadAllText(filePath);
            (string newSource, bool success, string errorMessage) = this.ProcessSourceCode(source, projectPath, filePath, null, parameters);
            if ((success) && (newSource != source) && (!String.IsNullOrWhiteSpace(newSource)))
                System.IO.File.WriteAllText(filePath, newSource);
            return (success, errorMessage);
        }

    }
}
