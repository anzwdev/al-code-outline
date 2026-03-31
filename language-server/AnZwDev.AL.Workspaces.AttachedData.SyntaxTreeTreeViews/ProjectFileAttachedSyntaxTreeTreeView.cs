using AnZwDev.AL.Symbols.CodeAnalysis;
using AnZwDev.AL.Views.SyntaxTreeTreeViews;
using AnZwDev.AL.Workspaces;
using AnZwDev.AL.Workspaces.AttachedData.SyntaxTrees;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.AttachedData.SyntaxTreeSymbolsTreeViews
{
    public class ProjectFileAttachedSyntaxTreeTreeView : ProjectFileAttachedData
    {

        private readonly static SyntaxTreeTreeViewBuilder _builder = new SyntaxTreeTreeViewBuilder();

        private SyntaxTreeTreeViewNode? _content = null;
        private bool _contentCreated = false;

        public ProjectFileAttachedSyntaxTreeTreeView(ProjectFile projectFile) : 
            base(projectFile, ProjectFileAttachedDataStorageMode.Open)
        {
        }

        protected override void Clear()
        {
            base.Clear();
            _content = null;
            _contentCreated = false;
        }

        public SyntaxTreeTreeViewNode? Get()
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
