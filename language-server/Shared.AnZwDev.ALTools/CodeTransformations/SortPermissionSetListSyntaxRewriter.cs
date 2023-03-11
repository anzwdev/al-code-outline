using AnZwDev.ALTools.Extensions;
using Microsoft.Dynamics.Nav.CodeAnalysis;
using Microsoft.Dynamics.Nav.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnZwDev.ALTools.CodeTransformations
{
    public class SortPermissionSetListSyntaxRewriter : ALSyntaxRewriter
    {

        public SortPermissionSetListSyntaxRewriter()
        {
        }

#if BC

        public override SyntaxNode VisitPermissionSet(PermissionSetSyntax node)
        {
            PropertySyntax property = node.PropertyList.GetPropertyEntry("IncludedPermissionSets");
            if ((property != null) && (this.NodeInSpan(property)) && (!property.ContainsDiagnostics))
            {
                PropertySyntax newProperty = this.Sort(property);
                node = node.WithPropertyList(node.PropertyList.WithProperties(node.PropertyList.Properties.Replace(property, newProperty)));
            }

            return base.VisitPermissionSet(node);
        }

        public override SyntaxNode VisitPermissionSetExtension(PermissionSetExtensionSyntax node)
        {
            PropertySyntax property = node.PropertyList.GetPropertyEntry("IncludedPermissionSets");
            if ((property != null) && (this.NodeInSpan(property)) && (!property.ContainsDiagnostics))
            {
                PropertySyntax newProperty = this.Sort(property);
                node = node.WithPropertyList(node.PropertyList.WithProperties(node.PropertyList.Properties.Replace(property, newProperty)));
            }

            return base.VisitPermissionSetExtension(node);
        }

#endif

        protected PropertySyntax Sort(PropertySyntax property)
        {
            property = property.SortCommaSeparatedPropertyValue(out bool sorted);
            if (sorted)
                NoOfChanges++;
            /*
            CommaSeparatedPropertyValueSyntax value = property.Value as CommaSeparatedPropertyValueSyntax;
            if (value != null)
            {
                value = value.WithValues(
                    SyntaxNodesGroupsTree<IdentifierNameSyntax>.SortSeparatedSyntaxList(value.Values, new IdentifierNameComparer(), out bool sorted));
                property = property.WithValue(value);
                if (sorted)
                    this.NoOfChanges++;
            }
            */
            return property;
        }

    }
}
