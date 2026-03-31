using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;

namespace AnZwDev.AL.Views.SyntaxTreeViewerTreeViews
{
    public class SyntaxTreeViewerTreeViewBuildersCollection
    {

        public Dictionary<SyntaxTreeViewerViewMode, SyntaxTreeViewerTreeViewBuilder> _builders { get; }

        public SyntaxTreeViewerTreeViewBuildersCollection()
        {
            _builders = new Dictionary<SyntaxTreeViewerViewMode, SyntaxTreeViewerTreeViewBuilder>
            {
                { SyntaxTreeViewerViewMode.ChildNodesBased, new ChildNodesBasedSyntaxTreeViewerTreeViewBuilder() },
                { SyntaxTreeViewerViewMode.PropertyBased, new PropertyBasedSyntaxTreeViewerTreeViewBuilder() }
            };
        }

        private SyntaxTreeViewerTreeViewBuilder? GetBuilder(SyntaxTreeViewerViewMode viewMode)
        {
            if (_builders.ContainsKey(viewMode))
                return _builders[viewMode];
            return null;
        }

        public SyntaxTreeViewerTreeNode? Create(SyntaxTreeViewerViewMode viewMode, SyntaxTree syntaxTree)
        {
            return GetBuilder(viewMode)?
                .Create(syntaxTree);
        }

    }
}
