using AnZwDev.AL.Symbols.CodeAnalysis;
using AnZwDev.AL.Views.SyntaxTreeViewerTreeViews;
using AnZwDev.AL.Workspaces;
using AnZwDev.AL.Workspaces.AttachedData.SyntaxTrees;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.AL.Workspaces.AttachedData.SyntaxTreeViewersStates
{
    public class ProjectFileAttachedSyntaxTreeViewerState : ProjectFileAttachedData
    {

        private readonly static SyntaxTreeViewerTreeViewBuildersCollection _treeBuilder = new SyntaxTreeViewerTreeViewBuildersCollection();
        private readonly static SyntaxTreeViewerPropertiesBuilder _propertyListBuilder = new SyntaxTreeViewerPropertiesBuilder();

        private SyntaxTreeViewerTreeNode? _treeRoot = null;
        public SyntaxTreeViewerViewMode ViewMode { get; private set; } = SyntaxTreeViewerViewMode.ChildNodesBased;
        private bool _contentCreated = false;

        public ProjectFileAttachedSyntaxTreeViewerState(ProjectFile projectFile) :
            base(projectFile, ProjectFileAttachedDataStorageMode.Open)
        {
        }

        protected override void Clear()
        {
            base.Clear();
            _treeRoot = null;
            _contentCreated = false;
        }

        public SyntaxTreeViewerTreeNode? Get(SyntaxTreeViewerViewMode viewMode)
        {
            if ((!_contentCreated) || (viewMode != ViewMode))
            {
                ViewMode = viewMode;

                var syntaxTree = ProjectFile.AttachedData.Get(ProjectFileAttachedSyntaxTreeFactory.Instance).Get();
                if (syntaxTree != null)
                    _treeRoot = _treeBuilder.Create(ViewMode, syntaxTree);
                else
                    _treeRoot = null;
                _contentCreated = true;
            }

            return _treeRoot;
        }

        public List<SyntaxTreeViewerTreeNodeProperty>? GetProperties(string uid)
        {
            var node = _treeRoot?.Find(uid);
            if ((node?.SyntaxNode != null) && (node.Properties == null))
                node.Properties = _propertyListBuilder.CreateProperties(node.SyntaxNode);
            return node?.Properties;
        }

    }
}
