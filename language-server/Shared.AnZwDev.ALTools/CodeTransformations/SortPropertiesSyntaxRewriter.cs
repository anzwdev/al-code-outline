using AnZwDev.ALTools.Core;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class SortPropertiesSyntaxRewriter: ALSyntaxRewriter
    {

        #region Properties comparer

#if BC

        protected class PropertyComparer : IComparer<SyntaxNodeSortInfo<PropertySyntaxOrEmpty>>
        {
            protected static IComparer<string> _stringComparer = new SyntaxNodeNameComparer();

            public PropertyComparer()
            {
            }

            public int Compare(SyntaxNodeSortInfo<PropertySyntaxOrEmpty> x, SyntaxNodeSortInfo<PropertySyntaxOrEmpty> y)
            {
                int val = _stringComparer.Compare(x.Name, y.Name);
                if (val != 0)
                    return val;
                return x.Index - y.Index;
            }
        }
#else
        protected class PropertyComparer : IComparer<SyntaxNodeSortInfo<PropertySyntax>>
        {
            protected static IComparer<string> _stringComparer = new SyntaxNodeNameComparer();

            public PropertyComparer()
            {
            }

            public int Compare(SyntaxNodeSortInfo<PropertySyntax> x, SyntaxNodeSortInfo<PropertySyntax> y)
            {
                int val = _stringComparer.Compare(x.Name, y.Name);
                if (val != 0)
                    return val;
                return x.Index - y.Index;
            }
        }

#endif

        #endregion

        public SortPropertiesSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitPropertyList(PropertyListSyntax node)
        {
            if ((this.NodeInSpan(node)) && (node.Properties != null) && (node.Properties.Count > 1) && (!node.ContainsDiagnostics))
            {
                node = SortPropertyList(node, out bool sorted);
                if (sorted)
                    this.NoOfChanges++;
            }
            return base.VisitPropertyList(node);
        }

        public PropertyListSyntax SortPropertyList(PropertyListSyntax node, out bool sorted)
        {
#if BC
            SyntaxList<PropertySyntaxOrEmpty> properties =
                SyntaxNodesGroupsTree<PropertySyntaxOrEmpty>.SortSyntaxListWithSortInfo(
                    node.Properties, new PropertyComparer(), out sorted);
#else
            SyntaxList<PropertySyntax> properties =
                SyntaxNodesGroupsTree<PropertySyntax>.SortSyntaxListWithSortInfo(
                    node.Properties, new PropertyComparer(), out sorted);
#endif
            return node.WithProperties(properties);
        }



    }
}
