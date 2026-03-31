using AnZwDev.AL.Symbols.CodeAnalysis;
using AnZwDev.AL.Views.SyntaxTreeSymbolsTreeViews;
using AnZwDev.AL.Workspaces;
using AnZwDev.AL.Workspaces.AttachedData.SyntaxTrees;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.AttachedData.SyntaxTreeSymbolsTreeViews
{
    public class ProjectFileAttachedSyntaxTreeSymbolsTreeView : ProjectFileAttachedData
    {

        private readonly static SyntaxTreeSymbolsTreeViewBuilder _builder = new SyntaxTreeSymbolsTreeViewBuilder();

        private SyntaxTreeSymbolsTreeViewNode? _content = null;
        private bool _contentCreated = false;

        public ProjectFileAttachedSyntaxTreeSymbolsTreeView(ProjectFile projectFile) : 
            base(projectFile, ProjectFileAttachedDataStorageMode.Open)
        {
        }

        protected override void Clear()
        {
            base.Clear();
            _content = null;
            _contentCreated = false;
        }

        public SyntaxTreeSymbolsTreeViewNode? Get()
        {
            if (!_contentCreated)
            {
                var syntaxTree = ProjectFile.AttachedData.Get(ProjectFileAttachedSyntaxTreeFactory.Instance).Get();
                if (syntaxTree != null)
                    _content = _builder.CreateView(syntaxTree);
                _contentCreated = true;
            }
            return _content;
        }

    }
}
