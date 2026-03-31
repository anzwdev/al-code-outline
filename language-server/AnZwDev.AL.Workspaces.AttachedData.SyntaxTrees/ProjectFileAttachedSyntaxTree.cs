using AnZwDev.AL.Symbols.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using Microsoft.Dynamics.Nav.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace AnZwDev.AL.Workspaces.AttachedData.SyntaxTrees
{
    public class ProjectFileAttachedSyntaxTree : ProjectFileAttachedData
    {

        private SyntaxTree? _content = null;

        public ProjectFileAttachedSyntaxTree(ProjectFile projectFile) : base(projectFile, ProjectFileAttachedDataStorageMode.Open)
        {
        }

        protected override void Clear()
        {
            base.Clear();
            _content = null;
        }

        private ParseOptions GetParseOptions()
        {
            return ProjectFile
                .Project
                .SymbolsProvider
                .ProjectCodeSymbolsProvider
                .GetSymbols()
                .GetParseOptions();
        }

        public SyntaxTree Get(ParseOptions? parseOptions = null)
        {
            if (_content == null)
            {
                if (parseOptions == null)
                    parseOptions = GetParseOptions();
                var fileContent = ProjectFile.ReadAllText();
                var sourceText = SourceText.From(fileContent, ProjectFile.Encoding);
                _content = SyntaxTree.ParseObjectText(sourceText, ProjectFile.FullPath, parseOptions, ProjectFile.Encoding);
            }
            return _content;
        }

    }
}
