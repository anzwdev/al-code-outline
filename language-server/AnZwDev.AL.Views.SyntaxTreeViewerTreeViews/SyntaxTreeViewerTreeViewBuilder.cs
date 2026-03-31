using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnZwDev.AL.Views.SyntaxTreeViewerTreeViews
{

    public abstract class SyntaxTreeViewerTreeViewBuilder
    {

        public SyntaxTreeViewerTreeNode? Create(SyntaxTree syntaxTree)
        {
            var rootNode = syntaxTree.GetRoot();
            if (rootNode != null)
            {
                var rootTreeNode = Create(syntaxTree, rootNode);
                rootTreeNode.CalculateUid();
                return rootTreeNode;
            }
            return null;
        }

        protected abstract SyntaxTreeViewerTreeNode Create(SyntaxTree syntaxTree, SyntaxNode node);

    }

}
