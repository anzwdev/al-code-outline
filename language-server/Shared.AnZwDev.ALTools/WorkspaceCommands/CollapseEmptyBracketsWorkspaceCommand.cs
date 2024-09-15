using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Workspace;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class CollapseEmptyBracketsWorkspaceCommand : SourceTextWorkspaceCommand
    {

        private enum SourceProcessingMode
        {
            Code,
            String,
            Name,
            MultiLineComment,
            SingleLineComment
        }

        private int _noOfChangedFiles = 0;
        private int _totalNoOfChanges = 0;

        public CollapseEmptyBracketsWorkspaceCommand(ALDevToolsServer alDevToolsServer) : base(alDevToolsServer, "collapseEmptyBrackets", false)
        {
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


        protected override (string, bool, string) ProcessSourceCode(string sourceCode, ALProject project, string filePath, TextRange range, Dictionary<string, string> parameters)
        {
            var mode = SourceProcessingMode.Code;
            var openBracketPos = -1;
            var lastValidPreBracketPos = -1;
            var builder = new StringBuilder();
            var lastPartEndPos = -1;
            var noOfChanges = 0;

            var pos = 0;
            while (pos < sourceCode.Length)
            {
                char currentChar = sourceCode[pos];
                switch (currentChar)
                {
                    case '\n':
                    case '\r':
                        if (mode == SourceProcessingMode.SingleLineComment)
                            mode = SourceProcessingMode.Code;
                        break;
                    case '/':
                        if ((mode == SourceProcessingMode.Code) && (pos < (sourceCode.Length - 1)))
                        {
                            var nextChar = sourceCode[pos + 1];
                            switch (nextChar)
                            {
                                case '/':
                                    mode = SourceProcessingMode.SingleLineComment;
                                    pos++;
                                    break;
                                case '*':
                                    mode = SourceProcessingMode.MultiLineComment;
                                    pos++;
                                    break;
                            }
                        }
                        break;
                    case '*':
                        if ((mode == SourceProcessingMode.MultiLineComment) && (pos < (sourceCode.Length - 1)) && (sourceCode[pos+1] == '/'))
                        {
                            mode = SourceProcessingMode.Code;
                            pos++;
                        }
                        break;
                    case '"':
                        switch (mode)
                        {
                            case SourceProcessingMode.Code:
                                mode = SourceProcessingMode.Name;
                                break;
                            case SourceProcessingMode.Name:
                                mode = SourceProcessingMode.Code;
                                break;
                        }
                        break;
                    case '\'':
                        switch (mode)
                        {
                            case SourceProcessingMode.Code:
                                mode = SourceProcessingMode.String;
                                break;
                            case SourceProcessingMode.String:
                                mode = SourceProcessingMode.Code;
                                break;
                        }
                        break;
                }


                if (mode == SourceProcessingMode.Code)
                {
                    currentChar = sourceCode[pos];
                    switch (currentChar)
                    {
                        case '{':
                            if (openBracketPos >= 0)
                            {
                                openBracketPos = -1;
                                lastValidPreBracketPos = -1;
                            } else if (lastValidPreBracketPos >= 0)
                                openBracketPos = pos;
                            break;
                        case '}':
                            if ((openBracketPos >= 0) && (lastValidPreBracketPos >= 0))
                            {
                                var part = sourceCode.Substring(lastPartEndPos + 1, lastValidPreBracketPos - lastPartEndPos);
                                builder.Append(part);
                                builder.Append(" { }");
                                lastPartEndPos = pos;
                                noOfChanges++;
                            }
                            openBracketPos = -1;
                            lastValidPreBracketPos = -1;
                            break;
                        case '\r':
                        case '\n':
                        case '\t':
                        case ' ':
                            break;
                        default:
                            if (
                                (currentChar == '"') ||
                                (currentChar == ')') ||
                                ((currentChar >= 'a') && (currentChar <= 'z')) ||
                                ((currentChar >= 'A') && (currentChar <= 'Z')) ||
                                ((currentChar >= '0') && (currentChar <= '9'))
                                )
                            {
                                lastValidPreBracketPos = pos;
                            }
                            else
                                lastValidPreBracketPos = -1;
                            openBracketPos = -1;
                            break;
                    }
                }
                else
                {
                    openBracketPos = -1;
                    lastValidPreBracketPos = -1;
                }

                pos++;
            }

            if (lastPartEndPos >= 0)
            {
                if (lastPartEndPos < (sourceCode.Length - 1))
                    builder.Append(sourceCode.Substring(lastPartEndPos + 1));
                sourceCode = builder.ToString();
            }

            _totalNoOfChanges += noOfChanges;
            if (noOfChanges > 0)
                _noOfChangedFiles++;

            return (sourceCode, true, null);
        }

    }

}
