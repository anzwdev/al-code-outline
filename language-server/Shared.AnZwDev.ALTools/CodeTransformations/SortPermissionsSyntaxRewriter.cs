using AnZwDev.ALTools.ALSymbols;
using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class SortPermissionsSyntaxRewriter : ALSyntaxRewriter
    {

        public SortPermissionsSyntaxRewriter()
        {
        }

        public override SyntaxNode VisitPermissionPropertyValue(PermissionPropertyValueSyntax node)
        {
            if ((this.NodeInSpan(node)) && (!node.ContainsDiagnostics))
                node = node.WithPermissionProperties(this.Sort(node.PermissionProperties));
            return base.VisitPermissionPropertyValue(node);
        }

        protected SeparatedSyntaxList<PermissionSyntax> Sort(SeparatedSyntaxList<PermissionSyntax> permissions)
        {            
            var newPermissions = SyntaxNodesGroupsTree<PermissionSyntax>.SortSeparatedSyntaxList(permissions, new PermissionComparer(), out bool sorted);
            if (sorted)
                this.NoOfChanges++;
            return newPermissions;
        }


    }
}
