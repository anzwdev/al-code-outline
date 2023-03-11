using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class TrimTrailingWhitespaceWorkspaceCommand : SourceTextWorkspaceCommand
    {
        protected int _totalNoOfChanges = 0;
        protected int _noOfChangedFiles = 0;

        public TrimTrailingWhitespaceWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "trimTrailingWhitespace")
        {
        }

        protected override (string, bool, string) ProcessSourceCode(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters)
        {
            string newSourceCode = sourceCode.MultilineTrimEnd();

            if (newSourceCode != sourceCode)
            {
                _totalNoOfChanges++;
                _noOfChangedFiles++;
            }

            return (newSourceCode, true, null);
        }

        public override WorkspaceCommandResult Run(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters, List<string> excludeFiles)
        {
            this._totalNoOfChanges = 0;
            this._noOfChangedFiles = 0;

            WorkspaceCommandResult result = base.Run(sourceCode, projectPath, filePath, range, parameters, excludeFiles);

            result.SetParameter(NoOfChangesParameterName, this._totalNoOfChanges.ToString());
            result.SetParameter(NoOfChangedFilesParameterName, this._noOfChangedFiles.ToString());
            return result;
        }

    }
}
