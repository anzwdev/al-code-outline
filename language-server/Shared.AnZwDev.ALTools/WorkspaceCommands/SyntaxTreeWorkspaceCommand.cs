using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using AnZwDev.ALTools.Logging;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;

namespace AnZwDev.ALTools.WorkspaceCommands
{
    public class SyntaxTreeWorkspaceCommand : SourceTextWorkspaceCommand
    {

        public SyntaxTreeWorkspaceCommand(ALDevToolsServer alDevToolsServer, string name) : base(alDevToolsServer, name)
        {
        }

        protected override (string, bool, string) ProcessSourceCode(string sourceCode, string projectPath, string filePath, Range range, Dictionary<string, string> parameters)
        {
            try
            {
                //parse source code
                SourceText sourceText = SourceText.From(sourceCode);
                SyntaxTree syntaxTree = SyntaxTree.ParseObjectText(sourceText);

                //convert range to TextSpan
                TextSpan span = new TextSpan(0, 0);
                if (range != null)
                {
                    LinePositionSpan srcRange = new LinePositionSpan(new LinePosition(range.start.line, range.start.character), new LinePosition(range.end.line, range.end.character));
                    span = sourceText.Lines.GetTextSpan(srcRange);
                }

                //fix nodes
                SyntaxNode node = this.ProcessSyntaxNode(syntaxTree.GetRoot(), sourceCode, projectPath, filePath, span, parameters);

                //return new source code
                if (node == null)
                    return (null, true, null);

                return (node.ToFullString(), true, null);

            }
            catch (Exception ex)
            {
                string errorMessage = (String.IsNullOrEmpty(filePath)) ?
                    $"Workspace command {this.Name} error during processing source code" : $"Workspace command {this.Name} error during processing file '{filePath}'";
                MessageLog.LogError(ex, errorMessage + ": ");
                return (null, false, errorMessage);
            }
        }

        /*
        protected virtual (bool, string) ProcessDirectory(string projectPath, Dictionary<string, string> parameters)
        {
            string[] filePathsList = System.IO.Directory.GetFiles(projectPath, "*.al", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < filePathsList.Length; i++)
            {
                (bool success, string errorMessage) = this.ProcessFile(projectPath, filePathsList[i], parameters);
                if (!success)
                    return (false, errorMessage);
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
        */

        public virtual SyntaxNode ProcessSyntaxNode(SyntaxNode node, string sourceCode, string projectPath, string filePath, TextSpan span, Dictionary<string, string> parameters)
        {
            bool skipFormatting = ((parameters != null) && (parameters.ContainsKey("skipFormatting")) && (parameters["skipFormatting"] == "true"));
            if (!skipFormatting)
                node = FormatSyntaxNode(node);
            return node;
        }

    }
}
