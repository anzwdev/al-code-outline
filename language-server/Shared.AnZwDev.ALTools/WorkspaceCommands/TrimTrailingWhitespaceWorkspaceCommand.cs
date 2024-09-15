using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Workspace;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Dynamics.Nav.CodeAnalysis;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class TrimTrailingWhitespaceWorkspaceCommand : SourceTextWorkspaceCommand
    {
        protected int _totalNoOfChanges = 0;
        protected int _noOfChangedFiles = 0;

        public TrimTrailingWhitespaceWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "trimTrailingWhitespace", false)
        {
        }

        protected override (string, bool, string) ProcessSourceCode(string sourceCode, ALProject project, string filePath, TextRange range, Dictionary<string, string> parameters)
        {
            string newSourceCode = sourceCode.MultilineTrimEnd();

            if (newSourceCode != sourceCode)
            {
                _totalNoOfChanges++;
                _noOfChangedFiles++;
            }

            return (newSourceCode, true, null);
        }

        public override WorkspaceCommandResult Run(string sourceCode, ALProject project, string filePath, TextRange range, Dictionary<string, string> parameters, List<string> excludeFiles, List<string> includeFiles)
        {
            this._totalNoOfChanges = 0;
            this._noOfChangedFiles = 0;

            WorkspaceCommandResult result = base.Run(sourceCode, project, filePath, range, parameters, excludeFiles, includeFiles);

            result.SetParameter(NoOfChangesParameterName, this._totalNoOfChanges.ToString());
            result.SetParameter(NoOfChangedFilesParameterName, this._noOfChangedFiles.ToString());
            return result;
        }

    }
}
